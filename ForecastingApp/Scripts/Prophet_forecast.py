import sys
import pandas as pd
import numpy as np
from prophet import Prophet
from sklearn.metrics import mean_absolute_error, mean_squared_error
from sklearn.preprocessing import StandardScaler

def main():
    if len(sys.argv) != 13:  # Fix: Expecting 12 arguments including script name
        print("Usage: Prophet_forecast.py <csv_file> <period> <train> <test> <split> <weekly> <monthly> <holiday> <standardization> <growth> <seasonality_mode>")
        print(sys.argv)
        input("sdfsdfsdf")
        sys.exit(1)

    try:
        # Parse command-line arguments
        csv_file = sys.argv[1]
        period = int(sys.argv[2])
        train_ratio = float(sys.argv[3])
        test_ratio = float(sys.argv[4])
        split_ratio = float(sys.argv[5])
        weekly = sys.argv[6].lower() == 'true'
        monthly = sys.argv[7].lower() == 'true'
        holiday = sys.argv[8].lower() == 'true'
        standardization = sys.argv[9].lower() == 'true'
        growth = sys.argv[10]  # "linear" or "logistic"
        seasonality_mode = sys.argv[11]  # "additive" or "multiplicative"

        print(f"Running Prophet with period={period}, train={train_ratio}, test={test_ratio}, split={split_ratio}, weekly={weekly}, monthly={monthly}, holiday={holiday}, standardization={standardization}, growth={growth}, seasonality_mode={seasonality_mode}")
        # input("sdfsdjkflskdjf")

        # Load dataset
        df = pd.read_csv(csv_file, parse_dates=['Date'], encoding='latin1')
        df = df.sort_values('Date')

        # Ensure 'Profit' column exists
        if 'Profit' not in df.columns:
            raise ValueError("The dataset must contain a 'Profit' column.")

        df['ds'] = pd.to_datetime(df['Date'])
        df['y'] = pd.to_numeric(df['Profit'], errors='coerce')
        df = df.dropna(subset=['y'])

        # Standardize data if enabled
        if standardization:
            scaler = StandardScaler()
            df['y'] = scaler.fit_transform(df[['y']])

        # Split data into train/test
        train_size = int(train_ratio * len(df))
        train_data = df.iloc[:train_size][['ds', 'y']]
        test_data = df.iloc[train_size:][['ds', 'y']]

        # Initialize Prophet model
        model = Prophet(growth=growth, seasonality_mode=seasonality_mode)

        if weekly:
            model.add_seasonality(name='weekly', period=7, fourier_order=3)
        if monthly:
            model.add_seasonality(name='monthly', period=30.5, fourier_order=5)

        model.fit(train_data)

        # Forecast
        future = model.make_future_dataframe(periods=period, freq='D')
        forecast = model.predict(future)

        # Evaluate model
        if len(test_data) > 0:
            forecast_test = forecast.iloc[-len(test_data):]
            mae = mean_absolute_error(test_data['y'], forecast_test['yhat'])
            rmse = np.sqrt(mean_squared_error(test_data['y'], forecast_test['yhat']))
            print(f"Model Evaluation: MAE = {mae:.4f}, RMSE = {rmse:.4f}")

        print(forecast[['ds', 'yhat', 'yhat_lower', 'yhat_upper']].head())

    except IndexError as e:
        print(f"Error: Missing required arguments. Expected 11, got {len(sys.argv) - 1}. {e}")
        sys.exit(1)
    except Exception as e:
        print(f"Error running Prophet: {e}")
        sys.exit(1)

if __name__ == "__main__":
    main()
