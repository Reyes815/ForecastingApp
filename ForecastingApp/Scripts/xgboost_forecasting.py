import numpy as np
import pandas as pd
from sklearn.metrics import mean_absolute_error, mean_squared_error, mean_absolute_percentage_error, r2_score
from sklearn.model_selection import GridSearchCV
import xgboost as xgb
from joblib import dump
import matplotlib.pyplot as plt
import os
import time

def run(selectedCSVFile, param_dict=None, num_lags=5, test_size=0.2):
    """Train an XGBoost model and visualize results in Prophet-style."""

    param_dict = dict(param_dict) if param_dict is not None else {}

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
        X[date_col] = X[date_col].apply(lambda x: x.timestamp() if not pd.isnull(x) else np.nan)
    else:
        dates_for_plot = pd.Series(index=X.index, data=range(len(X)))

    # Create lag features
    for lag in range(1, num_lags + 1):
        X[f"lag_{lag}"] = y.shift(lag)

    valid_idx = X.dropna().index
    X = X.loc[valid_idx]
    y = y.loc[valid_idx]
    dates_for_plot = dates_for_plot.loc[valid_idx]

    # Chronological split
    split_index = int(len(X) * (1 - test_size))
    X_train, X_test = X.iloc[:split_index], X.iloc[split_index:]
    y_train, y_test = y.iloc[:split_index], y.iloc[split_index:]
    dates_train, dates_test = dates_for_plot.iloc[:split_index], dates_for_plot.iloc[split_index:]

    # Default hyperparameters
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
    grid_search = GridSearchCV(
        xgb_model,
        param_grid,
        scoring='neg_mean_squared_error',
        cv=3,
        verbose=1,
        n_jobs=1
    )
    grid_search.fit(X_train, y_train)
    end_time = time.time()

    best_model = grid_search.best_estimator_
    dump(best_model, 'best_xgb_model.pkl')

    # Evaluate
    y_pred = best_model.predict(X_test)
    mape = mean_absolute_percentage_error(y_test, y_pred)
    realistic_accuracy = max(0, 100 - (mape * 100))

    results = {
        "best_params": grid_search.best_params_,
        "mae": mean_absolute_error(y_test, y_pred),
        "rmse": mean_squared_error(y_test, y_pred) ** 0.5,
        "mape": mape,
        "r2": r2_score(y_test, y_pred),
        "realistic_accuracy": realistic_accuracy,
        "training_duration": round(end_time - start_time, 2)
    }

    plot_forecast_with_metrics(
        train_dates=dates_train,
        train_y=y_train,
        test_dates=dates_test,
        test_y=y_test,
        y_pred=y_pred,
        metrics=results
    )

    return results

def plot_forecast_with_metrics(train_dates, train_y, test_dates, test_y, y_pred, metrics):
    fig, axs = plt.subplots(1, 2, figsize=(18, 6), gridspec_kw={'width_ratios': [4, 1]})
    fig.suptitle("XGBoost Forecast vs Actual", fontsize=16)

    train_sorted = pd.DataFrame({"dates": train_dates, "y": train_y}).sort_values(by="dates")
    test_sorted = pd.DataFrame({"dates": test_dates, "y_true": test_y, "y_pred": y_pred}).sort_values(by="dates")

    axs[0].plot(train_sorted["dates"], train_sorted["y"], label="Train (Actual)", color="blue", linewidth=1.5)
    axs[0].plot(test_sorted["dates"], test_sorted["y_true"], label="Test (Actual)", color="orange", linewidth=1.5)
    axs[0].plot(test_sorted["dates"], test_sorted["y_pred"], label="Test (Forecast)", color="green", linestyle="--", linewidth=2)

    split_date = test_sorted["dates"].iloc[0]
    axs[0].axvline(x=split_date, color='black', linestyle=':', linewidth=1.5, label="Train/Test Split")

    axs[0].set_xlabel("Date")
    axs[0].set_ylabel("Target Value")
    axs[0].legend(loc="upper left")
    axs[0].grid(True)

    metrics_text = (
        f"MAPE: {round(metrics['mape'] * 100, 2)}%\n"
        f"RMSE: {round(metrics['rmse'], 2)}\n"
        f"MAE: {round(metrics['mae'], 2)}\n"
        f"R²: {round(metrics['r2'], 3)}\n"
        f"Accuracy: {round(metrics['realistic_accuracy'], 2)}%\n"
        f"Train Time: {metrics['training_duration']}s"
    )

    axs[1].axis("off")
    axs[1].text(0, 0.5, metrics_text, fontsize=14, va="center", ha="left", fontweight="bold")

    plt.tight_layout()
    plt.subplots_adjust(top=0.85)
    plt.savefig("xgboost_forecast_with_train_test_forecast.png")
    plt.show()

if __name__ == "__main__":
    import argparse
    parser = argparse.ArgumentParser()
    parser.add_argument("file", help="Path to the CSV file")
    parser.add_argument("--max_depth", type=int)
    parser.add_argument("--learning_rate", type=float)
    parser.add_argument("--n_estimators", type=int)
    parser.add_argument("--subsample", type=float)
    parser.add_argument("--colsample_bytree", type=float)
    parser.add_argument("--reg_alpha", type=float)
    parser.add_argument("--reg_lambda", type=float)
    parser.add_argument("--num_lags", type=int, default=5)
    parser.add_argument("--test_size", type=float, default=0.2)
    args = parser.parse_args()

    param_dict = {k: v for k, v in vars(args).items() if k not in ["file", "num_lags", "test_size"] and v is not None}
    run(args.file, param_dict, num_lags=args.num_lags, test_size=args.test_size)