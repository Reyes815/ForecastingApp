import numpy as np
import pandas as pd
from sklearn.metrics import mean_absolute_error, mean_squared_error, mean_absolute_percentage_error, r2_score
from sklearn.model_selection import GridSearchCV
import xgboost as xgb
from joblib import dump
import matplotlib.pyplot as plt
import os
import sys
import time

print("Received arguments:", sys.argv)

def run(selectedCSVFile, param_dict=None, num_lags=5, test_size=0.2):
    """Train an XGBoost model and visualize results in Prophet-style."""

    if not os.path.exists(selectedCSVFile):
        raise FileNotFoundError(f"Error: File '{selectedCSVFile}' does not exist.")

    # Load and preprocess data
    data = pd.read_csv(selectedCSVFile)
    if data.empty:
        raise ValueError("Error: CSV file is empty or not formatted correctly.")

    target_column = data.columns[-1]
    X = data.drop(columns=[target_column])
    y = pd.to_numeric(data[target_column], errors='coerce').dropna()
    X = X.loc[y.index]

    # Handle categorical columns
    cat_cols = X.select_dtypes(include=["object"]).columns.tolist()
    for col in cat_cols:
        X[col] = X[col].astype("category")

    # Handle date column
    date_col = next((col for col in X.columns if "date" in col.lower()), None)
    if date_col:
        X[date_col] = pd.to_datetime(X[date_col], errors="coerce")
        dates_for_plot = X[date_col].copy()
        X[date_col] = X[date_col].astype("int64") // 10**9
    else:
        dates_for_plot = pd.Series(index=X.index, data=range(len(X)))

    # ✅ Chronological split (Prophet-style)
    split_index = int(len(X) * (1 - test_size))
    X_train, X_test = X.iloc[:split_index], X.iloc[split_index:]
    y_train, y_test = y.iloc[:split_index], y.iloc[split_index:]
    dates_train, dates_test = dates_for_plot.iloc[:split_index], dates_for_plot.iloc[split_index:]

    # Hyperparameters (default or passed)
    param_grid = {
        'objective': ['reg:squarederror'],
        'max_depth': [param_dict.get("max_depth", 6)],
        'learning_rate': [param_dict.get("learning_rate", 0.1)],
        'n_estimators': [param_dict.get("n_estimators", 100)],
        'subsample': [param_dict.get("subsample", 1.0)],
        'colsample_bytree': [param_dict.get("colsample_bytree", 0.8)],
        'reg_lambda': [param_dict.get("reg_lambda", 1)],
        'reg_alpha': [param_dict.get("reg_alpha", 0)]
    }

    # Train model
    xgb_model = xgb.XGBRegressor(random_state=42, enable_categorical=True)
    start_time = time.time()
    grid_search = GridSearchCV(xgb_model, param_grid, scoring='neg_mean_squared_error', cv=3, verbose=1, n_jobs=1)
    grid_search.fit(X_train, y_train)
    end_time = time.time()

    best_model = grid_search.best_estimator_
    dump(best_model, 'best_xgb_model.pkl')

    # Evaluate
    y_pred = best_model.predict(X_test)
    results = {
        "best_params": grid_search.best_params_,
        "mae": mean_absolute_error(y_test, y_pred),
        "rmse": mean_squared_error(y_test, y_pred) ** 0.5,
        "mape": mean_absolute_percentage_error(y_test, y_pred),
        "r2": r2_score(y_test, y_pred),
        "training_duration": round(end_time - start_time, 2)
    }

    # 📊 Prophet-style forecast visualization
    def plot_forecast_with_metrics(train_dates, train_y, test_dates, test_y, y_pred, metrics):
        accuracy = 100 - (metrics["mape"] * 100)

        fig, axs = plt.subplots(1, 2, figsize=(18, 6), gridspec_kw={'width_ratios': [4, 1]})
        fig.suptitle("XGBoost Forecast vs Actual", fontsize=16)

        # Forecast plot
        axs[0].plot(train_dates, train_y, label="Train", color="blue", linewidth=1.5)
        axs[0].plot(test_dates, test_y, label="Test", color="orange", linewidth=1.5)
        axs[0].plot(test_dates, y_pred, label="Forecast", color="green", linestyle="--", linewidth=2)

        axs[0].set_xlabel("Date")
        axs[0].set_ylabel("Target Value")
        axs[0].legend(loc="upper left")
        axs[0].grid(True)

        # Metrics panel
        metrics_text = (
            f"MAPE: {round(metrics['mape'] * 100, 2)}%\n"
            f"RMSE: {round(metrics['rmse'], 2)}\n"
            f"MAE: {round(metrics['mae'], 2)}\n"
            f"R²: {round(metrics['r2'], 3)}\n"
            f"Accuracy: {round(accuracy, 2)}%\n"
            f"Train Time: {metrics['training_duration']}s"
        )

        axs[1].axis("off")
        axs[1].text(0, 0.5, metrics_text, fontsize=14, va="center", ha="left", fontweight="bold")

        plt.tight_layout()
        plt.subplots_adjust(top=0.85)
        plt.savefig("xgboost_forecast_with_train_test_forecast.png")
        plt.show()

    # Call plot function
    plot_forecast_with_metrics(
        train_dates=dates_train,
        train_y=y_train,
        test_dates=dates_test,
        test_y=y_test,
        y_pred=y_pred,
        metrics=results
    )

    return results
