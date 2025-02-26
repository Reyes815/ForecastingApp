import pandas as pd
import numpy as np

def categorize_features(df):
    feature_types = {}

    for col in df.columns:
        unique_vals = df[col].dropna().unique()
        num_unique = len(unique_vals)
        total_vals = len(df[col])
        dtype = df[col].dtype

        if np.issubdtype(dtype, np.number):
            if num_unique == 2:
                feature_types[col] = "Binary"
            elif num_unique < 0.05 * total_vals:  # Arbitrary threshold for discrete
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

        elif np.issubdtype(dtype, np.datetime64):
            feature_types[col] = "Time-Series / DateTime"

        else:
            feature_types[col] = "Unknown"

    return feature_types