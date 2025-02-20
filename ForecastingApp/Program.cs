using System;
using System.Windows.Forms;
using Python.Runtime;

namespace ForecastingApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Set the Python runtime path before initializing PythonEngine
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", @"C:\Users\Rowen\AppData\Local\Programs\Python\Python310\python310.dll");

            try
            {
                PythonEngine.Initialize(); // Initialize Python.NET
            }
            catch (Exception ex)
            {
                MessageBox.Show("Buh Python.NET Initialization Failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit if Python.NET fails
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            // Shutdown Python.NET when closing the application
            PythonEngine.Shutdown();
        }
    }
}
