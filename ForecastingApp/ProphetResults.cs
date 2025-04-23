using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
    public partial class ProphetResults : Form
    {
        public ProphetResults()
        {
            InitializeComponent();
            LoadResults();
        }

        private void LoadResults()
        {
            try
            {
                string outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output");
                string jsonPath = Path.Combine(outputDir, "forecast_data.json");

                if (!File.Exists(jsonPath))
                {
                    MessageBox.Show("Forecast data file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Load and parse JSON
                string json = File.ReadAllText(jsonPath);
                JObject result = JObject.Parse(json);

                // Load graph image
                string imagePath = Directory.GetFiles(outputDir, "*_forecast.png").FirstOrDefault();
                if (imagePath != null)
                {
                    graphPicBox.Image = Image.FromFile(imagePath);
                    graphPicBox.SizeMode = PictureBoxSizeMode.Zoom;
                }

                // Optional: Load forecast table
                var forecastData = result["forecast"].ToString();
                DataTable table = JsonConvert.DeserializeObject<DataTable>(forecastData);
                dataGridView.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load results:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
