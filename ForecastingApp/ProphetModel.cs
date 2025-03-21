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
    public partial class ProphetModel : Form
    {
        private MainForm mainForm;
        private string filePath;

        public ProphetModel(MainForm mainForm, string path)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.filePath = path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunProphetScript(filePath);
        }

        private void RunProphetScript(string path)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                //psi.FileName = @"C:\Python312\python.exe"; // Ensure Python is in the system PATH

                // Pass the file path as an argument to the script
                string scriptPath = Path.Combine(Application.StartupPath, "..", "..", "..", "Scripts", "Prophet_forecast.py");
                psi.Arguments = $"\"{scriptPath}\" \"{path}\"";

                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.CreateNoWindow = true;

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo = psi;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Display output or error
                MessageBox.Show(output);
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show("Error: " + error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running Python script: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void ProphetModel_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
