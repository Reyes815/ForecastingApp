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
using Python.Runtime;

namespace ForecastingApp
{
    public partial class LSTM_MODEL : Form
    {
        private MainForm mainForm;
        private string cvs_filepath;
        private List<string> features;
        //private Stopwatch stopwatch;
        public LSTM_MODEL(MainForm form, string selectedcsv, List<string> processed_features)
        {
            InitializeComponent();
            mainForm = form;
            cvs_filepath = selectedcsv;
            features = processed_features;
            //stopwatch = new Stopwatch();
            
        }

        private void LSTM_MODEL_CLOSE(object sender, FormClosedEventArgs e)
        {
            mainForm.Show(); // Closes the entire application when Form2 is closed
        }

        private void RunPythonPrediction()
        {
            try
            {
                string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string pythonExecutable = Path.Combine(projectRoot, "Scripts", "python_env", "Scripts", "python.exe");
                string picklefilepath = Path.Combine(projectRoot, "Scripts", "Saved preprocessed csv", $"{cvs_filepath}");
                string scriptPath = Path.Combine(Application.StartupPath, "..", "..", "..", "Scripts", "LSTM_Model.py");

                string epochs = epoch_textbox.Text;
                string neuronslvl1 = neuronslvl1_textbox.Text;
                string neuronslvl2 = neuronslvl2_textbox.Text;
                string batchSize = batchsize_textbox.Text;
                string timeSteps = timesteps_textbox.Text;
                string target = target_dropdown.Text;

                string arguments = $"\"{scriptPath}\" {epochs} {neuronslvl1} {neuronslvl2} {batchSize} {timeSteps} \"{target}\" \"{picklefilepath}\"";


                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pythonExecutable,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = false
                };

                Process process = new Process { StartInfo = psi, EnableRaisingEvents = true };

                process.Exited += new EventHandler(Process_Exited);

                //stopwatch.Start();
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Python Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            //stopwatch.Stop();
            //TimeSpan trainingDuration = stopwatch.Elapsed;

            this.BeginInvoke((MethodInvoker)delegate
            {
                Results resultsForm = new Results();
                resultsForm.Show();
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunPythonPrediction();
        }

        private void LSTM_Form_Load(object sender, EventArgs e)
        {
            // Set default values for textboxes
            epoch_textbox.Text = "1";
            neuronslvl1_textbox.Text = "64";
            neuronslvl2_textbox.Text = "32";
            batchsize_textbox.Text = "16";
            timesteps_textbox.Text = "5";
            target_dropdown.Items.AddRange(features.ToArray());
        }
    }

}
