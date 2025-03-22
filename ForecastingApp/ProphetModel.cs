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

namespace ForecastingApp
{
    public partial class ProphetModel : Form
    {
        private MainForm mainForm;
        private string cvs_filepath;
        private List<string> features;

        public ProphetModel(MainForm mainForm, string path, List<string> processed_features)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.cvs_filepath = path;
            features = processed_features;
        }

        private void ProphetModel_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show(); // Show the main form when this form is closed
        }

        private void RunProphetPrediction()
        {
            try
            {
                // Get project root directory
                string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string pythonExecutable = Path.Combine(projectRoot, "Scripts", "python_env", "Scripts", "python.exe");
                string picklefilepath = Path.Combine(projectRoot, "Scripts", "Saved preprocessed csv", $"{cvs_filepath}");
                string scriptPath = Path.Combine(projectRoot, "Scripts", "Prophet_forecast.py");

                // Get user inputs from the form
                string period = period_txtbox.Text;
                string train = train_txtbox.Text;
                string test = test_txtbox.Text;
                string split = split_txtbox.Text;
                bool weekly = weekly_rbtn.Checked;
                bool monthly = monthly_rbtn.Checked;
                bool holiday = !Disable_rbtn.Checked;
                bool standardization = !Disable_rbtn2.Checked;
                string target = target_dropdown.Text;

                // Ensure 'growth' and 'seasonality_mode' have valid values
                string growth = linear_rbtn.Checked ? "linear" : logistic_rbtn.Checked ? "logistic" : "linear";  // Default to "linear"
                string seasonality_mode = additive_rbtn.Checked ? "additive" : multiplicative_rbtn.Checked ? "multiplicative" : "additive"; // Default to "additive"

                // Build the argument string correctly
                string arguments = $"\"{scriptPath}\" \"{cvs_filepath}\" {period} {train} {test} {split} " +
                                   $"{weekly.ToString().ToLower()} {monthly.ToString().ToLower()} {holiday.ToString().ToLower()} " +
                                   $"{standardization.ToString().ToLower()} \"{target}\" \"{growth}\" \"{seasonality_mode}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExecutable,
                    Arguments = arguments,
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                Process process = new Process { StartInfo = psi };
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Python Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunProphetPrediction();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }


        private void ProphetModel_load(object sender, EventArgs e)
        {
            // Set default text values
            period_txtbox.Text = "30";  // Default forecast period (days)
            train_txtbox.Text = "0.8";  // 80% training data
            test_txtbox.Text = "0.2";   // 20% test data
            split_txtbox.Text = "0.7";  // 70% data split ratio

            // Set default radio button selections
            Enable_rbtn.Checked = true;   // Enable holidays
            Disable_rbtn.Checked = false;

            Enable_rbtn2.Checked = true;  // Enable standardization
            Disable_rbtn2.Checked = false;

            // Default seasonality settings
            weekly_rbtn.Checked = true;   // Default to weekly seasonality
            monthly_rbtn.Checked = false;

            additive_rbtn.Checked = true;  // Default to additive mode
            multiplicative_rbtn.Checked = false;

            linear_rbtn.Checked = true;
            logistic_rbtn.Checked = false;

            // Ensure 'features' is not null before accessing it
            if (features != null && features.Count > 0)
            {
                target_dropdown.Items.Clear();  // Clear existing items to prevent duplicates
                target_dropdown.Items.AddRange(features.ToArray());

                // Set the first feature as the default selection, only if items exist
                if (target_dropdown.Items.Count > 0)
                {
                    target_dropdown.SelectedIndex = 0;
                }
            }
        }
    }
}
