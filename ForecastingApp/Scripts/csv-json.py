import pandas as pd
import json
import sys
from datetime import datetime

def csv_to_gemini_json(csv_file_path):
    try:
        df = pd.read_csv(csv_file_path)

        df = df.sample(n=min(1000, len(df)), random_state=42)

        processed_data = []
        for _, row in df.iterrows():
            record = {}
            for col, value in row.items():
                if pd.isna(value):
                    record[col] = None
                elif isinstance(value, (int, float)):
                    record[col] = value
                else:
                    try:
                        record[col] = datetime.fromisoformat(str(value)).isoformat()
                    except ValueError:
                        record[col] = str(value)
            processed_data.append(record)

        result = {
            "data_source": csv_file_path,
            "processing_time": datetime.now().isoformat(),
            "records": processed_data
        }

        return json.dumps(result, indent=2)

    except FileNotFoundError:
        return f"Error: CSV file not found at '{csv_file_path}'"
    except Exception as e:
        return f"An error occurred during processing: {e}"

if __name__ == "__main__":
    if len(sys.argv) == 2:
        csv_file = sys.argv[1]
        json_output = csv_to_gemini_json(csv_file)
        print(json_output)
    else:
        print("Usage: python csv_to_json.py <path_to_your_csv_file>")
