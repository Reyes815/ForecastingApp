using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Numerics;

namespace ForecastingApp
{
    public partial class XGBoost : Form
    {
        private MainForm mainForm;
        private string selectedCSVFile;

        public XGBoost(MainForm mainForm, string selectedCSVFile)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.selectedCSVFile = selectedCSVFile;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.Show();
        }

        private void RunPythonScript()
        {
            try
            {
                if (string.IsNullOrEmpty(selectedCSVFile))
                {
                    MessageBox.Show("Please upload a CSV file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int lags = int.TryParse(this.Lags?.Text, out int l) ? l : 5;
                double testSize = double.TryParse(this.TestSize?.Text, out double t) ? t : 0.2;
                int maxDepth = int.TryParse(this.MaxDepth?.Text, out int md) ? md : 6;
                double learningRate = double.TryParse(this.LearningRate?.Text, out double lr) ? lr : 0.1;
                int estimators = int.TryParse(this.Estimators?.Text, out int est) ? est : 100;
                double fraction = double.TryParse(this.Fraction?.Text, out double f) ? f : 0.8;
                double subSample = double.TryParse(this.SubSample?.Text, out double ss) ? ss : 1.0;
                double l1 = double.TryParse(this.L1?.Text, out double l1Val) ? l1Val : 0.0;
                double l2 = double.TryParse(this.L2?.Text, out double l2Val) ? l2Val : 1.0;

                // Ensure Python.NET is initialized
                if (!PythonEngine.IsInitialized)
                {
                    PythonEngine.Initialize();
                }

                using (Py.GIL()) // Acquire Global Interpreter Lock (GIL)
                {
                    dynamic sys = Py.Import("sys");
                    //sys.path.append(@"C:\Users\LENOVO\Source\Repos\ForecastingApp\ForecastingApp\Scripts");
                    sys.path.append(@"C:\Users\gianl\source\repos\ForecastingApp\ForecastingApp\Scripts");

                    dynamic train_model = Py.Import("xgboost_forecasting");

                    // Create Python dictionary for hyperparameters
                    using (var paramDict = new PyDict())
                    {
                        paramDict["max_depth"] = new PyInt(maxDepth);
                        paramDict["learning_rate"] = new PyFloat(learningRate);
                        paramDict["n_estimators"] = new PyInt(estimators);
                        paramDict["subsample"] = new PyFloat(subSample);
                        paramDict["colsample_bytree"] = new PyFloat(fraction);
                        paramDict["reg_alpha"] = new PyFloat(l1);
                        paramDict["reg_lambda"] = new PyFloat(l2);

                        // Run Python script
                        dynamic results = train_model.run(selectedCSVFile, paramDict, lags, testSize);

                        double mae = results["mae"].As<double>();
                        double rmse = results["rmse"].As<double>();
                        double mape = results["mape"].As<double>();

                        MessageBox.Show($"Model Performance:\nMAE: {mae}\nRMSE: {rmse}\nMAPE: {mape}", "Results");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error running Python script: " + ex.Message, "Python Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunPythonScript();
        }

        private void Lags_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
