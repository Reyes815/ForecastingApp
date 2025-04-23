import pandas as pd
import numpy as np
from prophet import Prophet
from sklearn.metrics import mean_absolute_error, mean_squared_error, mean_absolute_percentage_error
from sklearn.preprocessing import StandardScaler
import matplotlib
matplotlib.use('Agg')  # Use non-interactive backend
import matplotlib.pyplot as plt
import os
from prophet.make_holidays import make_holidays_df
import warnings
import sys
import traceback
import json

def main():
    print("Running Prophet forecast script...")
    warnings.filterwarnings("ignore")
    plt.rcParams['figure.figsize'] = (17, 5)

    if len(sys.argv) != 15:
        print("Usage: Prophet_forecast.py <csv_file> <period> <split> <daily> <weekly> <monthly> <yearly> <holiday> <standardization> <growth> <seasonality_mode> <changepoint_pscale> <seasonality_pscale> <target>")
        return

    # Parse arguments
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
        # Load dataset
        df = pd.read_csv(csv_file, encoding='latin1')
        df.columns = df.columns.str.strip().str.replace('﻿', '', regex=False)
        df = df.sample(frac=0.5, random_state=42)

        # Parse date
        try:
            df['ds'] = pd.to_datetime(df['Date'])
        except Exception:
            try:
                df['ds'] = pd.to_datetime(df['Date'], dayfirst=True, infer_datetime_format=True)
            except Exception:
                df['ds'] = pd.to_datetime(df['Date'], format='mixed', dayfirst=True, errors='coerce')
                df = df.dropna(subset=['ds'])

        df = df.sort_values('ds')

        if target_column not in df.columns:
            raise ValueError(f"'{target_column}' not found in dataset.")

        df['y'] = pd.to_numeric(df[target_column], errors='coerce')
        df = df.dropna(subset=['y'])

        if standardization:
            df['y'] = StandardScaler().fit_transform(df[['y']])

        # Split data
        train_size = int(len(df) * train_ratio)
        train_data = df.iloc[:train_size][['ds', 'y']]
        test_data = df.iloc[train_size:][['ds', 'y']].dropna()

        # Build Prophet model
        if holidays_enabled:
            year_list = df['ds'].dt.year.unique().tolist()
            holidays_df = make_holidays_df(year_list=year_list, country='US')
            model = Prophet(growth=growth, seasonality_mode=seasonality_mode,
                            changepoint_prior_scale=changepoint_prior_scale,
                            seasonality_prior_scale=seasonality_prior_scale,
                            holidays=holidays_df)
        else:
            model = Prophet(growth=growth, seasonality_mode=seasonality_mode,
                            changepoint_prior_scale=changepoint_prior_scale,
                            seasonality_prior_scale=seasonality_prior_scale)

        if daily:
            model.add_seasonality(name='daily', period=1, fourier_order=3)
        if weekly:
            model.add_seasonality(name='weekly', period=7, fourier_order=3)
        if monthly:
            model.add_seasonality(name='monthly', period=30.5, fourier_order=5)
        if yearly:
            model.add_seasonality(name='yearly', period=365.25, fourier_order=10)

        model.fit(train_data)

        freq = pd.infer_freq(df['ds']) or 'D'
        future = model.make_future_dataframe(periods=forecast_period, freq=freq)
        forecast = model.predict(future)

        # Trim forecast to match test
        forecast_test = forecast.iloc[-len(test_data):].dropna(subset=['yhat'])
        forecast_test['yhat'] = forecast_test['yhat'].clip(lower=0)
        min_len = min(len(test_data), len(forecast_test))
        test_data = test_data.iloc[:min_len]
        forecast_test = forecast_test.iloc[:min_len]

        # Evaluation
        def calculate_metrics(true, predicted):
            mape = mean_absolute_percentage_error(true, predicted)
            rmse = np.sqrt(mean_squared_error(true, predicted))
            mae = mean_absolute_error(true, predicted)
            return mape, rmse, mae

        mape, rmse, mae = calculate_metrics(test_data['y'], forecast_test['yhat'])
        accuracy = 100 - (mape * 100)

        # =======================
        # Save Plot + JSON Output
        # =======================

        os.makedirs("output", exist_ok=True)

        # Plot
        fig, axs = plt.subplots(1, 2, figsize=(18, 6), gridspec_kw={'width_ratios': [4, 1]})
        fig.suptitle(f'Forecast vs Actual - {dataset_name} ({target_name})', fontsize=16)

        axs[0].plot(train_data['ds'], train_data['y'], label='Train', color='blue')
        axs[0].plot(test_data['ds'], test_data['y'], label='Test', color='orange')
        axs[0].plot(forecast['ds'], forecast['yhat'], label='Forecast', color='green')
        axs[0].fill_between(forecast['ds'], forecast['yhat_lower'], forecast['yhat_upper'], color='gray', alpha=0.3)
        axs[0].legend()
        axs[0].grid(True)

        metrics_text = f"MAPE: {round(mape * 100, 2)}%\nRMSE: {round(rmse, 2)}\nMAE: {round(mae, 2)}\nAccuracy: {round(accuracy, 2)}%"
        axs[1].axis('off')
        axs[1].text(0, 0.5, metrics_text, fontsize=14, va='center', ha='left', fontweight='bold')

        plot_path = f"output/{dataset_name}_{target_name}_forecast.png"
        plt.tight_layout()
        plt.savefig(plot_path)
        print(f"Plot saved to {plot_path}")

        # JSON
        output_data = {
            "metrics": {
                "mape": round(mape * 100, 2),
                "rmse": round(rmse, 2),
                "mae": round(mae, 2),
                "accuracy": round(accuracy, 2)
            },
            "forecast": forecast_test[['ds', 'yhat', 'yhat_lower', 'yhat_upper']].assign(
                ds=forecast_test['ds'].dt.strftime('%Y-%m-%d')
            ).to_dict(orient='records')
        }

        with open("output/forecast_data.json", "w") as f:
            json.dump(output_data, f, indent=2)

        print("JSON data saved to output/forecast_data.json")

    except Exception as e:
        print("Error occurred:")
        traceback.print_exc()

    print("Script completed.")

if __name__ == "__main__":
    main()
