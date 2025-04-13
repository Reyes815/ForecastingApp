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
        private Dictionary<string, string> dictionary;

        public ProphetModel(MainForm mainForm, string path, List<string> processed_features, Dictionary<string, string> geminiHyperparams)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            cvs_filepath = path;
            features = processed_features;
            dictionary = geminiHyperparams;
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
                string scriptPath = Path.Combine(projectRoot, "Scripts", "Prophet_forecast.py");

                // Get user inputs from the form
                string period = period_txtbox.Text;
                string train = train_txtbox.Text;
                bool weekly = weekly_rbtn.Checked;
                bool monthly = monthly_rbtn.Checked;
                bool holiday = !Disable_rbtn.Checked;
                bool standardization = !Disable_rbtn2.Checked;
                string growth = linear_rbtn.Checked ? "flat" : logistic_rbtn.Checked ? "flat" : "linear";  // Default to "linear"
                string seasonality_mode = additive_rbtn.Checked ? "additive" : multiplicative_rbtn.Checked ? "multiplicative" : "additive"; // Default to "additive"
                string changepoint_pscale = cp_pscale_txtbox.Text;
                string seasonality_pscale = s_pscale_txtbox.Text;
                string target = target_dropdown.Text;

                // Build the argument string in correct order
                string arguments = $"\"{scriptPath}\" \"{cvs_filepath}\" {period} {train} " +
                                   $"{weekly.ToString().ToLower()} {monthly.ToString().ToLower()} {holiday.ToString().ToLower()} " +
                                   $"{standardization.ToString().ToLower()} \"{growth}\" \"{seasonality_mode}\" " +
                                   $"{changepoint_pscale} {seasonality_pscale} \"{target}\"";

                // Debugging: Show the arguments
                Console.WriteLine($"Running script with arguments: {arguments}");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExecutable,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                Process process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine(output);
                Console.WriteLine(errors);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Python Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProphetModel_load(object sender, EventArgs e)
        {
            // Show the hyperparameters in label_Hyperparameters
            StringBuilder hyperparamText = new StringBuilder("Recommended Hyperparameters:");
            foreach (var kvp in dictionary)
            {
                hyperparamText.AppendLine($"{kvp.Key}: {kvp.Value}");
            }
            label_Hyperparameters.Text = hyperparamText.ToString();

            if (dictionary != null)
            {
                period_txtbox.Text = dictionary["forecast_period"];
                train_txtbox.Text = dictionary["train_ratio"];
                weekly_rbtn.Checked = dictionary["weekly"] == "true";
                monthly_rbtn.Checked = dictionary["monthly"] == "true";
                Enable_rbtn.Checked = dictionary["holidays_enabled"] == "true";
                Disable_rbtn.Checked = !Enable_rbtn.Checked;
                Enable_rbtn2.Checked = dictionary["standardization"] == "true";
                Disable_rbtn2.Checked = !Enable_rbtn2.Checked;
                additive_rbtn.Checked = dictionary["seasonality_mode"] == "additive";
                multiplicative_rbtn.Checked = !additive_rbtn.Checked;
                linear_rbtn.Checked = dictionary["growth"] == "linear";
                logistic_rbtn.Checked = !linear_rbtn.Checked;
                cp_pscale_txtbox.Text = dictionary["changepoint_prior_scale"];
                s_pscale_txtbox.Text = dictionary["seasonality_prior_scale"];

                target_dropdown.Items.Clear();
                target_dropdown.Items.AddRange(features.ToArray());
                if (target_dropdown.Items.Count > 0)
                {
                    target_dropdown.SelectedItem = dictionary["target_column"];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunProphetPrediction();
        }
    }
}