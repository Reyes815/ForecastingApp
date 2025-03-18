import sys
import pandas as pd
import json
import os
import pickle
from sklearn.preprocessing import MinMaxScaler
from categorizing_features import categorize_features

def detect_id_column(df):
    """Detects and removes an ID column based on naming and uniqueness."""
    id_candidates = [col for col in df.columns if "id" in col.lower()]
    unique_counts = df.nunique()

    for col in id_candidates:
        if unique_counts[col] == len(df):  # Likely an ID column
            return col

    return None  # No ID column detected


def process_csv(file_path):
    try:
        # Load the CSV file
        df = pd.read_csv(file_path)

        # Detect and remove ID column
        id_column = detect_id_column(df)
        if id_column:
            df.drop(columns=[id_column], inplace=True)

        # Extract feature names
        features = df.columns.tolist()

        # Get data types
        data_types = df.dtypes.astype(str).tolist()

        # Get unique value counts
        unique_values = df.nunique().tolist()

        # Get null value counts
        null_values = df.isnull().sum().tolist()

        # Categorize features
        feature_categories = categorize_features(df)  # New step

        categorical_features = [col for col in df.columns if feature_categories.get(col, "Unknown") == "Categorical (Nominal)"]
        df_encoded = pd.get_dummies(df, columns=categorical_features)


        # Identify numerical features for scaling (exclude categorical ones)
        numerical_features = [col for col in df_encoded.columns if df_encoded[col].dtype in ["int64", "float64"]]

        # Apply MinMax scaling
        scaler = MinMaxScaler()
        df_encoded[numerical_features] = scaler.fit_transform(df_encoded[numerical_features])

        # Create "Saved preprocessed csv" folder if it doesn't exist
        save_dir = os.path.join(os.getcwd(), "Saved preprocessed csv")
        os.makedirs(save_dir, exist_ok=True)

        # Generate Pickle file path
        pickle_filename = os.path.splitext(os.path.basename(file_path))[0] + "_processed.pkl"
        pickle_path = os.path.join(save_dir, pickle_filename)

        # Save the processed DataFrame as a Pickle file
        with open(pickle_path, "wb") as f:
            pickle.dump(df_encoded, f)

        # Prepare results
        result = {
            "filename": file_path.split("/")[-1],  # Get filename from path
            "instances": len(df),
            "features_before_encoding": len(features),
            "features_after_encoding": len(df_encoded.columns),
            "columns": [
                {"name": features[i],
                 "type": data_types[i],
                 "unique_values": unique_values[i],
                 "category": feature_categories.get(features[i], "Unknown"),  # Add categorized type
                 "null_values": null_values[i]
                 }
                for i in range(len(features))
            ]
        }

        # Print JSON output to be captured by C#
        print(json.dumps(result))

    except Exception as e:
        print(json.dumps({"error": str(e)}))

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print(json.dumps({"error": "No CSV file path provided"}))
    else:
        process_csv(sys.argv[1])