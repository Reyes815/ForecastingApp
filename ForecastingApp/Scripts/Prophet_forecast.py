import sys
import pandas as pd
import numpy as np
from prophet import Prophet
from sklearn.metrics import mean_absolute_error, mean_squared_error
from sklearn.preprocessing import StandardScaler

def main():
    if len(sys.argv) != 13:  # Expecting 12 arguments including script name
        print("Usage: Prophet_forecast.py <csv_file> <period> <train> <test> <split> <weekly> <monthly> <holiday> <standardization> <growth> <seasonality_mode>")
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
        target_column = sys.argv[10]  # Dynamically passed column name for target
        growth = sys.argv[11]  # "linear" or "logistic"
        seasonality_mode = sys.argv[12]  # "additive" or "multiplicative"

        # Load dataset
        df = pd.read_csv(csv_file, parse_dates=['Date'], encoding='latin1')
        df = df.sort_values('Date')

        # Ensure the target column exists
        if target_column not in df.columns:
            raise ValueError(f"The dataset must contain a '{target_column}' column.")

        df['ds'] = pd.to_datetime(df['Date'])
        df['y'] = pd.to_numeric(df[target_column], errors='coerce')  # Use the user-selected target column
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

    except Exception as e:
        print(f"Error running Prophet: {e}")
        sys.exit(1)

    # Add a prompt to keep the script running and not exit immediately
    input("Press Enter to exit...")

if __name__ == "__main__":
    main()
