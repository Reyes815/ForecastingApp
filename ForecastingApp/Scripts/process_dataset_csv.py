import sys
import pandas as pd
import json
import os
import pickle
from sklearn.preprocessing import MinMaxScaler
from categorizing_features import categorize_features


def remove_text_columns(df):
    """Removes text-based (object) columns from the dataset."""
    text_columns = df.select_dtypes(include=['object']).columns.tolist()

    if text_columns:
        df.drop(columns=text_columns, inplace=True)

    return df, text_columns  # Return modified df and removed columns

def convert_datetime_columns(df):
    """Detects datetime columns and converts them into numerical features."""
    datetime_columns = [col for col in df.select_dtypes(include=['datetime'])]

    if datetime_columns:
        # Sort DataFrame by the first datetime column
        df = df.sort_values(by=datetime_columns[0])

        # Convert datetime columns to Unix timestamp
        for col in datetime_columns:
            df[f"{col}_timestamp"] = df[col].astype('int64') // 10 ** 9  # Convert to seconds
            df.drop(columns=[col], inplace=True)  # Remove original datetime column

    return df

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
        df = pd.read_csv(file_path, encoding='windows-1252')

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

        # Convert datetime columns into numerical features
        df = convert_datetime_columns(df)

        categorical_features = [col for col in df.columns if feature_categories.get(col, "Unknown") == "Categorical (Nominal)"]
        df_encoded = pd.get_dummies(df, columns=categorical_features)


        # Identify numerical features for scaling (exclude categorical ones)
        numerical_features = [col for col in df_encoded.columns if df_encoded[col].dtype in ["int64", "float64"]]

        # Apply MinMax scaling
        scaler = MinMaxScaler()
        df_encoded[numerical_features] = scaler.fit_transform(df_encoded[numerical_features])

        # Detect and remove ID column
        id_column = detect_id_column(df_encoded)
        if id_column:
            df_encoded.drop(columns=[id_column], inplace=True)

        # Remove text columns
        df_encoded, removed_text_columns = remove_text_columns(df_encoded)

        # Create "Saved preprocessed csv" folder if it doesn't exist
        script_dir = os.path.dirname(os.path.abspath(__file__))
        save_dir = os.path.join(script_dir, "Saved preprocessed csv")

        # Generate Pickle file path
        pickle_filename = os.path.splitext(os.path.basename(file_path))[0] + "_processed.pkl"
        pickle_path = os.path.join(save_dir, pickle_filename)

        pickle_scaler = os.path.splitext(os.path.basename(file_path))[0] + "_scaler.pkl"
        pickle_scaler_path = os.path.join(save_dir, pickle_scaler)

        # Save the processed DataFrame as a Pickle file
        with open(pickle_path, "wb") as f:
            pickle.dump(df_encoded, f)

        with open(pickle_scaler_path, "wb") as f:
            pickle.dump(scaler, f)

        # Prepare results
        result = {
            "filename": file_path.split("/")[-1],  # Get filename from path
            "saved_pickle": pickle_filename,
            "saved_scaler": pickle_scaler,
            "instances": len(df),
            "features_before_encoding": len(features),
            "features_after_encoding": len(df_encoded.columns),
            "removed_text_columns": removed_text_columns,  # Track removed columns
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