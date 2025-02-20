import pandas as pd
import numpy as np
from prophet import Prophet
from sklearn.metrics import mean_absolute_error, mean_squared_error

class ForecastModel:
    def __init__(self, csv_file):
        self.df = pd.read_csv(csv_file, parse_dates=['Date'], encoding='latin1', index_col='Date')
        self.df = self.df.sort_values('Date')
        self.model = None

    def preprocess_data(self):
        # Ensure all dates are formatted correctly
        self.df.index = pd.to_datetime(self.df.index, format='%d-%m-%Y', errors='coerce')
        self.df = self.df[self.df.index.notna()]  # Remove invalid dates

        # Prophet requires 'ds' and 'y' columns
        self.df['ds'] = self.df.index
        if 'Profit' in self.df.columns:
            self.df['y'] = pd.to_numeric(self.df['Profit'], errors='coerce')
            self.df = self.df.dropna(subset=['y'])
        else:
            raise ValueError("The dataset does not contain the 'Profit' column.")

        # Normalize the target variable
        self.df['y'] = self.df['y'].clip(lower=0)
        self.df = self.df.dropna(subset=['y'])
        self.df['y'] = (self.df['y'] - self.df['y'].mean()) / self.df['y'].std()

    def train_model(self):
        train_size = int(0.8 * len(self.df))
        train_data = self.df.iloc[:train_size][['ds', 'y']]

        self.model = Prophet()
        self.model.add_seasonality(name='monthly', period=30.5, fourier_order=5)
        self.model.add_seasonality(name='weekly', period=7, fourier_order=3)
        self.model.fit(train_data)

    def forecast(self, periods=30):
        future = self.model.make_future_dataframe(periods=periods, freq='D')
        forecast = self.model.predict(future)
        return forecast[['ds', 'yhat', 'yhat_lower', 'yhat_upper']]

    def evaluate_model(self):
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
    model = ForecastModel("superstore.csv")
    model.preprocess_data()
    model.train_model()
    forecast = model.forecast(60)  # Forecast 60 days
    print(forecast.head())
    print(model.evaluate_model())

