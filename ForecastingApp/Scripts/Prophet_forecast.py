import pandas as pd
import numpy as np
from prophet import Prophet
from sklearn.metrics import mean_absolute_error, mean_squared_error, mean_absolute_percentage_error
from sklearn.preprocessing import StandardScaler
import matplotlib
matplotlib.use('TkAgg')  # Try different backends if needed: 'Agg', 'Qt5Agg', etc.
import matplotlib.pyplot as plt
import os
from prophet.make_holidays import make_holidays_df
import warnings
import sys
import traceback

def main():
    print("Running Prophet forecast script...")
    warnings.filterwarnings("ignore")
    plt.rcParams['figure.figsize'] = (17, 5)

    if len(sys.argv) != 15:
        print("Usage: Prophet_forecast.py <csv_file> <period> <split> <daily> <weekly> <monthly> <yearly> <holiday> <standardization> <growth> <seasonality_mode> <changepoint_pscale> <seasonality_pscale> <target>")
        # For testing purposes, use default values instead of exiting

    # Parse command line arguments
    csv_file = sys.argv[1]
    forecast_period = int(sys.argv[2])
    train_ratio = float(sys.argv[3])
    daily = sys.argv[4].lower() == 'true'
    weekly = sys.argv[5].lower() == 'true'
    monthly = sys.argv[6].lower() == 'true'
    yearly = sys.argv[7].lower() == 'true'
    holidays_enabled = sys.argv[8].lower() == 'true'
    standardization = sys.argv[9].lower() == 'true'
    growth = sys.argv[10]
    seasonality_mode = sys.argv[11]
    changepoint_prior_scale = float(sys.argv[12])
    seasonality_prior_scale = float(sys.argv[13])
    target_column = sys.argv[14]

    dataset_name = os.path.splitext(os.path.basename(csv_file))[0]
    target_name = target_column.capitalize()

    try:
        # ============================
        # Load Dataset
        # ============================

        df = pd.read_csv(csv_file, encoding='latin1')
        df = df.sample(frac=0.5, random_state=42)  # Downsample for faster runs
        print(df.head())

        # Clean column names as in collab.py
        df.columns = df.columns.str.strip().str.replace('﻿', '', regex=False)

        # ============================
        # Robust Date Parsing
        # ============================

        try:
            df['ds'] = pd.to_datetime(df['Date'])
            print("Date parsing succeeded with default parsing.")
        except Exception as e1:
            print("\nWarning: Default date parsing failed. Trying with dayfirst=True...")
            try:
                df['ds'] = pd.to_datetime(df['Date'], dayfirst=True, infer_datetime_format=True)
                print("Date parsing succeeded with dayfirst=True.")
            except Exception as e2:
                print("\nWarning: dayfirst=True parsing also failed. Trying mixed format with coercion...")
                try:
                    df['ds'] = pd.to_datetime(df['Date'], format='mixed', dayfirst=True, errors='coerce')
                    df = df.dropna(subset=['ds'])
                    print("Date parsing succeeded using mixed format with coercion.")
                except Exception as e3:
                    raise ValueError(
                        f"All date parsing strategies failed. Please check your 'Date' column format.\n"
                        f"Initial Error: {e1}\n"
                        f"Dayfirst Error: {e2}\n"
                        f"Final Fallback Error: {e3}"
                    )

        df = df.sort_values('ds')

        if target_column not in df.columns:
            raise ValueError(f"The dataset must contain a '{target_column}' column.")

        df['y'] = pd.to_numeric(df[target_column], errors='coerce')
        df = df.dropna(subset=['y'])

        # ============================
        # Optional Standardization
        # ============================
        if standardization:
            scaler = StandardScaler()
            df['y'] = scaler.fit_transform(df[['y']])

        # ============================
        # Split Data into Train/Test
        # ============================
        train_size = int(len(df) * train_ratio)
        train_data = df.iloc[:train_size][['ds', 'y']]
        test_data = df.iloc[train_size:][['ds', 'y']]
        test_data = test_data.dropna(subset=['y'])

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

        if daily:
            model.add_seasonality(name='daily', period=1, fourier_order=3)
        if weekly:
            model.add_seasonality(name='weekly', period=7, fourier_order=3)
        if monthly:
            model.add_seasonality(name='monthly', period=30.5, fourier_order=5)
        if yearly:
            model.add_seasonality(name='yearly', period=365.25, fourier_order=10)

        model.fit(train_data)

        # Create future dataframe for forecasting 
        # Use 'D' as default frequency if infer_freq fails
        freq = pd.infer_freq(df['ds'])
        if freq is None:
            freq = 'D'

        future = model.make_future_dataframe(periods=forecast_period, freq=freq)
        forecast = model.predict(future)

        # ============================
        # Align Forecast with Test Data - Using the approach from collab.py
        # ============================
        forecast_test = forecast.iloc[-len(test_data):]
        forecast_test = forecast_test.dropna(subset=['yhat'])
        forecast_test['yhat'] = forecast_test['yhat'].clip(lower=0)
        
        # Ensure forecast and test data have the same length
        min_len = min(len(test_data), len(forecast_test))
        test_data = test_data.iloc[:min_len]
        forecast_test = forecast_test.iloc[:min_len]

        # ============================
        # Model Evaluation
        # ============================
        if not test_data.empty:
            print("Test data is not empty, proceeding with evaluation...")
            print(f"Test data length: {len(test_data)}")
            print(f"Forecast test length: {len(forecast_test)}")
            
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
            # Plot Forecast with Metrics - Using function from collab.py
            # ============================
            try:
                def plot_forecast_with_metrics(train, test, forecast, mape, rmse, mae):
                    accuracy = 100 - (mape * 100)

                    fig, axs = plt.subplots(1, 2, figsize=(18, 6), gridspec_kw={'width_ratios': [4, 1]})
                    fig.suptitle(f'Forecast vs Actual - {dataset_name} ({target_name})', fontsize=16)

                    axs[0].plot(train['ds'], train['y'], label='Train', color='blue', linewidth=1.5)
                    axs[0].plot(test['ds'], test['y'], label='Test', color='orange', linewidth=1.5)
                    axs[0].plot(forecast['ds'], forecast['yhat'], label='Forecast', color='green', linewidth=2)

                    axs[0].fill_between(forecast['ds'], forecast['yhat_lower'], forecast['yhat_upper'],
                                       color='gray', alpha=0.3, label='Uncertainty Interval')

                    axs[0].set_xlabel('Date')
                    axs[0].set_ylabel(target_name)
                    axs[0].legend(loc='upper left')
                    axs[0].grid(True)

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
                    
                    # Save figure to file regardless of interactive display
                    plt.savefig(f'{dataset_name}_{target_name}_forecast.png')
                    print(f"Plot saved to {dataset_name}_{target_name}_forecast.png")
                    
                    # Try to show the figure interactively
                    plt.show()
                    print("Plot display attempted")

                print("About to call plotting function...")
                plot_forecast_with_metrics(train_data, test_data, forecast_test, mape, rmse, mae)
                print("Plot function completed successfully")
                
            except Exception as plot_error:
                print(f"Error in plotting function: {plot_error}")
                traceback.print_exc()
        else:
            print("Test data is empty, skipping evaluation and plotting.")

    except Exception as e:
        print("\nAn error occurred:")
        traceback.print_exc()

    print("\nScript completed. Press Enter to exit...")

if __name__ == "__main__":
    main()