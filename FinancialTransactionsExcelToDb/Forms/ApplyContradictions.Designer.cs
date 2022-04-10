
namespace FinancialTransactionsExcelToDb.Forms
{
    partial class ApplyContradictions
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnAddRialInvoice = new System.Windows.Forms.Button();
            this.btnGetIds = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(226, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1000, 551);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "";
            // 
            // btnAddRialInvoice
            // 
            this.btnAddRialInvoice.Location = new System.Drawing.Point(12, 70);
            this.btnAddRialInvoice.Name = "btnAddRialInvoice";
            this.btnAddRialInvoice.Size = new System.Drawing.Size(205, 23);
            this.btnAddRialInvoice.TabIndex = 10;
            this.btnAddRialInvoice.Text = "CorrectWarehouseInventories";
            this.btnAddRialInvoice.UseVisualStyleBackColor = true;
            this.btnAddRialInvoice.Click += new System.EventHandler(this.btnAddRialInvoice_Click);
            // 
            // btnGetIds
            // 
            this.btnGetIds.Location = new System.Drawing.Point(12, 41);
            this.btnGetIds.Name = "btnGetIds";
            this.btnGetIds.Size = new System.Drawing.Size(205, 23);
            this.btnGetIds.TabIndex = 9;
            this.btnGetIds.Text = "GetStockTakingVariances";
            this.btnGetIds.UseVisualStyleBackColor = true;
            this.btnGetIds.Click += new System.EventHandler(this.btnGetIds_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 12);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(205, 23);
            this.btnLogin.TabIndex = 8;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(205, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "CorrectSerialWarehouseInventory";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ApplyContradictions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1263, 576);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnAddRialInvoice);
            this.Controls.Add(this.btnGetIds);
            this.Controls.Add(this.btnLogin);
            this.Name = "ApplyContradictions";
            this.Text = "ApplyContradictions";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnAddRialInvoice;
        private System.Windows.Forms.Button btnGetIds;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button button1;
    }
}