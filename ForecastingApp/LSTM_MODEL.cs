using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }

}
