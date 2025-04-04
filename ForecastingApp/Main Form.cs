using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Python.Runtime;
using Newtonsoft.Json;
using System.Reflection.Emit;

namespace ForecastingApp
{
    public partial class MainForm : Form
    {

        private string selected_model = "";
        private string filePath = "";
        private string rawCSVFile = "";
        private string selectedCSVFile = "";
        private List<string> features = new List<string>();

        public MainForm()
        {
            InitializeComponent();

            InitializeDataGridView();

            //Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", @"C:\Users\Rowen\AppData\Local\Programs\Python\Python310\python310.dll");

            //try
            //{
            //    PythonEngine.Initialize();
            //    MessageBox.Show("Python.NET Initialized Successfully!");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Python.NET Initialization Failed: " + ex.Message);
            //}
        }

        private void InitializeDataGridView()
        {
            // Clear existing columns
            dataGridView1.Columns.Clear();

            // Add three columns: Features, Column 2, Column 3
            dataGridView1.Columns.Add("Features", "Features");
            dataGridView1.Columns.Add("type", "Types");
            dataGridView1.Columns.Add("unique_vals", "Number of Unique Values");
            dataGridView1.Columns.Add("null_vals", "Number of Null Values");
            dataGridView1.Columns.Add("category", "Category");

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Cast sender to RadioButton
            RadioButton rb = sender as RadioButton;

            // Check if the RadioButton is checked
            if (rb != null && rb.Checked)
            {
                selected_model = rb.Text; // Store the selected value
            }
        }

        private void ProcessCSVFile(string filePath)
        {
            try
            {
                string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string pythonExecutable = Path.Combine(projectRoot, "Scripts", "python_env", "Scripts", "python.exe");
                string scriptPath = Path.Combine(Application.StartupPath, "..", "..", "..", "Scripts", "process_dataset_csv.py");


                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExecutable,
                    Arguments = $"\"{scriptPath}\" \"{filePath}\"", // Your Python script
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = psi;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // Parse the JSON output from the Python script
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(output);

                    if (result["error"] != null)
                    {
                        MessageBox.Show("Error: " + result["error"], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Display results
                    label2.Text = result["filename"];
                    label3.Text = $"Instances: {result["instances"]}";
                    label4.Text = $"Features Before Encoding: {result["features_before_encoding"]}";
                    label8.Text = $"Features After Encoding: {result["features_after_encoding"]}";

                    this.selectedCSVFile = result["saved_pickle"];

                    features.Clear();

                    // Populate DataGridView
                    dataGridView1.Rows.Clear();
                    foreach (var column in result["columns"])
                    {
                        string featureName = column["name"].ToString();
                        features.Add(featureName);
                        dataGridView1.Rows.Add(column["name"], column["type"], column["unique_values"], column["null_values"], column["category"]);
                    }
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
                    // You can now process the CSV file (e.g., read it into your application)
                    selectedCSVFile = openFileDialog.FileName;
                    rawCSVFile = selectedCSVFile;
                    ProcessCSVFile(selectedCSVFile);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (selected_model)
            {
                case "LSTM":
                    this.Hide();
                    LSTM_MODEL newLSTM_FORM = new LSTM_MODEL(this, this.selectedCSVFile, this.features);
                    newLSTM_FORM.Show();
                    break;
                case "XGBoost":
                    this.Hide();
                    //LSTM_MODEL newLSTM_FORM = new LSTM_MODEL();
                    //newLSTM_FORM.Show();
                    XGBoost new_xGBoost = new XGBoost(this, rawCSVFile);
                    new_xGBoost.Show();
                    break;
                case "Prophet":
                    this.Hide();
                    ProphetModel newProphetModel = new ProphetModel(this, rawCSVFile, this.features);
                    newProphetModel.Show();
                    break;
                default:
                    MessageBox.Show("Please select a model first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }

        }
    }
}
