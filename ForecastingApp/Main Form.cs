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
        private string LSTM_scaler = "";
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
                "Return your response in this exact format:\n\n" +
                "DATASET_ANALYSIS: <Two paragraphs, 2-3 sentences each, summarizing the dataset structure, patterns, and any challenges.>\n" +
                "MODEL_RECOMMENDATION: <Only one word — either LSTM, XGBoost, or Prophet. Do not explain the choice.>\n\n" +
                "Do not include any markdown formatting, JSON, or extra text outside of this format.";

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

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                try
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                    string fullResponse = jsonResponse.candidates[0].content.parts[0].text.ToString();

                    // Split using fixed markers
                    var datasetMarker = "DATASET_ANALYSIS:";
                    var modelMarker = "MODEL_RECOMMENDATION:";

                    int datasetStart = fullResponse.IndexOf(datasetMarker);
                    int modelStart = fullResponse.IndexOf(modelMarker);

                    if (datasetStart == -1 || modelStart == -1)
                        return ("Could not parse dataset analysis.", "Parsing Error");

                    string datasetAnalysis = fullResponse.Substring(datasetStart + datasetMarker.Length, modelStart - (datasetStart + datasetMarker.Length)).Trim();
                    string modelRecommendation = fullResponse.Substring(modelStart + modelMarker.Length).Trim().Split('\n')[0];

                    return (datasetAnalysis, modelRecommendation);
                }
                catch
                {
                    return ("Error analyzing dataset.", "Error getting model recommendation.");
                }
            }
        }

        //Get recommended hyperparameters for Prophet model from gemini
        private async Task<string> get_Prophet_Suggestions(List<string> features)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={API_Key}";
            string featureList = string.Join(", ", features);
            string prompt = $"You are configuring the Prophet model for a dataset with the following features: {featureList}. " +
                "Return hyperparameter tuning suggestions as a JSON dictionary with the following keys: " +
                "'Forecast period', 'train ratio', 'daily', 'weekly', 'monthly', 'yearly', 'standardization', 'holidays enabled', " +
                "'growth', 'seasonality mode', 'changepoint prior scale', 'seasonality prior scale', 'target column'. " +
                "Use default if no other answer. daily, weekly, monthly yearly standardization and holidays are true/false only. " +
                "Only respond with the raw JSON object — do not explain anything. Do not include anyother text aside form the hyperparameters given";

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

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
                var responseString = await response.Content.ReadAsStringAsync();

                dynamic result = JsonConvert.DeserializeObject(responseString);
                string rawOutput = result.candidates[0].content.parts[0].text;

                return rawOutput; // This is the raw JSON string response
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

                    this.selectedCSVFile = result["saved_pickle"];
                    this.LSTM_scaler = result["saved_scaler"];

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
                    LSTM_MODEL newLSTM_FORM = new LSTM_MODEL(this, this.selectedCSVFile, this.LSTM_scaler, this.features);
                    newLSTM_FORM.Show();
                    break;
                case "XGBoost":
                    this.Hide();
                    var xgb = new XGBoost(this, rawCSVFile);
                    xgb.Show();
                    break;
                case "Prophet":
                    this.Hide();
                    var hyperparams = await get_Prophet_Suggestions(features);
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
