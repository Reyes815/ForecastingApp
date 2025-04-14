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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ForecastingApp
{
    public partial class Results : Form
    {
        private string predictionImgPath;
        private string lossImagePath;
        private LSTM_MODEL form;

        public Results(LSTM_MODEL form)
        {
            InitializeComponent();
            LoadMetrics();
            this.Resize += Results_Resize;
            this.form = form;
        }

        private void LoadMetrics()
        {
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;

            // Get the current working directory (output folder like bin\x64\Debug)
            string workingDirectory = Directory.GetCurrentDirectory();

            // Calculate the path for the "Scripts\Metrics" folder based on the project root
            string projectRoot = Path.GetFullPath(Path.Combine(workingDirectory, @"..\..\..\"));
            string metricsFolder = Path.Combine(projectRoot, "Scripts", "Metrics");
            string graphsFolder = Path.Combine(projectRoot, "Scripts", "Graphs");

            // Define the file path for the saved metrics JSON
            string metricsFilePath = Path.Combine(metricsFolder, "lstm_metrics.json");

            // Graphs file path
            string predictionGraphPath = Path.Combine(graphsFolder, "actual_vs_predicted.png");
            string lossGraphPath = Path.Combine(graphsFolder, "training_vs_validation_loss.png");

            if (File.Exists(metricsFilePath))
            {
                string jsonContent = File.ReadAllText(metricsFilePath);

                JObject jsonObject = JObject.Parse(jsonContent);


                // Access values using keys
                double loss = jsonObject["Loss (MSE)"].Value<double>();
                double mae = jsonObject["Mean Absolute Error (MAE)"].Value<double>();
                double r2 = jsonObject["R2 Score"].Value<double>();
                double rmse = jsonObject["Root Mean Squared Error (RMSE)"].Value<double>();
                double mape = jsonObject["Mean Absolute Percentage Error (MAPE)"].Value<double>();
                double accuracy = jsonObject["Accuracy (100 - MAPE)%"].Value<double>();
                double trainingDuration = jsonObject["Training Duration (seconds)"].Value<double>();


                string[] metric_names =
                {   "Loss (MSE)",
                    "Mean Absolute Error (MAE)",
                    "R2 Score",
                    "Root Mean Squared Error (RMSE)",
                    "Mean Absolute Percentage Error (MAPE)",
                    "Accuracy",
                    "Training Duration (seconds)"
                };
                double[] metrics = { loss, mae, r2, rmse, mape, accuracy, trainingDuration };

                for (int i = 0; i < metrics.Length; i++)
                {
                    Label metricLabel = new Label
                    {
                        Text = $"{metric_names[i]}: {metrics[i]}",
                        Font = new Font("Arial", 11, FontStyle.Regular),
                        AutoSize = true
                    };

                    flowLayoutPanel1.Controls.Add(metricLabel);
                }

                LoadImageWithLabel("Actual vs. Predicted Sales", predictionGraphPath);
                LoadImageWithLabel("Training Loss over Epochs", lossGraphPath);
            }
        }

        public void LoadImageWithLabel(string title, string imgpath)
        {
            if (File.Exists(imgpath))
            {
                Label titleLabel = new Label
                {
                    Text = title,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    AutoSize = true
                };
                flowLayoutPanel1.Controls.Add(titleLabel);

                PictureBox pictureBox = new PictureBox
                {
                    Image = Image.FromFile(imgpath),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = flowLayoutPanel1.ClientSize.Width - 20,
                    Height = (flowLayoutPanel1.ClientSize.Height - 20) * 3 / 4
                };
                flowLayoutPanel1.Controls.Add(pictureBox);
            }
        }

        private void Results_Resize(object sender, EventArgs e)
        {
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Width = flowLayoutPanel1.ClientSize.Width - 20;
                    pictureBox.Height = (pictureBox.Width * 3) / 4; // Maintain aspect ratio
                }
            }
        }

        private void RESULTS_CLOSE(object sender, FormClosedEventArgs e)
        {
            form.Show(); // Closes the entire application when Form2 is closed
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
