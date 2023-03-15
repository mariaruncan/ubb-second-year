namespace Problema1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewBriose = new System.Windows.Forms.DataGridView();
            this.listBoxCofetarii = new System.Windows.Forms.ListBox();
            this.buttonCommit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBriose)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cofetarii";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(214, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Briose";
            // 
            // dataGridViewBriose
            // 
            this.dataGridViewBriose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBriose.Location = new System.Drawing.Point(214, 37);
            this.dataGridViewBriose.Name = "dataGridViewBriose";
            this.dataGridViewBriose.RowHeadersWidth = 51;
            this.dataGridViewBriose.RowTemplate.Height = 24;
            this.dataGridViewBriose.Size = new System.Drawing.Size(661, 197);
            this.dataGridViewBriose.TabIndex = 3;
            // 
            // listBoxCofetarii
            // 
            this.listBoxCofetarii.FormattingEnabled = true;
            this.listBoxCofetarii.ItemHeight = 16;
            this.listBoxCofetarii.Location = new System.Drawing.Point(12, 37);
            this.listBoxCofetarii.Name = "listBoxCofetarii";
            this.listBoxCofetarii.Size = new System.Drawing.Size(175, 196);
            this.listBoxCofetarii.TabIndex = 4;
            // 
            // buttonCommit
            // 
            this.buttonCommit.Location = new System.Drawing.Point(12, 261);
            this.buttonCommit.Name = "buttonCommit";
            this.buttonCommit.Size = new System.Drawing.Size(863, 35);
            this.buttonCommit.TabIndex = 5;
            this.buttonCommit.Text = "Commit";
            this.buttonCommit.UseVisualStyleBackColor = true;
            this.buttonCommit.Click += new System.EventHandler(this.buttonCommit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 308);
            this.Controls.Add(this.buttonCommit);
            this.Controls.Add(this.listBoxCofetarii);
            this.Controls.Add(this.dataGridViewBriose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBriose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridViewBriose;
        private System.Windows.Forms.ListBox listBoxCofetarii;
        private System.Windows.Forms.Button buttonCommit;
    }
}

