﻿using System;
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

namespace ForecastingApp
{
    public partial class MainForm : Form
    {
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
            dataGridView1.Columns.Add("category", "Category");

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ProcessCSVFile(string filePath)
        {
            try
            {
                // Start the Python process
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = @"C:\Python312\python.exe";  // Adjust Python path if needed
                string scriptPath = Path.Combine(Application.StartupPath, "..", "..", "..", "Scripts", "process_dataset_csv.py");
                psi.Arguments = $"\"{scriptPath}\" \"{filePath}\"";
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

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

                    // Populate DataGridView
                    dataGridView1.Rows.Clear();
                    foreach (var column in result["columns"])
                    {
                        dataGridView1.Rows.Add(column["name"], column["type"], column["unique_values"], column["category"]);
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
                    // Get the selected file path
                    string filePath = openFileDialog.FileName;

                    // You can now process the CSV file (e.g., read it into your application)
                    ProcessCSVFile(filePath);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
