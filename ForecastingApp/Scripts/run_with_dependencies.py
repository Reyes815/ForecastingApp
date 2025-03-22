import subprocess
import sys
import os
import time

# Define paths
PROJECT_DIR = os.path.dirname(os.path.abspath(__file__))  # Get current script directory
VENV_DIR = os.path.join(PROJECT_DIR, "python_env")  # Virtual environment folder
PYTHON_EXECUTABLE = os.path.join(VENV_DIR, "Scripts", "python.exe") if os.name == "nt" else os.path.join(VENV_DIR, "bin", "python3")

# Fallback for macOS/Linux if the Python executable isn't found
if not os.path.exists(PYTHON_EXECUTABLE):
    PYTHON_EXECUTABLE = os.path.join(VENV_DIR, "bin", "python3")

# List of required packages for Prophet-based forecasting
REQUIRED_PACKAGES = [
    "pandas",
    "scikit-learn",
    "prophet",
    "xgboost",
    "joblib",
    "matplotlib",
    "seaborn",
    "pythonnet"
]

def create_virtual_env():
    """Create a virtual environment if it doesn't exist."""
    if not os.path.exists(VENV_DIR):
        print("Creating virtual environment...")
        subprocess.check_call([sys.executable, "-m", "venv", VENV_DIR])
        wait_for_folder(VENV_DIR)
        wait_for_file(PYTHON_EXECUTABLE)
        print(f"Virtual environment created at: {VENV_DIR}")

def wait_for_folder(folder_path, timeout=30):
    """Wait until the specified folder exists, with a timeout."""
    start_time = time.time()
    while not os.path.exists(folder_path):
        if time.time() - start_time > timeout:
            raise TimeoutError(f"Timeout waiting for folder: {folder_path}")
        time.sleep(0.5)

def wait_for_file(file_path, timeout=30):
    """Wait until the specified file exists, with a timeout."""
    start_time = time.time()
    while not os.path.exists(file_path):
        if time.time() - start_time > timeout:
            raise TimeoutError(f"Timeout waiting for file: {file_path}")
        time.sleep(0.5)

def install_missing_packages():
    """Check and install missing packages inside the virtual environment."""
    for package in REQUIRED_PACKAGES:
        try:
            subprocess.run([PYTHON_EXECUTABLE, "-c", f"import {package}"], check=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        except subprocess.CalledProcessError:
            print(f"Installing {package}...")
            subprocess.check_call([PYTHON_EXECUTABLE, "-m", "pip", "install", package])

def run_prophet_forecast(csv_filepath, period, train, test, split, weekly, monthly, holiday, standardization, target):
    """Run the Prophet forecasting script with given arguments."""
    script_name = os.path.join(PROJECT_DIR, "Prophet_forecast.py")
    
    arguments = [
        PYTHON_EXECUTABLE, script_name, csv_filepath, str(period), str(train),
        str(test), str(split), str(weekly), str(monthly), str(holiday),
        str(standardization), target
    ]
    
    print(f"Running Prophet Forecast with: {arguments}")
    subprocess.run(arguments, check=True)

if __name__ == "__main__":
    create_virtual_env()
    install_missing_packages()
    
    # Example usage (you can replace with actual arguments from UI)
    test_csv_filepath = "data/sales_data.csv"  # Replace with actual file path
    run_prophet_forecast(test_csv_filepath, 30, 0.8, 0.2, 0.7, True, False, True, True, "Profit")

    input("\nPress Enter to exit...")
