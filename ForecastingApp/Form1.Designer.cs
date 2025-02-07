namespace ForecastingApp
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Uploadbtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelDateset = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.labelDateset);
            this.splitContainer1.Panel1.Controls.Add(this.Uploadbtn);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1067, 724);
            this.splitContainer1.SplitterDistance = 301;
            this.splitContainer1.TabIndex = 0;
            // 
            // Uploadbtn
            // 
            this.Uploadbtn.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Uploadbtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Uploadbtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Uploadbtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Uploadbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Uploadbtn.Location = new System.Drawing.Point(92, 104);
            this.Uploadbtn.Margin = new System.Windows.Forms.Padding(2);
            this.Uploadbtn.Name = "Uploadbtn";
            this.Uploadbtn.Size = new System.Drawing.Size(111, 41);
            this.Uploadbtn.TabIndex = 0;
            this.Uploadbtn.Text = "Upload Dataset";
            this.Uploadbtn.UseVisualStyleBackColor = false;
            this.Uploadbtn.Click += new System.EventHandler(this.Uploadbtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Processed Information";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(39, 181);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(684, 511);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // labelDateset
            // 
            this.labelDateset.AutoSize = true;
            this.labelDateset.Location = new System.Drawing.Point(24, 23);
            this.labelDateset.Name = "labelDateset";
            this.labelDateset.Size = new System.Drawing.Size(68, 13);
            this.labelDateset.TabIndex = 1;
            this.labelDateset.Text = "Dataset Info:";
            this.labelDateset.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Data...";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(27, 53);
            this.label2.MaximumSize = new System.Drawing.Size(240, 600);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(240, 35);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 724);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button Uploadbtn;
        private System.Windows.Forms.Label labelDateset;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}

