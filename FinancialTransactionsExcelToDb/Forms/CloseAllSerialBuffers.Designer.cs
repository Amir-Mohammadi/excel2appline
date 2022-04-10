
namespace FinancialTransactionsExcelToDb.Forms
{
    partial class CloseAllSerialBuffers
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
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(144, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1015, 551);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            // 
            // btnAddRialInvoice
            // 
            this.btnAddRialInvoice.Location = new System.Drawing.Point(12, 96);
            this.btnAddRialInvoice.Name = "btnAddRialInvoice";
            this.btnAddRialInvoice.Size = new System.Drawing.Size(126, 23);
            this.btnAddRialInvoice.TabIndex = 6;
            this.btnAddRialInvoice.Text = "CloseAll";
            this.btnAddRialInvoice.UseVisualStyleBackColor = true;
            this.btnAddRialInvoice.Click += new System.EventHandler(this.btnAddRialInvoice_Click);
            // 
            // btnGetIds
            // 
            this.btnGetIds.Location = new System.Drawing.Point(12, 54);
            this.btnGetIds.Name = "btnGetIds";
            this.btnGetIds.Size = new System.Drawing.Size(126, 23);
            this.btnGetIds.TabIndex = 5;
            this.btnGetIds.Text = "GetSerialBuffers";
            this.btnGetIds.UseVisualStyleBackColor = true;
            this.btnGetIds.Click += new System.EventHandler(this.btnGetIds_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 12);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(126, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // CloseAllSerialBuffers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 578);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnAddRialInvoice);
            this.Controls.Add(this.btnGetIds);
            this.Controls.Add(this.btnLogin);
            this.Name = "CloseAllSerialBuffers";
            this.Text = "CloseAllSerialBuffers";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnAddRialInvoice;
        private System.Windows.Forms.Button btnGetIds;
        private System.Windows.Forms.Button btnLogin;
    }
}