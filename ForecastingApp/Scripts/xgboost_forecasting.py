import numpy as np
import pandas as pd
from sklearn.metrics import mean_absolute_error, mean_squared_error, mean_absolute_percentage_error
from sklearn.model_selection import train_test_split, GridSearchCV
import xgboost as xgb
from joblib import dump
import os
import sys

print("Received arguments:", sys.argv)

def run(selectedCSVFile, param_dict=None, num_lags=5, test_size=0.2):
    """Train an XGBoost model with hyperparameter tuning if user inputs values."""

    if not os.path.exists(selectedCSVFile):
        raise FileNotFoundError(f"Error: File '{selectedCSVFile}' does not exist.")

    # Load and preprocess data
    data = pd.read_csv(selectedCSVFile)

    if data.empty:
        raise ValueError("Error: CSV file is empty or not formatted correctly.")

    target_column = data.columns[-1]
    X = data.drop(columns=[target_column])
    y = pd.to_numeric(data[target_column], errors='coerce').dropna()

    # Align X with cleaned target
    X = X.loc[y.index]

    # Dynamically detect categorical columns
    cat_cols = X.select_dtypes(include=["object"]).columns.tolist()
    
    # Convert categorical columns to category type
    for col in cat_cols:
        X[col] = X[col].astype("category")

    # Detect date-like columns and convert them to numerical timestamps
    date_cols = [col for col in X.columns if "date" in col.lower()]

    for col in date_cols:
        X[col] = pd.to_datetime(X[col], errors="coerce")
        X[col] = X[col].astype(int) // 10**9  # Convert to Unix timestamp

    # Ensure categorical encoding works with XGBoost
    dtrain = xgb.DMatrix(X, label=y, enable_categorical=True)

    # Split data
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=test_size, random_state=42)

    # Default hyperparameter grid
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

    # Initialize XGBoost model
    xgb_model = xgb.XGBRegressor(random_state=42, enable_categorical=True)

    # Hyperparameter tuning
    grid_search = GridSearchCV(xgb_model, param_grid, scoring='neg_mean_squared_error', cv=3, verbose=1, n_jobs=1)
    grid_search.fit(X_train, y_train)

    # Get the best model
    best_model = grid_search.best_estimator_

    # Save trained model
    dump(best_model, 'best_xgb_model.pkl')

    # Evaluate
    y_pred = best_model.predict(X_test)
    results = {
        "best_params": grid_search.best_params_,
        "mae": mean_absolute_error(y_test, y_pred),
	"rmse": mean_squared_error(y_test, y_pred) ** 0.5,
        "mape": mean_absolute_percentage_error(y_test, y_pred)
    }

    return results