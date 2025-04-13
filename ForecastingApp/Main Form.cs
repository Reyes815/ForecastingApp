using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ForecastingApp
{
    public partial class MainForm : Form
    {
        private string selected_model = "";
        private string filePath = "";
        private string rawCSVFile = "";
        private string selectedCSVFile = "";
        private List<string> features = new List<string>();
        private static readonly HttpClient client = new HttpClient();
        public readonly string API_Key = "AIzaSyBpw0WyxmUU2F7PsMQMf1Xh9C89GtGpRl0";

        public MainForm()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Features", "Features");
            dataGridView1.Columns.Add("type", "Types");
            dataGridView1.Columns.Add("unique_vals", "Number of Unique Values");
            dataGridView1.Columns.Add("null_vals", "Number of Null Values");
            dataGridView1.Columns.Add("category", "Category");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.Checked)
            {
                selected_model = rb.Text;
            }
        }

        private async Task<(string datasetAnalysis, string modelRecommendation)> GetAnalysisFromGemini(List<string> features)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={API_Key}";
            string featureList = string.Join(", ", features);
            string prompt = $"You are analyzing a dataset with the following features: {featureList}. " +
                "DATASET_ANALYSIS: In two paragraph (5 sentences), describe key insights about the dataset's structure, relationships among features, and any challenges that may affect forecasting. " +
                "MODEL_RECOMMENDATION: Based on the features, recommend the most suitable model for sales forecasting by replying with only one word: LSTM, XGBoost, or Prophet.";

            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            try
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                string fullResponse = jsonResponse.candidates[0].content.parts[0].text.ToString();
                int modelIndex = fullResponse.IndexOf("MODEL_RECOMMENDATION:");
                string datasetAnalysis = modelIndex > 0 ? fullResponse.Substring(fullResponse.IndexOf("DATASET_ANALYSIS:") + "DATASET_ANALYSIS:".Length, modelIndex - "DATASET_ANALYSIS:".Length).Trim() : "Could not parse dataset analysis.";
                string modelRecommendation = modelIndex > 0 ? fullResponse.Substring(modelIndex + "MODEL_RECOMMENDATION:".Length).Trim() : fullResponse;
                return (datasetAnalysis, modelRecommendation);
            }
            catch
            {
                return ("Error analyzing dataset.", "Error getting model recommendation.");
            }
        }

        private async Task<Dictionary<string, string>> GetProphetHyperparamSuggestionsFromGemini(List<string> features)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={API_Key}";
            string featureList = string.Join(", ", features);
            string prompt = $"You are configuring the Prophet model for a dataset with the following features: {featureList}. " +
                "Return hyperparameter tuning suggestions as a JSON dictionary with the following keys: " +
                "'forecast_period', 'train_ratio', 'weekly', 'monthly', 'yearly', 'standardization', 'holidays_enabled', " +
                "'growth', 'seasonality_mode', 'changepoint_prior_scale', 'seasonality_prior_scale', 'target_column'. " +
                "Only respond with the raw JSON object — do not explain anything.";

            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            try
            {
                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                string jsonPart = jsonResponse.candidates[0].content.parts[0].text;
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPart);
            }
            catch
            {
                return new Dictionary<string, string>
                {
                    { "forecast_period", "30" },
                    { "train_ratio", "0.8" },
                    { "weekly", "true" },
                    { "monthly", "true" },
                    { "yearly", "true" },
                    { "standardization", "false" },
                    { "holidays_enabled", "false" },
                    { "growth", "linear" },
                    { "seasonality_mode", "additive" },
                    { "changepoint_prior_scale", "0.05" },
                    { "seasonality_prior_scale", "10" },
                    { "target_column", features.FirstOrDefault() ?? "Sales" }
                };
            }
        }

        private async Task ProcessCSVFile(string filePath)
        {
            try
            {
                string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string pythonExecutable = Path.Combine(projectRoot, "Scripts", "python_env", "Scripts", "python.exe");
                string scriptPath = Path.Combine(Application.StartupPath, "..", "..", "..", "Scripts", "process_dataset_csv.py");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExecutable,
                    Arguments = $"\"{scriptPath}\" \"{filePath}\"",
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

                    var result = JsonConvert.DeserializeObject<dynamic>(output);

                    if (result["error"] != null)
                    {
                        MessageBox.Show("Error: " + result["error"], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    label2.Text = result["filename"];
                    label3.Text = $"Instances: {result["instances"]}";
                    label4.Text = $"Features Before Encoding: {result["features_before_encoding"]}";
                    label8.Text = $"Features After Encoding: {result["features_after_encoding"]}";

                    selectedCSVFile = result["saved_pickle"];
                    features.Clear();
                    dataGridView1.Rows.Clear();
                    foreach (var column in result["columns"])
                    {
                        string featureName = column["name"].ToString();
                        features.Add(featureName);
                        dataGridView1.Rows.Add(column["name"], column["type"], column["unique_values"], column["null_values"], column["category"]);
                    }

                    var (datasetAnalysis, modelRecommendation) = await GetAnalysisFromGemini(features);
                    label_Dataset.Text = datasetAnalysis;
                    label_Model.Text = modelRecommendation;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running Python script: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void UploadMenuBtn(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedCSVFile = openFileDialog.FileName;
                    rawCSVFile = selectedCSVFile;
                    await ProcessCSVFile(selectedCSVFile);
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            switch (selected_model)
            {
                case "LSTM":
                    this.Hide();
                    var lstm = new LSTM_MODEL(this, selectedCSVFile, features);
                    lstm.Show();
                    break;
                case "XGBoost":
                    this.Hide();
                    var xgb = new XGBoost(this, rawCSVFile);
                    xgb.Show();
                    break;
                case "Prophet":
                    this.Hide();
                    var hyperparams = await GetProphetHyperparamSuggestionsFromGemini(features);
                    var prophet = new ProphetModel(this, rawCSVFile, features, hyperparams);
                    prophet.Show();
                    break;
                default:
                    MessageBox.Show("Please select a model first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }
    }
}
