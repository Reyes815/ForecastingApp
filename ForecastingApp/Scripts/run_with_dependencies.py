import subprocess
import sys
import os

# Define paths
PROJECT_DIR = os.path.dirname(os.path.abspath(__file__))  # Get current script directory
VENV_DIR = os.path.join(PROJECT_DIR, "python_env")  # Virtual environment folder
PYTHON_EXECUTABLE = os.path.join(VENV_DIR, "Scripts", "python.exe") if os.name == "nt" else os.path.join(VENV_DIR, "bin", "python3")
# if not os.path.exists(PYTHON_EXECUTABLE):  # Linux/macOS fallback
#     PYTHON_EXECUTABLE = os.path.join(VENV_DIR, "bin", "python3")

# List of required packages
REQUIRED_PACKAGES = [
    "pandas",
    "tensorflow",
    "scikit-learn",
    "prophet",
    "xgboost",
    "joblib",
    "pythonnet"
]

import time

def create_virtual_env():
    """Create a virtual environment if it doesn't exist and wait for completion."""
    if not os.path.exists(VENV_DIR):
        print("Creating virtual environment...")
        subprocess.check_call([sys.executable, "-m", "venv", VENV_DIR])

        # Wait until the folder and Python executable exist
        wait_for_folder(VENV_DIR)
        wait_for_file(PYTHON_EXECUTABLE)

        print(f"Virtual environment created at: {VENV_DIR}")

def wait_for_folder(folder_path, timeout=30):
    """Wait until the specified folder exists, with a timeout."""
    start_time = time.time()
    while not os.path.exists(folder_path):
        if time.time() - start_time > timeout:
            raise TimeoutError(f"Timeout waiting for folder: {folder_path}")
        time.sleep(0.5)  # Check every 0.5 seconds

def wait_for_file(file_path, timeout=30):
    """Wait until the specified file exists, with a timeout."""
    start_time = time.time()
    while not os.path.exists(file_path):
        if time.time() - start_time > timeout:
            raise TimeoutError(f"Timeout waiting for file: {file_path}")
        time.sleep(0.5)  # Check every 0.5 seconds

def install_missing_packages():
    """Check and install missing packages inside the virtual environment."""
    for package in REQUIRED_PACKAGES:
        try:
            subprocess.run([PYTHON_EXECUTABLE, "-c", f"import {package}"], check=True, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        except subprocess.CalledProcessError:
            print(f"Installing {package}...")
            subprocess.check_call([PYTHON_EXECUTABLE, "-m", "pip", "install", package])

def run_main_script():
    """Run the main Python script using the virtual environment."""
    script_name = "main.py"  # Change this to your script file
    print(f"Running {script_name} with virtual environment...")
    subprocess.run([PYTHON_EXECUTABLE, script_name], check=True)

if __name__ == "__main__":
    create_virtual_env()
    install_missing_packages()
    # run_main_script()
    input("\nPress Enter to exit...")
