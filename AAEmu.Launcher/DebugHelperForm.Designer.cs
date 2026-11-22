namespace AAEmu.Launcher
{
    partial class DebugHelperForm
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
            this.eArgs = new System.Windows.Forms.TextBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.eHackShieldArg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.eExe = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.eVerb = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lUpdatedConfig = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // eArgs
            // 
            this.eArgs.Location = new System.Drawing.Point(12, 68);
            this.eArgs.Name = "eArgs";
            this.eArgs.Size = new System.Drawing.Size(457, 20);
            this.eArgs.TabIndex = 0;
            // 
            // btnContinue
            // 
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContinue.Location = new System.Drawing.Point(394, 182);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 1;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            // 
            // eHackShieldArg
            // 
            this.eHackShieldArg.Location = new System.Drawing.Point(12, 154);
            this.eHackShieldArg.Name = "eHackShieldArg";
            this.eHackShieldArg.Size = new System.Drawing.Size(142, 20);
            this.eHackShieldArg.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "All Parameters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Disable HShield parameter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Executeable";
            // 
            // eExe
            // 
            this.eExe.Location = new System.Drawing.Point(12, 25);
            this.eExe.Name = "eExe";
            this.eExe.Size = new System.Drawing.Size(457, 20);
            this.eExe.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Launch Verb";
            // 
            // eVerb
            // 
            this.eVerb.Location = new System.Drawing.Point(178, 154);
            this.eVerb.Name = "eVerb";
            this.eVerb.Size = new System.Drawing.Size(142, 20);
            this.eVerb.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Detected system.cfg location";
            // 
            // lUpdatedConfig
            // 
            this.lUpdatedConfig.AutoSize = true;
            this.lUpdatedConfig.Location = new System.Drawing.Point(24, 113);
            this.lUpdatedConfig.Name = "lUpdatedConfig";
            this.lUpdatedConfig.Size = new System.Drawing.Size(76, 13);
            this.lUpdatedConfig.TabIndex = 10;
            this.lUpdatedConfig.Text = "<not updated>";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 182);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // DebugHelperForm
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 217);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lUpdatedConfig);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.eVerb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.eExe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.eHackShieldArg);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.eArgs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DebugHelperForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DebugHelperForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox eArgs;
        private System.Windows.Forms.Button btnContinue;
        public System.Windows.Forms.TextBox eHackShieldArg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox eExe;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox eVerb;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label lUpdatedConfig;
        private System.Windows.Forms.Button btnCancel;
    }
}