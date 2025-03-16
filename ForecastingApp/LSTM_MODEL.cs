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
        public LSTM_MODEL(MainForm form)
        {
            InitializeComponent();
            mainForm = form;
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
                string scriptPath = Path.Combine(Application.StartupPath, "..", "..", "..", "Scripts", "LSTM_Model.py");

                string epochs = epoch_textbox.Text;
                string neuronslvl1 = neuronslvl1_textbox.Text;
                string neuronslvl2 = neuronslvl2_textbox.Text;
                string batchSize = batchsize_textbox.Text;
                string timeSteps = timesteps_textbox.Text;

                string arguments = $"\"{scriptPath}\" {epochs} {neuronslvl1} {neuronslvl2} {batchSize} {timeSteps}";


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
            RunPythonPrediction();
        }
    }

}
