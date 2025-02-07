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

namespace ForecastingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Uploadbtn_Click(object sender, EventArgs e)
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

        private void ProcessCSVFile(string filePath)
        {
            try
            {
                // Example: Read the CSV file into a DataTable or process it as needed
                using (var reader = new StreamReader(filePath))
                {
                    // Read the first line
                    string headerLine = reader.ReadLine();

                    if (headerLine != null)
                    {
                        // Update label2 with the header
                        label2.Text = "Dataset Header: " + headerLine;
                        Uploadbtn.Top = label2.Bottom + 10;
                    }
                    else
                    {
                        label2.Text = "The file is empty.";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing CSV file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            
        }
    }
}
