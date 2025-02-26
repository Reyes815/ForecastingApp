import sys
import pandas as pd
import json
from categorizing_features import categorize_features

def process_csv(file_path):
    try:
        # Load the CSV file
        df = pd.read_csv(file_path)

        # Extract feature names
        features = df.columns.tolist()

        # Get data types
        data_types = df.dtypes.astype(str).tolist()

        # Get unique value counts
        unique_values = df.nunique().tolist()

        # Categorize features
        feature_categories = categorize_features(df)  # New step

        # Prepare results
        result = {
            "filename": file_path.split("/")[-1],  # Get filename from path
            "instances": len(df),
            "features_before_encoding": len(features),
            "columns": [
                {"name": features[i], "type": data_types[i], "unique_values": unique_values[i], "category": feature_categories.get(features[i], "Unknown")  # Add categorized type
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

