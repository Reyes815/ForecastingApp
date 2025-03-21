using System;
using System.Diagnostics;
using System.IO;
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

            string pythonPath = FindRealPython();
            if (pythonPath == null)
            {
                MessageBox.Show("Python installation with python312.dll not found!");
                return;
            }

            string pythonDll = Path.Combine(Path.GetDirectoryName(pythonPath), "python312.dll");
            if (File.Exists(pythonDll))
            {
                //MessageBox.Show($"Found python312.dll at: {pythonDll}");
            }
            else
            {
                MessageBox.Show("python312.dll not found in the selected Python installation!");
            }

            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);


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

        static string FindRealPython()
        {
            // Run 'where python' command
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "where",
                Arguments = "python",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                string[] paths = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string path in paths)
                {
                    // Ignore Microsoft Store Python
                    if (!path.Contains("WindowsApps"))
                    {
                        return path.Trim();  // Return the first valid Python path
                    }
                }
            }

            return null; // No valid Python installation found
        }
    }
}
