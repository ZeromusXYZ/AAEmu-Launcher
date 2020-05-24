namespace AAEmu.Launcher
{
    partial class URIGenForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(URIGenForm));
            this.tServerIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tConfigName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tAuthDriver = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tWebsite = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tNews = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tPatchLocation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tCompiledURI = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.cbObfuscate = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tServerIP
            // 
            this.tServerIP.Location = new System.Drawing.Point(133, 6);
            this.tServerIP.Name = "tServerIP";
            this.tServerIP.Size = new System.Drawing.Size(137, 20);
            this.tServerIP.TabIndex = 0;
            this.tServerIP.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ServerIP or Domain";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Configuration Name";
            // 
            // tConfigName
            // 
            this.tConfigName.Location = new System.Drawing.Point(133, 32);
            this.tConfigName.Name = "tConfigName";
            this.tConfigName.Size = new System.Drawing.Size(137, 20);
            this.tConfigName.TabIndex = 2;
            this.tConfigName.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Login Username";
            // 
            // tUserName
            // 
            this.tUserName.Location = new System.Drawing.Point(133, 208);
            this.tUserName.Name = "tUserName";
            this.tUserName.Size = new System.Drawing.Size(137, 20);
            this.tUserName.TabIndex = 4;
            this.tUserName.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(280, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Password";
            // 
            // tPassword
            // 
            this.tPassword.Location = new System.Drawing.Point(338, 208);
            this.tPassword.Name = "tPassword";
            this.tPassword.Size = new System.Drawing.Size(137, 20);
            this.tPassword.TabIndex = 6;
            this.tPassword.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Authentication Driver";
            // 
            // tAuthDriver
            // 
            this.tAuthDriver.Location = new System.Drawing.Point(133, 77);
            this.tAuthDriver.Name = "tAuthDriver";
            this.tAuthDriver.Size = new System.Drawing.Size(137, 20);
            this.tAuthDriver.TabIndex = 8;
            this.tAuthDriver.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Website URL";
            // 
            // tWebsite
            // 
            this.tWebsite.Location = new System.Drawing.Point(133, 103);
            this.tWebsite.Name = "tWebsite";
            this.tWebsite.Size = new System.Drawing.Size(342, 20);
            this.tWebsite.TabIndex = 10;
            this.tWebsite.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Newsfeed URL";
            // 
            // tNews
            // 
            this.tNews.Location = new System.Drawing.Point(133, 129);
            this.tNews.Name = "tNews";
            this.tNews.Size = new System.Drawing.Size(342, 20);
            this.tNews.TabIndex = 12;
            this.tNews.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 158);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Patch Folder Base URL";
            // 
            // tPatchLocation
            // 
            this.tPatchLocation.Location = new System.Drawing.Point(133, 155);
            this.tPatchLocation.Name = "tPatchLocation";
            this.tPatchLocation.Size = new System.Drawing.Size(342, 20);
            this.tPatchLocation.TabIndex = 14;
            this.tPatchLocation.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 287);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "URI";
            // 
            // tCompiledURI
            // 
            this.tCompiledURI.Location = new System.Drawing.Point(34, 284);
            this.tCompiledURI.Name = "tCompiledURI";
            this.tCompiledURI.Size = new System.Drawing.Size(414, 20);
            this.tCompiledURI.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(454, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // cbObfuscate
            // 
            this.cbObfuscate.AutoSize = true;
            this.cbObfuscate.Checked = true;
            this.cbObfuscate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbObfuscate.Location = new System.Drawing.Point(34, 261);
            this.cbObfuscate.Name = "cbObfuscate";
            this.cbObfuscate.Size = new System.Drawing.Size(75, 17);
            this.cbObfuscate.TabIndex = 19;
            this.cbObfuscate.Text = "Obfuscate";
            this.cbObfuscate.UseVisualStyleBackColor = true;
            this.cbObfuscate.CheckedChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 188);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(398, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "(*) It is NOT recommended to include the username, and certainly not the password" +
    "";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(454, 284);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 21;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // URIGenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 316);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbObfuscate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tCompiledURI);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tPatchLocation);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tNews);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tWebsite);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tAuthDriver);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tConfigName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tServerIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "URIGenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "URI Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tCompiledURI;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.TextBox tServerIP;
        public System.Windows.Forms.TextBox tConfigName;
        public System.Windows.Forms.TextBox tUserName;
        public System.Windows.Forms.TextBox tPassword;
        public System.Windows.Forms.TextBox tAuthDriver;
        public System.Windows.Forms.TextBox tWebsite;
        public System.Windows.Forms.TextBox tNews;
        public System.Windows.Forms.TextBox tPatchLocation;
        private System.Windows.Forms.CheckBox cbObfuscate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnCopy;
    }
}