namespace FormsUI
{
    partial class MainForm
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
            this.EmailTabs = new System.Windows.Forms.TabControl();
            this.Account1 = new System.Windows.Forms.TabPage();
            this.accountControl1 = new EmailMemoryClass.AccountControl();
            this.Account2 = new System.Windows.Forms.TabPage();
            this.Account3 = new System.Windows.Forms.TabPage();
            this.EmailTabs.SuspendLayout();
            this.Account1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EmailTabs
            // 
            this.EmailTabs.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.EmailTabs.Controls.Add(this.Account1);
            this.EmailTabs.Controls.Add(this.Account2);
            this.EmailTabs.Controls.Add(this.Account3);
            this.EmailTabs.Location = new System.Drawing.Point(12, 12);
            this.EmailTabs.Name = "EmailTabs";
            this.EmailTabs.SelectedIndex = 0;
            this.EmailTabs.Size = new System.Drawing.Size(1005, 652);
            this.EmailTabs.TabIndex = 0;
            // 
            // Account1
            // 
            this.Account1.Controls.Add(this.accountControl1);
            this.Account1.Location = new System.Drawing.Point(4, 4);
            this.Account1.Name = "Account1";
            this.Account1.Padding = new System.Windows.Forms.Padding(3);
            this.Account1.Size = new System.Drawing.Size(997, 626);
            this.Account1.TabIndex = 0;
            this.Account1.Text = "CC_UK";
            this.Account1.UseVisualStyleBackColor = true;
            // 
            // accountControl1
            // 
            this.accountControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.accountControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accountControl1.Location = new System.Drawing.Point(3, 3);
            this.accountControl1.Name = "accountControl1";
            this.accountControl1.Size = new System.Drawing.Size(991, 620);
            this.accountControl1.TabIndex = 0;
            // 
            // Account2
            // 
            this.Account2.Location = new System.Drawing.Point(4, 4);
            this.Account2.Name = "Account2";
            this.Account2.Padding = new System.Windows.Forms.Padding(3);
            this.Account2.Size = new System.Drawing.Size(997, 626);
            this.Account2.TabIndex = 1;
            this.Account2.Text = "CC_IE";
            this.Account2.UseVisualStyleBackColor = true;
            // 
            // Account3
            // 
            this.Account3.Location = new System.Drawing.Point(4, 4);
            this.Account3.Name = "Account3";
            this.Account3.Size = new System.Drawing.Size(997, 626);
            this.Account3.TabIndex = 2;
            this.Account3.Text = "Personal";
            this.Account3.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 676);
            this.Controls.Add(this.EmailTabs);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.EmailTabs.ResumeLayout(false);
            this.Account1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl EmailTabs;
        private System.Windows.Forms.TabPage Account1;
        private System.Windows.Forms.TabPage Account2;
        private System.Windows.Forms.TabPage Account3;
        private EmailMemoryClass.AccountControl accountControl1;
    }
}

