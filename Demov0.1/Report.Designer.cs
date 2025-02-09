namespace Demov0._1
{
    partial class Report
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labeltotalPages = new System.Windows.Forms.Label();
            this.labelCurrentPage = new System.Windows.Forms.Label();
            this.labelPageSize = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(472, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "รายงาน";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(425, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "#";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(472, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 37);
            this.label3.TabIndex = 2;
            this.label3.Text = "วันที่ #";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 362);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(982, 693);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // labeltotalPages
            // 
            this.labeltotalPages.AutoSize = true;
            this.labeltotalPages.Location = new System.Drawing.Point(883, 23);
            this.labeltotalPages.Name = "labeltotalPages";
            this.labeltotalPages.Size = new System.Drawing.Size(28, 37);
            this.labeltotalPages.TabIndex = 4;
            this.labeltotalPages.Text = "#";
            // 
            // labelCurrentPage
            // 
            this.labelCurrentPage.AutoSize = true;
            this.labelCurrentPage.Location = new System.Drawing.Point(807, 23);
            this.labelCurrentPage.Name = "labelCurrentPage";
            this.labelCurrentPage.Size = new System.Drawing.Size(28, 37);
            this.labelCurrentPage.TabIndex = 5;
            this.labelCurrentPage.Text = "#";
            // 
            // labelPageSize
            // 
            this.labelPageSize.AutoSize = true;
            this.labelPageSize.Location = new System.Drawing.Point(950, 9);
            this.labelPageSize.Name = "labelPageSize";
            this.labelPageSize.Size = new System.Drawing.Size(0, 37);
            this.labelPageSize.TabIndex = 6;
            this.labelPageSize.Click += new System.EventHandler(this.labelPageSize_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 37);
            this.button1.TabIndex = 7;
            this.button1.Text = "พิมพ์";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labeltotalPages);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.labelCurrentPage);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(982, 134);
            this.panel2.TabIndex = 8;
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 36F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(982, 1055);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.labelPageSize);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("TH Sarabun New", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.Name = "Report";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form4";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Load += new System.EventHandler(this.Form4_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labeltotalPages;
        private System.Windows.Forms.Label labelCurrentPage;
        private System.Windows.Forms.Label labelPageSize;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
    }
}