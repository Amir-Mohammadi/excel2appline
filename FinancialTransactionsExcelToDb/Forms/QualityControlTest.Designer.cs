
namespace FinancialTransactionsExcelToDb.Forms
{
    partial class QualityControlTest
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
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnInsert2 = new System.Windows.Forms.Button();
            this.btnAddTestCondition = new System.Windows.Forms.Button();
            this.btnSaveStuffQualityControlTests = new System.Windows.Forms.Button();
            this.btnLinkStuffQcTestToTestCondition = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(123, 43);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(98, 23);
            this.btnInsert.TabIndex = 14;
            this.btnInsert.Text = "AddQcTests";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 14);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 13;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 72);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(696, 337);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(93, 14);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadFile.TabIndex = 11;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click_1);
            // 
            // btnInsert2
            // 
            this.btnInsert2.Location = new System.Drawing.Point(227, 43);
            this.btnInsert2.Name = "btnInsert2";
            this.btnInsert2.Size = new System.Drawing.Size(173, 23);
            this.btnInsert2.TabIndex = 15;
            this.btnInsert2.Text = "LinkQcTestsToTestConditions";
            this.btnInsert2.UseVisualStyleBackColor = true;
            this.btnInsert2.Click += new System.EventHandler(this.btnInsert2_Click);
            // 
            // btnAddTestCondition
            // 
            this.btnAddTestCondition.Location = new System.Drawing.Point(12, 43);
            this.btnAddTestCondition.Name = "btnAddTestCondition";
            this.btnAddTestCondition.Size = new System.Drawing.Size(105, 23);
            this.btnAddTestCondition.TabIndex = 17;
            this.btnAddTestCondition.Text = "AddTestConditions";
            this.btnAddTestCondition.UseVisualStyleBackColor = true;
            this.btnAddTestCondition.Click += new System.EventHandler(this.btnAddTestCondition_Click);
            // 
            // btnSaveStuffQualityControlTests
            // 
            this.btnSaveStuffQualityControlTests.Location = new System.Drawing.Point(406, 43);
            this.btnSaveStuffQualityControlTests.Name = "btnSaveStuffQualityControlTests";
            this.btnSaveStuffQualityControlTests.Size = new System.Drawing.Size(105, 23);
            this.btnSaveStuffQualityControlTests.TabIndex = 18;
            this.btnSaveStuffQualityControlTests.Text = "AddStuffQcTests";
            this.btnSaveStuffQualityControlTests.UseVisualStyleBackColor = true;
            this.btnSaveStuffQualityControlTests.Click += new System.EventHandler(this.btnSaveStuffQualityControlTests_Click);
            // 
            // btnLinkStuffQcTestToTestCondition
            // 
            this.btnLinkStuffQcTestToTestCondition.Location = new System.Drawing.Point(517, 43);
            this.btnLinkStuffQcTestToTestCondition.Name = "btnLinkStuffQcTestToTestCondition";
            this.btnLinkStuffQcTestToTestCondition.Size = new System.Drawing.Size(191, 23);
            this.btnLinkStuffQcTestToTestCondition.TabIndex = 19;
            this.btnLinkStuffQcTestToTestCondition.Text = "LinkStuffQcTestToTestCondition";
            this.btnLinkStuffQcTestToTestCondition.UseVisualStyleBackColor = true;
            this.btnLinkStuffQcTestToTestCondition.Click += new System.EventHandler(this.btnLinkStuffQcTestToTestCondition_Click);
            // 
            // QualityControlTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 427);
            this.Controls.Add(this.btnLinkStuffQcTestToTestCondition);
            this.Controls.Add(this.btnSaveStuffQualityControlTests);
            this.Controls.Add(this.btnAddTestCondition);
            this.Controls.Add(this.btnInsert2);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnLoadFile);
            this.Name = "QualityControlTest";
            this.Text = "QualityControlTest";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.QualityControlTest_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnInsert2;
        private System.Windows.Forms.Button btnAddTestCondition;
        private System.Windows.Forms.Button btnSaveStuffQualityControlTests;
        private System.Windows.Forms.Button btnLinkStuffQcTestToTestCondition;
    }
}