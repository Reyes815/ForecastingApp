import pandas as pd
import json
import sys
from datetime import datetime

def csv_to_gemini_json(csv_file_path):
    """
    Reads a CSV file, processes it, and formats it into a JSON structure
    suitable for the Gemini API.

    Args:
        csv_file_path (str): The path to the input CSV file.

    Returns:
        str: A JSON string representing the processed data, or an error message.
    """
    try:
        df = pd.read_csv(csv_file_path)

        # Basic data cleaning and type handling (customize as needed)
        processed_data = []
        for index, row in df.iterrows():
            record = {}
            for col, value in row.items():
                # Try to convert to appropriate JSON-serializable types
                if pd.isna(value):
                    record[col] = None
                elif isinstance(value, (int, float)):
                    record[col] = value
                else:
                    try:
                        # Attempt to parse dates if they look like dates
                        record[col] = datetime.fromisoformat(str(value)).isoformat()
                    except ValueError:
                        record[col] = str(value) # Keep as string if not a recognized date

            processed_data.append(record)

        # Format the data into a JSON structure Gemini API might expect
        # (This structure can be adapted based on the specific Gemini API endpoint)
        gemini_input = {
            "data_source": csv_file_path,
            "processing_time": datetime.now().isoformat(),
            "records": processed_data
        }

        return json.dumps(gemini_input, indent=2)

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