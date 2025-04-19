using Newtonsoft.Json.Linq;
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
        private string suggestions;

        public ProphetModel(MainForm mainForm, string path, List<string> processed_features, string geminiHyperparams)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            cvs_filepath = path;
            features = processed_features;
            suggestions = geminiHyperparams;
        }

        private void ProphetModel_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Show();
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
                bool daily = daily_rbtn.Checked;
                bool weekly = weekly_rbtn.Checked;
                bool monthly = monthly_rbtn.Checked;
                bool yearly = yearly_rbtn.Checked;
                bool holiday = !Disable_rbtn.Checked;
                bool standardization = !Disable_rbtn2.Checked;
                string growth = linear_rbtn.Checked ? "flat" : logistic_rbtn.Checked ? "flat" : "linear";  // Default to "linear"
                string seasonality_mode = additive_rbtn.Checked ? "additive" : multiplicative_rbtn.Checked ? "multiplicative" : "additive"; // Default to "additive"
                string changepoint_pscale = cp_pscale_txtbox.Text;
                string seasonality_pscale = s_pscale_txtbox.Text;
                string target = target_dropdown.Text;

                // Build the argument string in correct order
                string arguments = $"\"{scriptPath}\" \"{cvs_filepath}\" {period} {train} " +
                                   $"{daily.ToString().ToLower()} {weekly.ToString().ToLower()} {monthly.ToString().ToLower()} {yearly.ToString().ToLower()} {holiday.ToString().ToLower()} " +
                                   $"{standardization.ToString().ToLower()} \"{growth}\" \"{seasonality_mode}\" " +
                                   $"{changepoint_pscale} {seasonality_pscale} \"{target}\"";

                // Debugging: Show the arguments
                Console.WriteLine($"Running script with arguments: {arguments}");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExecutable,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = false
                    //RedirectStandardOutput = true,
                    //RedirectStandardError = true,
                    //UseShellExecute = false,
                    //CreateNoWindow = false
                };

                //Process process = Process.Start(psi);
                
                //process.WaitForExit();

                Process process = new Process { StartInfo = psi, EnableRaisingEvents = true };

                //stopwatch.Start();
                process.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Python Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ProphetModel_load(object sender, EventArgs e)
        {
            string cleanedJson = suggestions.Trim();

            // If wrapped in ```json ... ```
            if (cleanedJson.StartsWith("```"))
            {
                int firstBrace = cleanedJson.IndexOf('{');
                int lastBrace = cleanedJson.LastIndexOf('}');
                if (firstBrace != -1 && lastBrace != -1)
                {
                    cleanedJson = cleanedJson.Substring(firstBrace, (lastBrace - firstBrace + 1));
                }
            }

            try
            {
                var json = JObject.Parse(cleanedJson);
                StringBuilder hyperparamText = new StringBuilder("Recommended Hyperparameters:");

                foreach (var kvp in json)
                {
                    hyperparamText.AppendLine($"{kvp.Key}: {kvp.Value}");
                }

                label_Hyperparameters.Text = hyperparamText.ToString();
            }
            catch (Exception ex)
            {
                label_Hyperparameters.Text = $"Error parsing hyperparameters: {ex.Message}";
            }

            // Forecasting horizon (e.g., 90 days ahead)
            period_txtbox.Text = "90";

            // Training split (e.g., 80% train, 20% test)
            train_txtbox.Text = "0.8";

            // Seasonality toggles (Prophet has built-in yearly and weekly; monthly is custom)
            daily_rbtn.Checked = true;
            weekly_rbtn.Checked = true;
            monthly_rbtn.Checked = true;
            yearly_rbtn.Checked = true;

            // Holidays enabled (optional, good default to start with disabled)
            Enable_rbtn.Checked = false;
            Disable_rbtn.Checked = !Enable_rbtn.Checked;

            // Standardization (optional for Prophet, but can improve performance on large range data)
            Enable_rbtn2.Checked = false;
            Disable_rbtn2.Checked = !Enable_rbtn2.Checked;

            // Seasonality Mode
            additive_rbtn.Checked = true;
            multiplicative_rbtn.Checked = !additive_rbtn.Checked;

            // Growth Model (linear is default unless your data saturates or caps)
            linear_rbtn.Checked = true;
            logistic_rbtn.Checked = !linear_rbtn.Checked;

            // Prior scales — moderate flexibility
            cp_pscale_txtbox.Text = "0.05";     // Trend changepoint flexibility
            s_pscale_txtbox.Text = "10";        // Seasonality strength

            // Target column setup
            target_dropdown.Items.Clear();
            target_dropdown.Items.AddRange(features.ToArray());
            if (target_dropdown.Items.Count > 0)
            {
                target_dropdown.SelectedItem = target_dropdown.Items[0];
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunProphetPrediction();
        }
    }
}