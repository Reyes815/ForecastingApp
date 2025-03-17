import pandas as pd
import numpy as np
import warnings


def is_date_column(series, threshold=0.1):
    """Check if a Pandas Series is mostly convertible to datetime, suppressing warnings."""
    with warnings.catch_warnings():
        warnings.simplefilter("ignore", category=UserWarning)  # Suppress warnings

        try:
            converted = pd.to_datetime(series, errors="coerce")
            valid_ratio = converted.notna().mean()  # Ratio of valid (non-NaT) values

            return valid_ratio >= threshold  # True if at least `threshold` fraction is valid
        except Exception:
            return False  # Catch unexpected errors silently

def categorize_features(df):
    feature_types = {}

    # Convert only columns that look like dates
    for col in df.columns:
        with warnings.catch_warnings():
            warnings.simplefilter("ignore", category=UserWarning)  # Suppress warnings

            if df[col].dtype == "object" and is_date_column(df[col]):
                df[col] = pd.to_datetime(df[col], errors="coerce")

    for col in df.columns:
        unique_vals = df[col].dropna().unique()
        num_unique = len(unique_vals)
        total_vals = len(df[col])
        dtype = df[col].dtype

        if np.issubdtype(dtype, np.datetime64):
            feature_types[col] = "Time-Series / DateTime"
        elif np.issubdtype(dtype, np.number):
            if num_unique == 2:
                feature_types[col] = "Binary"
            elif num_unique < 0.05 * total_vals:
                feature_types[col] = "Discrete Numerical"
            else:
                feature_types[col] = "Continuous Numerical"
        elif np.issubdtype(dtype, np.object_) or np.issubdtype(dtype, pd.CategoricalDtype):
            if num_unique == 2:
                feature_types[col] = "Binary"
            elif num_unique < 0.05 * total_vals:
                feature_types[col] = "Categorical (Nominal)"
            else:
                feature_types[col] = "Text"
        else:
            feature_types[col] = "Unknown"

    return feature_types