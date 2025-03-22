namespace ForecastingApp
{
    partial class ProphetModel
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
            this.train_txtbox = new System.Windows.Forms.TextBox();
            this.period_txtbox = new System.Windows.Forms.TextBox();
            this.train_btn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Hyperparameters = new System.Windows.Forms.Label();
            this.weekly_rbtn = new System.Windows.Forms.RadioButton();
            this.monthly_rbtn = new System.Windows.Forms.RadioButton();
            this.Disable_rbtn = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.test_txtbox = new System.Windows.Forms.TextBox();
            this.split_txtbox = new System.Windows.Forms.TextBox();
            this.multiplicative_rbtn = new System.Windows.Forms.RadioButton();
            this.additive_rbtn = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.logistic_rbtn = new System.Windows.Forms.RadioButton();
            this.linear_rbtn = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.seasonality_grpbox = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SeasonMode_grpbox = new System.Windows.Forms.GroupBox();
            this.Growth_grpbox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.target_dropdown = new System.Windows.Forms.ComboBox();
            this.Holiday_grpbox = new System.Windows.Forms.GroupBox();
            this.Enable_rbtn = new System.Windows.Forms.RadioButton();
            this.Standard_grpbox = new System.Windows.Forms.GroupBox();
            this.Enable_rbtn2 = new System.Windows.Forms.RadioButton();
            this.Disable_rbtn2 = new System.Windows.Forms.RadioButton();
            this.seasonality_grpbox.SuspendLayout();
            this.SeasonMode_grpbox.SuspendLayout();
            this.Growth_grpbox.SuspendLayout();
            this.Holiday_grpbox.SuspendLayout();
            this.Standard_grpbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // train_txtbox
            // 
            this.train_txtbox.Location = new System.Drawing.Point(257, 204);
            this.train_txtbox.Name = "train_txtbox";
            this.train_txtbox.Size = new System.Drawing.Size(40, 20);
            this.train_txtbox.TabIndex = 48;
            // 
            // period_txtbox
            // 
            this.period_txtbox.Location = new System.Drawing.Point(66, 115);
            this.period_txtbox.Name = "period_txtbox";
            this.period_txtbox.Size = new System.Drawing.Size(100, 20);
            this.period_txtbox.TabIndex = 40;
            // 
            // train_btn
            // 
            this.train_btn.Location = new System.Drawing.Point(355, 326);
            this.train_btn.Name = "train_btn";
            this.train_btn.Size = new System.Drawing.Size(107, 33);
            this.train_btn.TabIndex = 39;
            this.train_btn.Text = "Train";
            this.train_btn.UseVisualStyleBackColor = true;
            this.train_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(427, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 20);
            this.label6.TabIndex = 35;
            this.label6.Text = "Standardization";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(253, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 20);
            this.label5.TabIndex = 34;
            this.label5.Text = "Train - Test - Split";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(49, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 20);
            this.label2.TabIndex = 31;
            this.label2.Text = "Seasonality Frequency";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(49, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 20);
            this.label1.TabIndex = 30;
            this.label1.Text = "Forecasting Period";
            // 
            // Hyperparameters
            // 
            this.Hyperparameters.AutoSize = true;
            this.Hyperparameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Hyperparameters.Location = new System.Drawing.Point(23, 20);
            this.Hyperparameters.Name = "Hyperparameters";
            this.Hyperparameters.Size = new System.Drawing.Size(170, 24);
            this.Hyperparameters.TabIndex = 29;
            this.Hyperparameters.Text = "Hyperparameters";
            // 
            // weekly_rbtn
            // 
            this.weekly_rbtn.AutoSize = true;
            this.weekly_rbtn.Location = new System.Drawing.Point(0, 34);
            this.weekly_rbtn.Name = "weekly_rbtn";
            this.weekly_rbtn.Size = new System.Drawing.Size(118, 17);
            this.weekly_rbtn.TabIndex = 49;
            this.weekly_rbtn.TabStop = true;
            this.weekly_rbtn.Text = "Weekly Seasonality";
            this.weekly_rbtn.UseVisualStyleBackColor = true;
            // 
            // monthly_rbtn
            // 
            this.monthly_rbtn.AutoSize = true;
            this.monthly_rbtn.Location = new System.Drawing.Point(1, 11);
            this.monthly_rbtn.Name = "monthly_rbtn";
            this.monthly_rbtn.Size = new System.Drawing.Size(119, 17);
            this.monthly_rbtn.TabIndex = 50;
            this.monthly_rbtn.TabStop = true;
            this.monthly_rbtn.Text = "Monthly Seasonality";
            this.monthly_rbtn.UseVisualStyleBackColor = true;
            // 
            // Disable_rbtn
            // 
            this.Disable_rbtn.AutoSize = true;
            this.Disable_rbtn.Location = new System.Drawing.Point(0, 36);
            this.Disable_rbtn.Name = "Disable_rbtn";
            this.Disable_rbtn.Size = new System.Drawing.Size(60, 17);
            this.Disable_rbtn.TabIndex = 52;
            this.Disable_rbtn.TabStop = true;
            this.Disable_rbtn.Text = "Disable";
            this.Disable_rbtn.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(253, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.TabIndex = 51;
            this.label4.Text = "Holidays";
            // 
            // test_txtbox
            // 
            this.test_txtbox.Location = new System.Drawing.Point(303, 204);
            this.test_txtbox.Name = "test_txtbox";
            this.test_txtbox.Size = new System.Drawing.Size(40, 20);
            this.test_txtbox.TabIndex = 53;
            // 
            // split_txtbox
            // 
            this.split_txtbox.Location = new System.Drawing.Point(349, 204);
            this.split_txtbox.Name = "split_txtbox";
            this.split_txtbox.Size = new System.Drawing.Size(40, 20);
            this.split_txtbox.TabIndex = 54;
            // 
            // multiplicative_rbtn
            // 
            this.multiplicative_rbtn.AutoSize = true;
            this.multiplicative_rbtn.Location = new System.Drawing.Point(0, 36);
            this.multiplicative_rbtn.Name = "multiplicative_rbtn";
            this.multiplicative_rbtn.Size = new System.Drawing.Size(86, 17);
            this.multiplicative_rbtn.TabIndex = 58;
            this.multiplicative_rbtn.TabStop = true;
            this.multiplicative_rbtn.Text = "Multiplicative";
            this.multiplicative_rbtn.UseVisualStyleBackColor = true;
            // 
            // additive_rbtn
            // 
            this.additive_rbtn.AutoSize = true;
            this.additive_rbtn.Location = new System.Drawing.Point(0, 11);
            this.additive_rbtn.Name = "additive_rbtn";
            this.additive_rbtn.Size = new System.Drawing.Size(63, 17);
            this.additive_rbtn.TabIndex = 57;
            this.additive_rbtn.TabStop = true;
            this.additive_rbtn.Text = "Additive";
            this.additive_rbtn.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(427, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 20);
            this.label3.TabIndex = 56;
            this.label3.Text = "Seasonality Mode";
            // 
            // logistic_rbtn
            // 
            this.logistic_rbtn.AutoSize = true;
            this.logistic_rbtn.Location = new System.Drawing.Point(0, 36);
            this.logistic_rbtn.Name = "logistic_rbtn";
            this.logistic_rbtn.Size = new System.Drawing.Size(61, 17);
            this.logistic_rbtn.TabIndex = 61;
            this.logistic_rbtn.TabStop = true;
            this.logistic_rbtn.Text = "Logistic";
            this.logistic_rbtn.UseVisualStyleBackColor = true;
            // 
            // linear_rbtn
            // 
            this.linear_rbtn.AutoSize = true;
            this.linear_rbtn.Location = new System.Drawing.Point(0, 11);
            this.linear_rbtn.Name = "linear_rbtn";
            this.linear_rbtn.Size = new System.Drawing.Size(54, 17);
            this.linear_rbtn.TabIndex = 60;
            this.linear_rbtn.TabStop = true;
            this.linear_rbtn.Text = "Linear";
            this.linear_rbtn.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(618, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 20);
            this.label7.TabIndex = 59;
            this.label7.Text = "Growth Type";
            // 
            // seasonality_grpbox
            // 
            this.seasonality_grpbox.Controls.Add(this.weekly_rbtn);
            this.seasonality_grpbox.Controls.Add(this.monthly_rbtn);
            this.seasonality_grpbox.Location = new System.Drawing.Point(65, 203);
            this.seasonality_grpbox.Name = "seasonality_grpbox";
            this.seasonality_grpbox.Size = new System.Drawing.Size(128, 59);
            this.seasonality_grpbox.TabIndex = 62;
            this.seasonality_grpbox.TabStop = false;
            // 
            // SeasonMode_grpbox
            // 
            this.SeasonMode_grpbox.Controls.Add(this.additive_rbtn);
            this.SeasonMode_grpbox.Controls.Add(this.multiplicative_rbtn);
            this.SeasonMode_grpbox.Location = new System.Drawing.Point(455, 199);
            this.SeasonMode_grpbox.Name = "SeasonMode_grpbox";
            this.SeasonMode_grpbox.Size = new System.Drawing.Size(128, 59);
            this.SeasonMode_grpbox.TabIndex = 63;
            this.SeasonMode_grpbox.TabStop = false;
            // 
            // Growth_grpbox
            // 
            this.Growth_grpbox.Controls.Add(this.linear_rbtn);
            this.Growth_grpbox.Controls.Add(this.logistic_rbtn);
            this.Growth_grpbox.Location = new System.Drawing.Point(646, 104);
            this.Growth_grpbox.Name = "Growth_grpbox";
            this.Growth_grpbox.Size = new System.Drawing.Size(86, 59);
            this.Growth_grpbox.TabIndex = 64;
            this.Growth_grpbox.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(622, 172);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 20);
            this.label8.TabIndex = 66;
            this.label8.Text = "Target Feature";
            // 
            // target_dropdown
            // 
            this.target_dropdown.FormattingEnabled = true;
            this.target_dropdown.Location = new System.Drawing.Point(646, 203);
            this.target_dropdown.Margin = new System.Windows.Forms.Padding(2);
            this.target_dropdown.Name = "target_dropdown";
            this.target_dropdown.Size = new System.Drawing.Size(134, 21);
            this.target_dropdown.TabIndex = 65;
            // 
            // Holiday_grpbox
            // 
            this.Holiday_grpbox.Controls.Add(this.Enable_rbtn);
            this.Holiday_grpbox.Controls.Add(this.Disable_rbtn);
            this.Holiday_grpbox.Location = new System.Drawing.Point(270, 104);
            this.Holiday_grpbox.Name = "Holiday_grpbox";
            this.Holiday_grpbox.Size = new System.Drawing.Size(86, 59);
            this.Holiday_grpbox.TabIndex = 67;
            this.Holiday_grpbox.TabStop = false;
            // 
            // Enable_rbtn
            // 
            this.Enable_rbtn.AutoSize = true;
            this.Enable_rbtn.Location = new System.Drawing.Point(0, 11);
            this.Enable_rbtn.Name = "Enable_rbtn";
            this.Enable_rbtn.Size = new System.Drawing.Size(58, 17);
            this.Enable_rbtn.TabIndex = 60;
            this.Enable_rbtn.TabStop = true;
            this.Enable_rbtn.Text = "Enable";
            this.Enable_rbtn.UseVisualStyleBackColor = true;
            // 
            // Standard_grpbox
            // 
            this.Standard_grpbox.Controls.Add(this.Enable_rbtn2);
            this.Standard_grpbox.Controls.Add(this.Disable_rbtn2);
            this.Standard_grpbox.Location = new System.Drawing.Point(455, 104);
            this.Standard_grpbox.Name = "Standard_grpbox";
            this.Standard_grpbox.Size = new System.Drawing.Size(86, 59);
            this.Standard_grpbox.TabIndex = 68;
            this.Standard_grpbox.TabStop = false;
            // 
            // Enable_rbtn2
            // 
            this.Enable_rbtn2.AutoSize = true;
            this.Enable_rbtn2.Location = new System.Drawing.Point(0, 11);
            this.Enable_rbtn2.Name = "Enable_rbtn2";
            this.Enable_rbtn2.Size = new System.Drawing.Size(58, 17);
            this.Enable_rbtn2.TabIndex = 60;
            this.Enable_rbtn2.TabStop = true;
            this.Enable_rbtn2.Text = "Enable";
            this.Enable_rbtn2.UseVisualStyleBackColor = true;
            // 
            // Disable_rbtn2
            // 
            this.Disable_rbtn2.AutoSize = true;
            this.Disable_rbtn2.Location = new System.Drawing.Point(0, 36);
            this.Disable_rbtn2.Name = "Disable_rbtn2";
            this.Disable_rbtn2.Size = new System.Drawing.Size(60, 17);
            this.Disable_rbtn2.TabIndex = 52;
            this.Disable_rbtn2.TabStop = true;
            this.Disable_rbtn2.Text = "Disable";
            this.Disable_rbtn2.UseVisualStyleBackColor = true;
            // 
            // ProphetModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 373);
            this.Controls.Add(this.Standard_grpbox);
            this.Controls.Add(this.Holiday_grpbox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.target_dropdown);
            this.Controls.Add(this.Growth_grpbox);
            this.Controls.Add(this.SeasonMode_grpbox);
            this.Controls.Add(this.seasonality_grpbox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.split_txtbox);
            this.Controls.Add(this.test_txtbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.train_txtbox);
            this.Controls.Add(this.period_txtbox);
            this.Controls.Add(this.train_btn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Hyperparameters);
            this.Name = "ProphetModel";
            this.Text = "Prophet";
            this.Load += new System.EventHandler(this.ProphetModel_load);
            this.seasonality_grpbox.ResumeLayout(false);
            this.seasonality_grpbox.PerformLayout();
            this.SeasonMode_grpbox.ResumeLayout(false);
            this.SeasonMode_grpbox.PerformLayout();
            this.Growth_grpbox.ResumeLayout(false);
            this.Growth_grpbox.PerformLayout();
            this.Holiday_grpbox.ResumeLayout(false);
            this.Holiday_grpbox.PerformLayout();
            this.Standard_grpbox.ResumeLayout(false);
            this.Standard_grpbox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox train_txtbox;
        private System.Windows.Forms.TextBox period_txtbox;
        private System.Windows.Forms.Button train_btn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Hyperparameters;
        private System.Windows.Forms.RadioButton weekly_rbtn;
        private System.Windows.Forms.RadioButton monthly_rbtn;
        private System.Windows.Forms.RadioButton Disable_rbtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox test_txtbox;
        private System.Windows.Forms.TextBox split_txtbox;
        private System.Windows.Forms.RadioButton multiplicative_rbtn;
        private System.Windows.Forms.RadioButton additive_rbtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton logistic_rbtn;
        private System.Windows.Forms.RadioButton linear_rbtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox seasonality_grpbox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox SeasonMode_grpbox;
        private System.Windows.Forms.GroupBox Growth_grpbox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox target_dropdown;
        private System.Windows.Forms.GroupBox Holiday_grpbox;
        private System.Windows.Forms.RadioButton Enable_rbtn;
        private System.Windows.Forms.GroupBox Standard_grpbox;
        private System.Windows.Forms.RadioButton Enable_rbtn2;
        private System.Windows.Forms.RadioButton Disable_rbtn2;
    }
}