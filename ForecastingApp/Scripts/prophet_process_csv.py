import sys
import pandas as pd
import json
import os

def try_parse_dates(date_series):
    """
    Attempt to parse a series of dates with various formats.
    """
    try:
        return pd.to_datetime(date_series, errors='coerce', format='%d-%m-%Y')  # You can adjust the format here
    except Exception as e:
        print(f"Error parsing dates: {e}")
        return pd.to_datetime(date_series, errors='coerce')  # Fallback if the first format fails

def convert_datetime_columns(df):
    """
    Detects datetime columns (except 'ds') and converts them to Unix timestamps.
    Drops the original datetime columns after conversion.
    """
    datetime_columns = [
        col for col in df.select_dtypes(include=['datetime64[ns]', 'datetime'])
        if col != 'ds'  # Don't include 'ds' column, as Prophet expects it
    ]

    for col in datetime_columns:
        df[f"{col}_timestamp"] = df[col].astype('int64') // 10**9  # Convert to Unix timestamp in seconds
        df.drop(columns=[col], inplace=True)  # Remove original datetime columns

    return df

def preprocess_for_prophet(file_path):
    try:
        # Load CSV file
        df = pd.read_csv(file_path)

        # Ensure the 'Date' column exists
        if 'Date' not in df.columns:
            raise ValueError("The dataset does not contain the 'Date' column.")
        
        # Parse and rename 'Date' column to 'ds' (Prophet's required column)
        df['ds'] = try_parse_dates(df['Date'])
        df = df.dropna(subset=['ds'])  # Drop rows where 'ds' is NaT (invalid dates)

        # Convert any other datetime columns to Unix timestamps
        df = convert_datetime_columns(df)

        # Save preprocessed file for Prophet
        script_dir = os.path.dirname(os.path.abspath(__file__))
        save_dir = os.path.join(script_dir, "preprocessed prophet csv")
        os.makedirs(save_dir, exist_ok=True)

        # Generate output file path (output as CSV for simplicity)
        output_csv_path = os.path.join(save_dir, f"{os.path.splitext(os.path.basename(file_path))[0]}_processed.csv")
        df.to_csv(output_csv_path, index=False)

        # Return success message
        result = {
            "status": "success",
            "message": f"Data preprocessed for Prophet and saved as {output_csv_path}",
            "rows": len(df),
            "saved_file": output_csv_path  # Return the full path
        }
        print(json.dumps(result))

    except Exception as e:
        print(json.dumps({"error": str(e)}))

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print(json.dumps({"error": "Usage: Prophet_preprocessing.py <csv_file>"}))
    else:
        preprocess_for_prophet(sys.argv[1])
