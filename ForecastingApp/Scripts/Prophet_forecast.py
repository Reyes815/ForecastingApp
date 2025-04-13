import sys
import pandas as pd
import numpy as np
from prophet import Prophet
from sklearn.metrics import mean_absolute_error, mean_squared_error, mean_absolute_percentage_error
from sklearn.preprocessing import StandardScaler
import matplotlib.pyplot as plt
import os
from prophet.make_holidays import make_holidays_df
import warnings


def main():
    warnings.filterwarnings("ignore", category=FutureWarning)
    plt.rcParams['figure.figsize'] = (17, 5)

    if len(sys.argv) != 13:  # Expecting 12 arguments including script name
        print("Usage: Prophet_forecast.py <csv_file> <period> <split> <weekly> <monthly> <holiday> <standardization> <growth> <seasonality_mode> <changepoint_pscale> <seasonality_pscale> <target>")
        sys.exit(1)

    # ============================
    # Configurable Parameters
    # ============================

    csv_file = sys.argv[1]  # Path to dataset

    forecast_period = int(sys.argv[2])  
    # Number of future periods to forecast

    train_ratio = float(sys.argv[3])  
    # Train/Test Split Ratio

    weekly = sys.argv[4].lower() == 'true'  
    # Enable Weekly Seasonality

    monthly = sys.argv[5].lower() == 'true'
    # Enable Monthly Seasonality

    holidays_enabled = sys.argv[6].lower() == 'true'
    # Enable Holiday Effects

    standardization = sys.argv[7].lower() == 'true'  
    # Standardize Target Variable

    growth = sys.argv[8]  
    # Prophet Growth Type ('linear' or 'logistic')

    seasonality_mode = sys.argv[9]  
    # Seasonality Mode ('additive' or 'multiplicative')

    changepoint_prior_scale = sys.argv[10]
    # Changepoint Prior Scale (Trend Flexibility)

    seasonality_prior_scale = sys.argv[11]
    # Seasonality Prior Scale (Seasonality Flexibility)

    target_column = sys.argv[12]
    # Target Column to Forecast

    dataset_name = os.path.splitext(os.path.basename(csv_file))[0]
    target_name = target_column.capitalize()

    # ============================
    # Load and Prepare Dataset
    # ============================

    try:
        df = pd.read_csv(csv_file, encoding='latin1')
        df = df.sample(frac=0.3, random_state=42)  # Downsample for faster runs
        print(df.head())

        # Date Parsing Logic
        try:
            df['ds'] = pd.to_datetime(df['Date'])
        except Exception as e:
            print(f"\nWarning: Auto date parsing failed. Trying with dayfirst=True...")
            try:
                df['ds'] = pd.to_datetime(df['Date'], dayfirst=True, infer_datetime_format=True)
                print("Date parsing succeeded with dayfirst=True.")
            except Exception as e:
                raise ValueError(f"Date parsing failed. Please check your 'Date' column format.\nOriginal Error: {e}")

        df = df.sort_values('ds')

        if target_column not in df.columns:
            raise ValueError(f"The dataset must contain a '{target_column}' column.")

        df['y'] = pd.to_numeric(df[target_column], errors='coerce')
        df = df.dropna(subset=['y'])

        # ============================
        # Split Data into Train/Test
        # ============================

        train_size = int(len(df) * train_ratio)
        train_data = df.iloc[:train_size][['ds', 'y']]
        test_data = df.iloc[train_size:][['ds', 'y']]
        test_data = test_data.dropna(subset=['y'])

        # ============================
        # Optional Standardization
        # ============================

        if standardization:
            scaler = StandardScaler()
            df_train_scaled = train_data.copy()
            df_train_scaled['y'] = scaler.fit_transform(train_data[['y']])
            df_test_scaled = test_data.copy()
            df_test_scaled['y'] = scaler.transform(test_data[['y']])

        # ============================
        # Prophet Model Setup
        # ============================

        if holidays_enabled:
            year_list = df['ds'].dt.year.unique().tolist()
            holidays_df = make_holidays_df(year_list=year_list, country='US')
            model = Prophet(
                growth=growth,
                seasonality_mode=seasonality_mode,
                changepoint_prior_scale=changepoint_prior_scale,
                seasonality_prior_scale=seasonality_prior_scale,
                holidays=holidays_df
            )
            print("U.S. Holidays enabled in Prophet Model.")
        else:
            model = Prophet(
                growth=growth,
                seasonality_mode=seasonality_mode,
                changepoint_prior_scale=changepoint_prior_scale,
                seasonality_prior_scale=seasonality_prior_scale
            )
            print("Holidays disabled.")

        if weekly:
            model.add_seasonality(name='weekly', period=7, fourier_order=3)
        if monthly:
            model.add_seasonality(name='monthly', period=30.5, fourier_order=5)

        # Fit Model
        model.fit(train_data)

        # Generate Future Dataframe
        freq = pd.infer_freq(df['ds'])
        if freq is None:
            freq = 'D'
        future = model.make_future_dataframe(periods=forecast_period, freq=freq)

        # Forecast Future
        forecast = model.predict(future)

        # ============================
        # Align Forecast with Test Data
        # ============================

        forecast_test = forecast.dropna(subset=['yhat']).iloc[-len(test_data):]
        forecast_test['yhat'] = forecast_test['yhat'].clip(lower=0)

        # Ensure Forecast & Test Data Match Length
        test_data, forecast_test = test_data.iloc[:len(forecast_test)], forecast_test.iloc[:len(test_data)]

        # ============================
        # Model Evaluation
        # ============================

        if not test_data.empty:
            def calculate_metrics(true, predicted):
                mape = mean_absolute_percentage_error(true, predicted)
                rmse = np.sqrt(mean_squared_error(true, predicted))
                mae = mean_absolute_error(true, predicted)
                return mape, rmse, mae

            mape, rmse, mae = calculate_metrics(test_data['y'], forecast_test['yhat'])

            print(f"\nModel Evaluation Results for '{dataset_name}' [{target_name}]:")
            print(f"MAPE (Mean Absolute Percentage Error) = {round(mape * 100, 2)}%")
            print(f"RMSE (Root Mean Squared Error) = {round(rmse, 2)}")
            print(f"MAE (Mean Absolute Error) = {round(mae, 2)}")
            print(f"Accuracy = {round(100 - (mape * 100), 2)}%")

        # ============================
        # Enhanced Visualization (Side-by-Side Plot + Metrics)
        # ============================

        def plot_forecast_with_metrics(train, test, forecast, mape, rmse, mae):
            accuracy = 100 - (mape * 100)

            fig, axs = plt.subplots(1, 2, figsize=(18, 6), gridspec_kw={'width_ratios': [4, 1]})
            fig.suptitle(f'Forecast vs Actual - {dataset_name} ({target_name})', fontsize=16)

            # Forecast Line Plot
            axs[0].plot(train['ds'], train['y'], label='Train', color='blue', linewidth=1.5)
            axs[0].plot(test['ds'], test['y'], label='Test', color='orange', linewidth=1.5)
            axs[0].plot(forecast['ds'], forecast['yhat'], label='Forecast', color='green', linewidth=2)

            axs[0].fill_between(forecast['ds'], forecast['yhat_lower'], forecast['yhat_upper'],
                                color='gray', alpha=0.3, label='Uncertainty Interval')

            axs[0].set_xlabel('Date')
            axs[0].set_ylabel(target_name)
            axs[0].legend(loc='upper left')
            axs[0].grid(True)

            # Metrics Box (Text on Right)
            metrics_text = (
                f"MAPE: {round(mape * 100, 2)}%\n"
                f"RMSE: {round(rmse, 2)}\n"
                f"MAE: {round(mae, 2)}\n"
                f"Accuracy: {round(accuracy, 2)}%"
            )

            axs[1].axis('off')
            axs[1].text(0, 0.5, metrics_text, fontsize=14, va='center', ha='left', fontweight='bold')

            plt.tight_layout()
            plt.subplots_adjust(top=0.85)
            plt.show()


        # Call enhanced plot function
        plot_forecast_with_metrics(train_data, test_data, forecast_test, mape, rmse, mae)

    except Exception as e:
        print(f"\nError: {e}")

    input("\nPress Enter to exit...")