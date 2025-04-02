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
        //private TimeSpan trainingDuration;
        //private Dictionary<string, string> metrics;
        private string predictionImgPath;
        private string lossImagePath;

        public Results()
        {
            InitializeComponent();
            LoadMetrics();
        }

        private void LoadMetrics()
        {
            string metricsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "Metrics", "lstm_metrics.json");
            string predictionGraphPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "Graphs", "actual_vs_predicted.png");
            string lossGraphPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "Graphs", "training_vs_validation_loss.png");

            if (File.Exists(metricsFilePath))
            {
                string jsonContent = File.ReadAllText(metricsFilePath);

                JObject jsonObject = JObject.Parse(jsonContent);

                // Access values using keys
                double loss = jsonObject["Loss"].Value<double>();
                double mae = jsonObject["MAE"].Value<double>();
                double r2 = jsonObject["R2"].Value<double>();
                double trainingDuration = jsonObject["TrainingDuration"].Value<double>();


                string[] metric_names = { "Loss", "MAE", "R2", "Training Duration" };
                double[] metrics = { loss, mae, r2, trainingDuration };

                //foreach (var entry in metrics)
                //{
                //    Label metricLabel = new Label
                //    {
                //        Text = $"{entry.Key}: {entry.Value}",
                //        Font = new Font("Arial", 11),
                //        AutoSize = true
                //    };
                //    flowLayoutPanel1.Controls.Add(metricLabel);
                //}

                //for (int i = 0; i < metrics.Length; i++)
                //{
                //    Label metricLabel = new Label
                //    {
                //        Text = $"{metrics[0]}: {metric_names[0]}",
                //        //Text = "jlkdsflksjkldfjsldfkjklsf",
                //        Font = new Font("Arial", 11, FontStyle.Regular),
                //        AutoSize = true
                //    };
                //    flowLayoutPanel1.Controls.Add(metricLabel);
                //}

                //LoadImageWithLabel("Actual vs. Predicted Sales", predictionGraphPath);
                //LoadImageWithLabel("Training Loss over Epochs", lossGraphPath);
            }

            Label metricLabel = new Label
            {
                //Text = $"{metrics[0]}: {metric_names[0]}",
                Text = metricsFilePath,
                Font = new Font("Arial", 11, FontStyle.Regular),
                AutoSize = true
            };
            flowLayoutPanel1.Controls.Add(metricLabel);
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
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 400,
                    Height = 300
                };
                flowLayoutPanel1.Controls.Add(pictureBox);
            }
        }
    }
}
