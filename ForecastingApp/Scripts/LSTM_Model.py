import json
import time
import pandas as pd
import numpy as np
import sys
import pickle

from sklearn.metrics import mean_squared_error, mean_absolute_error, r2_score
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import LSTM, Dense
import matplotlib.pyplot as plt
import seaborn as sns
import os

pd.set_option('display.max_columns', None)


def inverse_transform_target(y_scaled, scaler, target_column):
    """
    Inverse transform scaled predictions or actuals for a single target column
    using a full-column scaler.

    Args:
        y_scaled (np.array): Scaled predictions or true values (1D or 2D)
        scaler (MinMaxScaler): Scaler fitted on all numerical columns
        target_column (str): Column name of the selected target

    Returns:
        np.array: Inverse-transformed values for the target column
    """
    if y_scaled.ndim == 1:
        y_scaled = y_scaled.reshape(-1, 1)

    # Get the index of the target column in the scaler
    target_index = list(scaler.feature_names_in_).index(target_column)

    # Create dummy array with all features (same shape as original input to scaler)
    dummy_input = np.zeros((len(y_scaled), scaler.n_features_in_))
    dummy_input[:, target_index] = y_scaled[:, 0]

    # Inverse transform and return just the target column
    y_unscaled = scaler.inverse_transform(dummy_input)[:, target_index]
    return y_unscaled

def build_lstm_model(neurons_layer1, neurons_layer2, time_steps, feature_count):
    model = Sequential([
        LSTM(neurons_layer1, activation='relu', return_sequences=True, input_shape=(time_steps, feature_count)),
        LSTM(neurons_layer2, activation='relu'),
        Dense(1)
    ])
    return model

def main():
    if len(sys.argv) != 9:
        print(sys.argv)
        print("Usage: LSTM_Model.py <epochs> <neurons_layer1> <neurons_layer2> <batch_size> <time_steps> <target_feature> <pickle_filename> <pickle_scaler>")
        input("Enter to exit")
        sys.exit(1)


    epochs = int(sys.argv[1])
    neurons_layer1 = int(sys.argv[2])
    neurons_layer2 = int(sys.argv[3])
    batch_size = int(sys.argv[4])
    timesteps = int(sys.argv[5])
    target = sys.argv[6]
    pickle_filename = sys.argv[7]
    pickle_scaler = sys.argv[8]

    print(f"Running LSTM model with {epochs} epochs, {neurons_layer1} neurons_lvl1, {neurons_layer2} neurons_lvl2, {batch_size} batch size, {timesteps} time steps.")

    # Load preprocessed DataFrame
    with open(pickle_filename, 'rb') as f:
        df = pickle.load(f)

    # Load the target scaler
    with open(pickle_scaler, "rb") as f:
        target_scaler = pickle.load(f)

    # Extract feature columns (all except the target)
    features = [col for col in df.columns if col != target]

    # Ensure the target exists
    if target not in df.columns:
        print(f"Error: Target feature '{target}' not found in dataset.")
        input("sdfsdfsdf")
        sys.exit(1)

    # target = 'Total Sales'

    X = df[features].values
    y = df[target].values

    def create_sequences(X, y, time_steps=5):
        X_seq, y_seq = [], []
        for i in range(len(X) - time_steps):
            X_seq.append(X[i:i+time_steps])
            y_seq.append(y[i+time_steps])
        return np.array(X_seq), np.array(y_seq)


    # time_steps = 10
    X_seq, y_seq = create_sequences(X, y, timesteps)

    split = int(0.75 * len(X_seq))
    X_train, X_test = X_seq[:split], X_seq[split:]
    y_train, y_test = y_seq[:split], y_seq[split:]

    X_train = X_train.astype(np.float32)
    X_test = X_test.astype(np.float32)
    y_train = y_train.astype(np.float32)
    y_test = y_test.astype(np.float32)

    model = build_lstm_model(neurons_layer1, neurons_layer2, timesteps, len(features))

    model.compile(optimizer='adam', loss='mse', metrics=['mae'])

    # Start training timer
    start_time = time.time()

    # Train the model and store history
    history = model.fit(X_train, y_train, validation_data=(X_test, y_test), epochs=epochs, batch_size=batch_size)

    # Stop timer
    training_duration = time.time() - start_time

    # Make predictions
    y_pred = model.predict(X_test).reshape(-1)

    # y_pred_scaled and y_test_scaled must be 2D for inverse_transform
    y_pred_unscaled = inverse_transform_target(y_pred, target_scaler, target)
    y_test_unscaled = inverse_transform_target(y_test, target_scaler, target)

    # Evaluate the model
    # loss, mae = model.evaluate(X_test, y_test)

    mse = mean_squared_error(y_test_unscaled, y_pred_unscaled)
    rmse = np.sqrt(mse)
    mae = mean_absolute_error(y_test_unscaled, y_pred_unscaled)

    # Avoid MAPE explosion from near-zero targets
    def safe_mape(y_true, y_pred):
        y_true, y_pred = np.array(y_true), np.array(y_pred)
        mask = y_true != 0
        return np.mean(np.abs((y_true[mask] - y_pred[mask]) / y_true[mask])) * 100

    mape = safe_mape(y_test_unscaled, y_pred_unscaled)
    accuracy = 100 - mape
    r2 = r2_score(y_test_unscaled, y_pred_unscaled)

    # Save evaluation metrics
    metrics = {
        "Loss (MSE)": float(round(mse, 4)),
        "Mean Absolute Error (MAE)": float(round(mae, 4)),
        "Root Mean Squared Error (RMSE)": float(round(rmse, 4)),
        "Mean Absolute Percentage Error (MAPE)": float(round(mape, 2)),
        "R2 Score": float(round(r2, 4)),
        "Accuracy (100 - MAPE)%": float(round(accuracy, 2)),
        "Training Duration (seconds)": float(round(training_duration, 2))
    }

    metrics_folder = os.path.join(os.path.dirname(__file__), 'Metrics')
    os.makedirs(metrics_folder, exist_ok=True)

    metrics_path = os.path.join(metrics_folder, "lstm_metrics.json")
    with open(metrics_path, 'w') as f:
        json.dump(metrics, f, indent=4)

    print("Metrics saved to: ", metrics_path)

    # Create Graphs folder if it doesn't exist
    graphs_folder = os.path.join(os.path.dirname(__file__), 'Graphs')
    os.makedirs(graphs_folder, exist_ok=True)


    # Training vs. Validation Loss
    plt.figure(figsize=(10, 5))
    plt.plot(history.history['loss'], label='Training Loss', color='blue')
    plt.plot(history.history['val_loss'], label='Validation Loss', color='red')
    plt.xlabel('Epochs')
    plt.ylabel('Loss (MSE)')
    plt.title('Training vs. Validation Loss')
    plt.legend()
    plt.savefig(os.path.join(graphs_folder, "training_vs_validation_loss.png"))  # Save as PNG
    plt.close()  # Close the figure to free memory

    # Actual vs Predicted Values
    plt.figure(figsize=(10, 5))
    plt.plot(y_test, label="Actual", color="blue", alpha=0.7)
    plt.plot(y_pred, label="Predicted", color="red", alpha=0.7)
    plt.xlabel('Time')
    plt.ylabel('Sales Quantity')
    plt.title('Actual vs Predicted Sales')
    plt.legend()
    plt.savefig(os.path.join(graphs_folder, "actual_vs_predicted.png"))
    plt.close()

    print("Graphs saved to: ", graphs_folder)

    # Residual Plot (Errors)
    errors = y_test - y_pred.reshape(-1)
    plt.figure(figsize=(10, 5))
    sns.histplot(errors, bins=50, kde=True, color="purple")
    plt.xlabel("Prediction Error")
    plt.ylabel("Frequency")
    plt.title("Residual Distribution")
    plt.savefig(os.path.join(graphs_folder, "residual_distribution.png"))
    plt.close()

    input("Press Enter to exit...")

if __name__ == "__main__":
    main()
    # input("sdfsdfsdf")