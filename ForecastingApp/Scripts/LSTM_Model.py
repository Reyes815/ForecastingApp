import json
import time
import pandas as pd
import numpy as np
import sys
import pickle

from sklearn.metrics import r2_score
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import LSTM, Dense
import matplotlib.pyplot as plt
import seaborn as sns
import os

pd.set_option('display.max_columns', None)

def build_lstm_model(neurons_layer1, neurons_layer2, time_steps, feature_count):
    model = Sequential([
        LSTM(neurons_layer1, activation='relu', return_sequences=True, input_shape=(time_steps, feature_count)),
        LSTM(neurons_layer2, activation='relu'),
        Dense(1)
    ])
    return model

def main():
    if len(sys.argv) != 8:
        print(sys.argv)
        print("Usage: LSTM_Model.py <epochs> <neurons_layer1> <neurons_layer2> <batch_size> <time_steps> <target_feature> <pickle_filename>")
        input("Enter to exit")
        sys.exit(1)


    epochs = int(sys.argv[1])
    neurons_layer1 = int(sys.argv[2])
    neurons_layer2 = int(sys.argv[3])
    batch_size = int(sys.argv[4])
    timesteps = int(sys.argv[5])
    target = sys.argv[6]
    pickle_filename = sys.argv[7]

    print(f"Running LSTM model with {epochs} epochs, {neurons_layer1} neurons_lvl1, {neurons_layer2} neurons_lvl2, {batch_size} batch size, {timesteps} time steps.")

    # Load preprocessed DataFrame
    with open(pickle_filename, 'rb') as f:
        df = pickle.load(f)

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

    # Evaluate the model
    loss, mae = model.evaluate(X_test, y_test)
    # print(f"Test Loss: {loss}, Test MAE: {mae}")

    # Make predictions
    y_pred = model.predict(X_test).reshape(-1)

    # Calculate R^2 Score
    r2 = r2_score(y_test, y_pred)

    # Save evaluation metrics
    metrics = {
        "Loss (MSE)": round(loss, 4),
        "Mean Absolute Error (MAE)": round(mae, 4),
        "R² Score": round(r2, 4),
        "Training Duration (seconds)": round(training_duration, 2)
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