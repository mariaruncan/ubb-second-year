namespace Seminar_2_226_SGBD
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
            this.labelParent = new System.Windows.Forms.Label();
            this.labelChild = new System.Windows.Forms.Label();
            this.dataGridViewParent = new System.Windows.Forms.DataGridView();
            this.dataGridViewChild = new System.Windows.Forms.DataGridView();
            this.labelNumeProdus = new System.Windows.Forms.Label();
            this.labelTipProdus = new System.Windows.Forms.Label();
            this.labelVolumProdus = new System.Windows.Forms.Label();
            this.labelPretProdus = new System.Windows.Forms.Label();
            this.labelIdProducatorProdus = new System.Windows.Forms.Label();
            this.labelIdProdus = new System.Windows.Forms.Label();
            this.textBoxNume = new System.Windows.Forms.TextBox();
            this.textBoxIdProducator = new System.Windows.Forms.TextBox();
            this.textBoxPret = new System.Windows.Forms.TextBox();
            this.textBoxVolum = new System.Windows.Forms.TextBox();
            this.textBoxTip = new System.Windows.Forms.TextBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonShowProducatori = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChild)).BeginInit();
            this.SuspendLayout();
            // 
            // labelParent
            // 
            this.labelParent.AutoSize = true;
            this.labelParent.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelParent.Location = new System.Drawing.Point(24, 11);
            this.labelParent.Name = "labelParent";
            this.labelParent.Size = new System.Drawing.Size(227, 29);
            this.labelParent.TabIndex = 0;
            this.labelParent.Text = "Producatori - Parent";
            // 
            // labelChild
            // 
            this.labelChild.AutoSize = true;
            this.labelChild.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChild.Location = new System.Drawing.Point(24, 289);
            this.labelChild.Name = "labelChild";
            this.labelChild.Size = new System.Drawing.Size(180, 29);
            this.labelChild.TabIndex = 1;
            this.labelChild.Text = "Produse - Child";
            // 
            // dataGridViewParent
            // 
            this.dataGridViewParent.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewParent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewParent.Location = new System.Drawing.Point(29, 43);
            this.dataGridViewParent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewParent.Name = "dataGridViewParent";
            this.dataGridViewParent.RowHeadersWidth = 51;
            this.dataGridViewParent.RowTemplate.Height = 24;
            this.dataGridViewParent.Size = new System.Drawing.Size(609, 235);
            this.dataGridViewParent.TabIndex = 2;
            this.dataGridViewParent.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewParent_CellClick);
            // 
            // dataGridViewChild
            // 
            this.dataGridViewChild.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewChild.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewChild.Location = new System.Drawing.Point(29, 321);
            this.dataGridViewChild.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewChild.Name = "dataGridViewChild";
            this.dataGridViewChild.RowHeadersWidth = 51;
            this.dataGridViewChild.RowTemplate.Height = 24;
            this.dataGridViewChild.Size = new System.Drawing.Size(899, 235);
            this.dataGridViewChild.TabIndex = 3;
            this.dataGridViewChild.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewChild_CellClick);
            // 
            // labelNumeProdus
            // 
            this.labelNumeProdus.AutoSize = true;
            this.labelNumeProdus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumeProdus.Location = new System.Drawing.Point(975, 87);
            this.labelNumeProdus.Name = "labelNumeProdus";
            this.labelNumeProdus.Size = new System.Drawing.Size(67, 24);
            this.labelNumeProdus.TabIndex = 4;
            this.labelNumeProdus.Text = "Nume:";
            // 
            // labelTipProdus
            // 
            this.labelTipProdus.AutoSize = true;
            this.labelTipProdus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTipProdus.Location = new System.Drawing.Point(991, 114);
            this.labelTipProdus.Name = "labelTipProdus";
            this.labelTipProdus.Size = new System.Drawing.Size(42, 24);
            this.labelTipProdus.TabIndex = 5;
            this.labelTipProdus.Text = "Tip:";
            // 
            // labelVolumProdus
            // 
            this.labelVolumProdus.AutoSize = true;
            this.labelVolumProdus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVolumProdus.Location = new System.Drawing.Point(977, 143);
            this.labelVolumProdus.Name = "labelVolumProdus";
            this.labelVolumProdus.Size = new System.Drawing.Size(70, 24);
            this.labelVolumProdus.TabIndex = 6;
            this.labelVolumProdus.Text = "Volum:";
            // 
            // labelPretProdus
            // 
            this.labelPretProdus.AutoSize = true;
            this.labelPretProdus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPretProdus.Location = new System.Drawing.Point(993, 171);
            this.labelPretProdus.Name = "labelPretProdus";
            this.labelPretProdus.Size = new System.Drawing.Size(48, 24);
            this.labelPretProdus.TabIndex = 7;
            this.labelPretProdus.Text = "Pret:";
            // 
            // labelIdProducatorProdus
            // 
            this.labelIdProducatorProdus.AutoSize = true;
            this.labelIdProducatorProdus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIdProducatorProdus.Location = new System.Drawing.Point(933, 202);
            this.labelIdProducatorProdus.Name = "labelIdProducatorProdus";
            this.labelIdProducatorProdus.Size = new System.Drawing.Size(122, 24);
            this.labelIdProducatorProdus.TabIndex = 8;
            this.labelIdProducatorProdus.Text = "IdProducator:";
            // 
            // labelIdProdus
            // 
            this.labelIdProdus.AutoSize = true;
            this.labelIdProdus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIdProdus.Location = new System.Drawing.Point(983, 55);
            this.labelIdProdus.Name = "labelIdProdus";
            this.labelIdProdus.Size = new System.Drawing.Size(0, 24);
            this.labelIdProdus.TabIndex = 9;
            // 
            // textBoxNume
            // 
            this.textBoxNume.Location = new System.Drawing.Point(1075, 85);
            this.textBoxNume.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxNume.Name = "textBoxNume";
            this.textBoxNume.Size = new System.Drawing.Size(239, 22);
            this.textBoxNume.TabIndex = 10;
            // 
            // textBoxIdProducator
            // 
            this.textBoxIdProducator.Location = new System.Drawing.Point(1075, 197);
            this.textBoxIdProducator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxIdProducator.Name = "textBoxIdProducator";
            this.textBoxIdProducator.Size = new System.Drawing.Size(239, 22);
            this.textBoxIdProducator.TabIndex = 11;
            // 
            // textBoxPret
            // 
            this.textBoxPret.Location = new System.Drawing.Point(1075, 169);
            this.textBoxPret.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxPret.Name = "textBoxPret";
            this.textBoxPret.Size = new System.Drawing.Size(239, 22);
            this.textBoxPret.TabIndex = 12;
            // 
            // textBoxVolum
            // 
            this.textBoxVolum.Location = new System.Drawing.Point(1075, 140);
            this.textBoxVolum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxVolum.Name = "textBoxVolum";
            this.textBoxVolum.Size = new System.Drawing.Size(239, 22);
            this.textBoxVolum.TabIndex = 13;
            // 
            // textBoxTip
            // 
            this.textBoxTip.Location = new System.Drawing.Point(1075, 112);
            this.textBoxTip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxTip.Name = "textBoxTip";
            this.textBoxTip.Size = new System.Drawing.Size(239, 22);
            this.textBoxTip.TabIndex = 14;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Enabled = false;
            this.buttonUpdate.Location = new System.Drawing.Point(724, 100);
            this.buttonUpdate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(204, 37);
            this.buttonUpdate.TabIndex = 15;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(724, 153);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(204, 37);
            this.buttonDelete.TabIndex = 16;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Enabled = false;
            this.buttonAdd.Location = new System.Drawing.Point(724, 202);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(204, 37);
            this.buttonAdd.TabIndex = 17;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonShowProducatori
            // 
            this.buttonShowProducatori.Location = new System.Drawing.Point(724, 52);
            this.buttonShowProducatori.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonShowProducatori.Name = "buttonShowProducatori";
            this.buttonShowProducatori.Size = new System.Drawing.Size(204, 37);
            this.buttonShowProducatori.TabIndex = 18;
            this.buttonShowProducatori.Text = "Show Producatori";
            this.buttonShowProducatori.UseVisualStyleBackColor = true;
            this.buttonShowProducatori.Click += new System.EventHandler(this.buttonShowProducatori_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1333, 564);
            this.Controls.Add(this.buttonShowProducatori);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.textBoxTip);
            this.Controls.Add(this.textBoxVolum);
            this.Controls.Add(this.textBoxPret);
            this.Controls.Add(this.textBoxIdProducator);
            this.Controls.Add(this.textBoxNume);
            this.Controls.Add(this.labelIdProdus);
            this.Controls.Add(this.labelIdProducatorProdus);
            this.Controls.Add(this.labelPretProdus);
            this.Controls.Add(this.labelVolumProdus);
            this.Controls.Add(this.labelTipProdus);
            this.Controls.Add(this.labelNumeProdus);
            this.Controls.Add(this.dataGridViewChild);
            this.Controls.Add(this.dataGridViewParent);
            this.Controls.Add(this.labelChild);
            this.Controls.Add(this.labelParent);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Tema lab 1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewParent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewChild)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelParent;
        private System.Windows.Forms.Label labelChild;
        private System.Windows.Forms.DataGridView dataGridViewParent;
        private System.Windows.Forms.DataGridView dataGridViewChild;
        private System.Windows.Forms.Label labelNumeProdus;
        private System.Windows.Forms.Label labelTipProdus;
        private System.Windows.Forms.Label labelVolumProdus;
        private System.Windows.Forms.Label labelPretProdus;
        private System.Windows.Forms.Label labelIdProducatorProdus;
        private System.Windows.Forms.Label labelIdProdus;
        private System.Windows.Forms.TextBox textBoxNume;
        private System.Windows.Forms.TextBox textBoxIdProducator;
        private System.Windows.Forms.TextBox textBoxPret;
        private System.Windows.Forms.TextBox textBoxVolum;
        private System.Windows.Forms.TextBox textBoxTip;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonShowProducatori;
    }
}

