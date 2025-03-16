namespace ForecastingApp
{
    partial class LSTM_MODEL
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.epoch_textbox = new System.Windows.Forms.TextBox();
            this.neuronslvl1_textbox = new System.Windows.Forms.TextBox();
            this.batchsize_textbox = new System.Windows.Forms.TextBox();
            this.timesteps_textbox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.neuronslvl2_textbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Epochs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Neurons Level 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(457, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Time Steps";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 233);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Batch Size";
            // 
            // epoch_textbox
            // 
            this.epoch_textbox.Location = new System.Drawing.Point(176, 69);
            this.epoch_textbox.Name = "epoch_textbox";
            this.epoch_textbox.Size = new System.Drawing.Size(177, 22);
            this.epoch_textbox.TabIndex = 4;
            // 
            // neuronslvl1_textbox
            // 
            this.neuronslvl1_textbox.Location = new System.Drawing.Point(176, 119);
            this.neuronslvl1_textbox.Name = "neuronslvl1_textbox";
            this.neuronslvl1_textbox.Size = new System.Drawing.Size(177, 22);
            this.neuronslvl1_textbox.TabIndex = 5;
            // 
            // batchsize_textbox
            // 
            this.batchsize_textbox.Location = new System.Drawing.Point(176, 227);
            this.batchsize_textbox.Name = "batchsize_textbox";
            this.batchsize_textbox.Size = new System.Drawing.Size(177, 22);
            this.batchsize_textbox.TabIndex = 6;
            // 
            // timesteps_textbox
            // 
            this.timesteps_textbox.Location = new System.Drawing.Point(539, 69);
            this.timesteps_textbox.Name = "timesteps_textbox";
            this.timesteps_textbox.Size = new System.Drawing.Size(177, 22);
            this.timesteps_textbox.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(341, 363);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 40);
            this.button1.TabIndex = 8;
            this.button1.Text = "Train";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(56, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(148, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "HYPERPARAMETERS";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(53, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 16);
            this.label6.TabIndex = 10;
            this.label6.Text = "Neurons Level 2";
            // 
            // neuronslvl2_textbox
            // 
            this.neuronslvl2_textbox.Location = new System.Drawing.Point(176, 173);
            this.neuronslvl2_textbox.Name = "neuronslvl2_textbox";
            this.neuronslvl2_textbox.Size = new System.Drawing.Size(177, 22);
            this.neuronslvl2_textbox.TabIndex = 11;
            // 
            // LSTM_MODEL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.neuronslvl2_textbox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.timesteps_textbox);
            this.Controls.Add(this.batchsize_textbox);
            this.Controls.Add(this.neuronslvl1_textbox);
            this.Controls.Add(this.epoch_textbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "LSTM_MODEL";
            this.Text = "LSTM_MODEL";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LSTM_MODEL_CLOSE);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox epoch_textbox;
        private System.Windows.Forms.TextBox neuronslvl1_textbox;
        private System.Windows.Forms.TextBox batchsize_textbox;
        private System.Windows.Forms.TextBox timesteps_textbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox neuronslvl2_textbox;
    }
}