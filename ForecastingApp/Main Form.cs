using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Python.Runtime;

namespace ForecastingApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", @"C:\Users\Rowen\AppData\Local\Programs\Python\Python310\python310.dll");

            try
            {
                PythonEngine.Initialize();
                MessageBox.Show("Python.NET Initialized Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Python.NET Initialization Failed: " + ex.Message);
            }
        }

        private void ProcessCSVFile(string filePath)
        {
            try
            {
                using (Py.GIL())  // Acquire Python Global Interpreter Lock
                {
                    dynamic forecasting = Py.Import("forecasting"); // Import forecasting.py
                    dynamic model = forecasting.ForecastModel(filePath);

                    model.preprocess_data();
                    model.train_model();
                    dynamic forecast = model.forecast(30); // Predict 30 days

                    // Display the forecasted results
                    label1.Text = "Forecast Results: " + forecast.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running Python script: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UploadMenuBtn(object sender, EventArgs e)
        {
            // Create an instance of OpenFileDialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the filter to only allow CSV files
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1; // Default to CSV files
                openFileDialog.RestoreDirectory = true; // Restore the directory after closing the dialog

                // Show the file dialog and check if the user clicked OK
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path
                    string filePath = openFileDialog.FileName;

                    // You can now process the CSV file (e.g., read it into your application)
                    ProcessCSVFile(filePath);
                }
            }
        }
    }
}
