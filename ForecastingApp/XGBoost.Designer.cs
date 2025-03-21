namespace ForecastingApp
{
    partial class XGBoost
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
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Hyperparameters = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.TestSize = new System.Windows.Forms.TextBox();
            this.MaxDepth = new System.Windows.Forms.TextBox();
            this.LearningRate = new System.Windows.Forms.TextBox();
            this.SubSample = new System.Windows.Forms.TextBox();
            this.L2 = new System.Windows.Forms.TextBox();
            this.L1 = new System.Windows.Forms.TextBox();
            this.Fraction = new System.Windows.Forms.TextBox();
            this.Estimators = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.Lags = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.uploadToolStripMenuItem.Text = "Upload";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Hyperparameters
            // 
            this.Hyperparameters.AutoSize = true;
            this.Hyperparameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Hyperparameters.Location = new System.Drawing.Point(12, 39);
            this.Hyperparameters.Name = "Hyperparameters";
            this.Hyperparameters.Size = new System.Drawing.Size(170, 24);
            this.Hyperparameters.TabIndex = 8;
            this.Hyperparameters.Text = "Hyperparameters";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(100, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Lags";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(69, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Test Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(58, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Max Depth";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(34, 275);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Learning Rate";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(59, 337);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Estimators";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(426, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Fraction";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(466, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(27, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "L1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(466, 216);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "L2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(397, 275);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 20);
            this.label9.TabIndex = 17;
            this.label9.Text = "Sub Sample";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(303, 395);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 33);
            this.button1.TabIndex = 18;
            this.button1.Text = "Train";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TestSize
            // 
            this.TestSize.Location = new System.Drawing.Point(159, 161);
            this.TestSize.Name = "TestSize";
            this.TestSize.Size = new System.Drawing.Size(100, 20);
            this.TestSize.TabIndex = 20;
            // 
            // MaxDepth
            // 
            this.MaxDepth.Location = new System.Drawing.Point(159, 209);
            this.MaxDepth.Name = "MaxDepth";
            this.MaxDepth.Size = new System.Drawing.Size(100, 20);
            this.MaxDepth.TabIndex = 21;
            // 
            // LearningRate
            // 
            this.LearningRate.Location = new System.Drawing.Point(159, 275);
            this.LearningRate.Name = "LearningRate";
            this.LearningRate.Size = new System.Drawing.Size(100, 20);
            this.LearningRate.TabIndex = 22;
            // 
            // SubSample
            // 
            this.SubSample.Location = new System.Drawing.Point(512, 275);
            this.SubSample.Name = "SubSample";
            this.SubSample.Size = new System.Drawing.Size(100, 20);
            this.SubSample.TabIndex = 23;
            // 
            // L2
            // 
            this.L2.Location = new System.Drawing.Point(512, 209);
            this.L2.Name = "L2";
            this.L2.Size = new System.Drawing.Size(100, 20);
            this.L2.TabIndex = 24;
            // 
            // L1
            // 
            this.L1.Location = new System.Drawing.Point(512, 159);
            this.L1.Name = "L1";
            this.L1.Size = new System.Drawing.Size(100, 20);
            this.L1.TabIndex = 25;
            // 
            // Fraction
            // 
            this.Fraction.Location = new System.Drawing.Point(512, 100);
            this.Fraction.Name = "Fraction";
            this.Fraction.Size = new System.Drawing.Size(100, 20);
            this.Fraction.TabIndex = 26;
            // 
            // Estimators
            // 
            this.Estimators.Location = new System.Drawing.Point(159, 339);
            this.Estimators.Name = "Estimators";
            this.Estimators.Size = new System.Drawing.Size(100, 20);
            this.Estimators.TabIndex = 27;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(713, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 29;
            this.button3.Text = "Back";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Lags
            // 
            this.Lags.Location = new System.Drawing.Point(159, 96);
            this.Lags.Name = "Lags";
            this.Lags.Size = new System.Drawing.Size(100, 20);
            this.Lags.TabIndex = 30;
            // 
            // XGBoost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Lags);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Estimators);
            this.Controls.Add(this.Fraction);
            this.Controls.Add(this.L1);
            this.Controls.Add(this.L2);
            this.Controls.Add(this.SubSample);
            this.Controls.Add(this.LearningRate);
            this.Controls.Add(this.MaxDepth);
            this.Controls.Add(this.TestSize);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Hyperparameters);
            this.Controls.Add(this.menuStrip1);
            this.Name = "XGBoost";
            this.Text = "XGBoost";
            this.Load += new System.EventHandler(this.XGBoost_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Label Hyperparameters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TestSize;
        private System.Windows.Forms.TextBox MaxDepth;
        private System.Windows.Forms.TextBox LearningRate;
        private System.Windows.Forms.TextBox SubSample;
        private System.Windows.Forms.TextBox L2;
        private System.Windows.Forms.TextBox L1;
        private System.Windows.Forms.TextBox Fraction;
        private System.Windows.Forms.TextBox Estimators;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox Lags;
    }
}