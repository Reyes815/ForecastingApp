import sys
import pandas as pd
import numpy as np
from prophet import Prophet
from sklearn.metrics import mean_absolute_error, mean_squared_error

class ProphetForecast:
    def __init__(self, csv_file):
        self.csv_file = csv_file
        self.df = None
        self.model = None

    def load_data(self):
        """Load the dataset and preprocess the date column."""
        try:
            self.df = pd.read_csv(self.csv_file, parse_dates=['Date'], encoding='latin1', index_col='Date')
            self.df = self.df.sort_values('Date')

            # Convert dates properly
            self.df.index = pd.to_datetime(self.df.index, format='%d-%m-%Y', errors='coerce')
            self.df = self.df[self.df.index.notna()]  # Remove invalid dates

            if 'Profit' not in self.df.columns:
                raise ValueError("The dataset must contain a 'Profit' column.")

            # Prepare Prophet-compatible format
            self.df['ds'] = self.df.index
            self.df['y'] = pd.to_numeric(self.df['Profit'], errors='coerce')
            self.df = self.df.dropna(subset=['y'])

            # Normalize 'y' values
            self.df['y'] = (self.df['y'] - self.df['y'].mean()) / self.df['y'].std()

        except Exception as e:
            print(f"Error loading data: {e}")
            sys.exit(1)

    def train_model(self):
        """Train the Prophet model on the dataset."""
        train_size = int(0.8 * len(self.df))
        train_data = self.df.iloc[:train_size][['ds', 'y']]

        self.model = Prophet()
        self.model.add_seasonality(name='monthly', period=30.5, fourier_order=5)
        self.model.add_seasonality(name='weekly', period=7, fourier_order=3)
        self.model.fit(train_data)

    def forecast(self, periods=30):
        """Generate a forecast for the specified number of periods."""
        future = self.model.make_future_dataframe(periods=periods, freq='D')
        forecast = self.model.predict(future)
        return forecast[['ds', 'yhat', 'yhat_lower', 'yhat_upper']]

    def evaluate_model(self):
        """Evaluate the model's performance using MAE and RMSE."""
        train_size = int(0.8 * len(self.df))
        test_data = self.df.iloc[train_size:][['ds', 'y']]

        if self.model is None:
            raise ValueError("Model has not been trained. Call train_model() first.")

        forecast = self.forecast(len(test_data))
        forecast = forecast.iloc[-len(test_data):]

        mae = mean_absolute_error(test_data['y'], forecast['yhat'])
        rmse = np.sqrt(mean_squared_error(test_data['y'], forecast['yhat']))

        return {"MAE": mae, "RMSE": rmse}

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python prophet_forecast.py <csv_file_path>")
        sys.exit(1)

    csv_file = sys.argv[1]

    # Run Prophet forecast
    model = ProphetForecast(csv_file)
    model.load_data()
    model.train_model()
    forecast = model.forecast(60)  # Forecast for 60 days
    evaluation = model.evaluate_model()

    print(forecast.head())
    print("Model Evaluation:", evaluation)
