import pandas as pd
import numpy as np
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import LSTM, Dense
from sklearn.preprocessing import MinMaxScaler

# Load the CSV data
df = pd.read_csv('C:\\Users\\gianl\\Downloads\\Nike Dataset.csv')  # Replace 'your_data.csv' with the actual file path

# One-hot encode the 'Beer_Style' column
df = pd.get_dummies(df, columns=['Product'], prefix='Product')

df = pd.get_dummies(df, columns=['Region'], prefix='Region')

df = pd.get_dummies(df, columns=['Retailer'], prefix='Retailer')

df = pd.get_dummies(df, columns=['Sales Method'], prefix='Sales_Method')

df = pd.get_dummies(df, columns=['State'], prefix='State')

print(df.head())


# Set pandas to display more columns (you can set it to None for unlimited display)
pd.set_option('display.max_columns', None)

columns_to_scale = [
    'Price per Unit', 'Total Sales', 'Units Sold'  ]


df_to_scale = df[columns_to_scale]

scaler = MinMaxScaler()

print('Before scaling:\n', df.head(), '\n\n')

# Fit and transform the data
scaled_data = scaler.fit_transform(df_to_scale)

# Convert scaled data back to a DataFrame
scaled_df = pd.DataFrame(scaled_data, columns=columns_to_scale)

# Replace original columns with scaled data
df[columns_to_scale] = scaled_df

print('After scaling:\n', df.head())

# Convert 'Invoice Date' to datetime
df['Invoice Date'] = pd.to_datetime(df['Invoice Date'], format='%d-%m-%Y')

# Sort by 'Invoice Date'
df.sort_values('Invoice Date', inplace=True)

# Convert 'Invoice Date' to numeric (seconds since the epoch)
df['Invoice Date_Numeric'] = df['Invoice Date'].astype('int64') / 1e9

print(df.head())


features = ['Invoice Date_Numeric', 'Price per Unit',
            "Product_Men's Apparel", "Product_Men's Athletic Footwear",
            "Product_Men's Street Footwear", "Product_Women's Apparel",
            "Product_Women's Athletic Footwear", "Product_Women's Street Footwear",
            'Region_Midwest', 'Region_Northeast', 'Region_South', 'Region_Southeast',
            'Region_West', 'Retailer_Amazon', 'Retailer_Foot Locker',
            "Retailer_Kohl's", 'Retailer_Sports Direct', 'Retailer_Walmart',
            'Retailer_West Gear', 'Sales_Method_In-store', 'Sales_Method_Online',
            'Sales_Method_Outlet', 'State_Alabama', 'State_Arizona', 'State_Arkansas',
            'State_California', 'State_Colorado', 'State_Connecticut', 'State_Delaware',
            'State_Florida', 'State_Georgia', 'State_Idaho', 'State_Illinois', 'State_Indiana',
            'State_Iowa', 'State_Kansas', 'State_Kentucky', 'State_Louisiana', 'State_Maine',
            'State_Maryland', 'State_Massachusetts', 'State_Michigan', 'State_Minnesota',
            'State_Mississippi', 'State_Missouri', 'State_Montana', 'State_Nebraska', 'State_Nevada',
            'State_New Hampshire', 'State_New Jersey', 'State_New Mexico',
            'State_New York', 'State_North Carolina', 'State_North Dakota',
            'State_Ohio', 'State_Oklahoma', 'State_Oregon', 'State_Pennsylvania',
            'State_Rhode Island', 'State_South Carolina', 'State_South Dakota',
            'State_Tennessee', 'State_Texas', 'State_Utah', 'State_Vermont',
            'State_Virginia', 'State_Washington', 'State_West Virginia',
            'State_Wisconsin', 'State_Wyoming']

target = 'Total Sales'

X = df[features].values
y = df[target].values

def create_sequences(X, y, time_steps=5):
    X_seq, y_seq = [], []
    for i in range(len(X) - time_steps):
        X_seq.append(X[i:i+time_steps])
        y_seq.append(y[i+time_steps])
    return np.array(X_seq), np.array(y_seq)


time_steps = 10
X_seq, y_seq = create_sequences(X, y, time_steps)

split = int(0.8 * len(X_seq))
X_train, X_test = X_seq[:split], X_seq[split:]
y_train, y_test = y_seq[:split], y_seq[split:]

X_train = X_train.astype(np.float32)
X_test = X_test.astype(np.float32)
y_train = y_train.astype(np.float32)
y_test = y_test.astype(np.float32)


model = Sequential([
    LSTM(64, activation='relu', return_sequences=True, input_shape=(time_steps, len(features))),
    LSTM(32, activation='relu'),
    Dense(1)
])

model.compile(optimizer='adam', loss='mse', metrics=['mae'])

model.fit(X_train, y_train, validation_data=(X_test, y_test), epochs=30, batch_size=64)

loss, mae = model.evaluate(X_test, y_test)
print(f"Test Loss: {loss}, Test MAE: {mae}")

y_pred = model.predict(X_test)