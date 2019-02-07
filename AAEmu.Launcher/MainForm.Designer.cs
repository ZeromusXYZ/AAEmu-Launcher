namespace AAEmu.Launcher
{
    partial class LauncherForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.LblLogin = new System.Windows.Forms.Label();
            this.LblPassword = new System.Windows.Forms.Label();
            this.PicButEnter = new System.Windows.Forms.PictureBox();
            this.PicButLangChange = new System.Windows.Forms.PictureBox();
            this.PicButSetting = new System.Windows.Forms.PictureBox();
            this.PicButExit = new System.Windows.Forms.PictureBox();
            this.PicButGithub = new System.Windows.Forms.PictureBox();
            this.PicButDiscord = new System.Windows.Forms.PictureBox();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.ButSettingCancel = new System.Windows.Forms.Button();
            this.ButSettingSave = new System.Windows.Forms.Button();
            this.cbHideSplashLogo = new System.Windows.Forms.CheckBox();
            this.cbSkipIntro = new System.Windows.Forms.CheckBox();
            this.ButPathToGame = new System.Windows.Forms.Button();
            this.txtPathToGame = new System.Windows.Forms.TextBox();
            this.cbSaveLogin = new System.Windows.Forms.CheckBox();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.LblPathToGame = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LblIPAddress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicButEnter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButLangChange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButGithub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButDiscord)).BeginInit();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(617, 191);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(329, 285);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Url = new System.Uri("https://aaemu.pw/updater/", System.UriKind.Absolute);
            // 
            // txtLogin
            // 
            this.txtLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.txtLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogin.ForeColor = System.Drawing.Color.White;
            this.txtLogin.Location = new System.Drawing.Point(688, 162);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(71, 21);
            this.txtLogin.TabIndex = 1;
            this.txtLogin.Text = "Test";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.Location = new System.Drawing.Point(855, 162);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(89, 21);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "test";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(1, 487);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(958, 2);
            this.progressBar1.TabIndex = 3;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(1, 484);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(958, 2);
            this.progressBar2.TabIndex = 4;
            // 
            // LblLogin
            // 
            this.LblLogin.BackColor = System.Drawing.Color.Transparent;
            this.LblLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblLogin.ForeColor = System.Drawing.Color.White;
            this.LblLogin.Location = new System.Drawing.Point(548, 162);
            this.LblLogin.Name = "LblLogin";
            this.LblLogin.Size = new System.Drawing.Size(134, 16);
            this.LblLogin.TabIndex = 5;
            this.LblLogin.Text = "Логин:";
            this.LblLogin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblPassword
            // 
            this.LblPassword.BackColor = System.Drawing.Color.Transparent;
            this.LblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblPassword.ForeColor = System.Drawing.Color.White;
            this.LblPassword.Location = new System.Drawing.Point(765, 162);
            this.LblPassword.Name = "LblPassword";
            this.LblPassword.Size = new System.Drawing.Size(83, 16);
            this.LblPassword.TabIndex = 6;
            this.LblPassword.Text = "Пароль:";
            this.LblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PicButEnter
            // 
            this.PicButEnter.BackColor = System.Drawing.Color.Transparent;
            this.PicButEnter.BackgroundImage = global::AAEmu.Launcher.Properties.Resources.Logo;
            this.PicButEnter.Location = new System.Drawing.Point(178, 209);
            this.PicButEnter.Name = "PicButEnter";
            this.PicButEnter.Size = new System.Drawing.Size(205, 133);
            this.PicButEnter.TabIndex = 7;
            this.PicButEnter.TabStop = false;
            this.PicButEnter.Click += new System.EventHandler(this.PicButEnter_Click);
            this.PicButEnter.MouseEnter += new System.EventHandler(this.PicButEnter_MouseEnter);
            this.PicButEnter.MouseLeave += new System.EventHandler(this.PicButEnter_MouseLeave);
            // 
            // PicButLangChange
            // 
            this.PicButLangChange.BackColor = System.Drawing.Color.Transparent;
            this.PicButLangChange.Image = global::AAEmu.Launcher.Properties.Resources.But_Lang_Ru;
            this.PicButLangChange.Location = new System.Drawing.Point(808, 115);
            this.PicButLangChange.Name = "PicButLangChange";
            this.PicButLangChange.Size = new System.Drawing.Size(40, 40);
            this.PicButLangChange.TabIndex = 8;
            this.PicButLangChange.TabStop = false;
            this.PicButLangChange.Click += new System.EventHandler(this.PicButLangChange_Click);
            // 
            // PicButSetting
            // 
            this.PicButSetting.BackColor = System.Drawing.Color.Transparent;
            this.PicButSetting.Image = global::AAEmu.Launcher.Properties.Resources.But_Settings;
            this.PicButSetting.Location = new System.Drawing.Point(855, 115);
            this.PicButSetting.Name = "PicButSetting";
            this.PicButSetting.Size = new System.Drawing.Size(40, 40);
            this.PicButSetting.TabIndex = 9;
            this.PicButSetting.TabStop = false;
            this.PicButSetting.Click += new System.EventHandler(this.PicButSetting_Click);
            this.PicButSetting.MouseEnter += new System.EventHandler(this.PicButSetting_MouseEnter);
            this.PicButSetting.MouseLeave += new System.EventHandler(this.PicButSetting_MouseLeave);
            // 
            // PicButExit
            // 
            this.PicButExit.BackColor = System.Drawing.Color.Transparent;
            this.PicButExit.Image = global::AAEmu.Launcher.Properties.Resources.But_Power;
            this.PicButExit.Location = new System.Drawing.Point(902, 115);
            this.PicButExit.Name = "PicButExit";
            this.PicButExit.Size = new System.Drawing.Size(40, 40);
            this.PicButExit.TabIndex = 10;
            this.PicButExit.TabStop = false;
            this.PicButExit.Click += new System.EventHandler(this.pictureBox4_Click);
            this.PicButExit.MouseEnter += new System.EventHandler(this.PicButExit_MouseEnter);
            this.PicButExit.MouseLeave += new System.EventHandler(this.PicButExit_MouseLeave);
            // 
            // PicButGithub
            // 
            this.PicButGithub.BackColor = System.Drawing.Color.Transparent;
            this.PicButGithub.Image = ((System.Drawing.Image)(resources.GetObject("PicButGithub.Image")));
            this.PicButGithub.Location = new System.Drawing.Point(12, 154);
            this.PicButGithub.Name = "PicButGithub";
            this.PicButGithub.Size = new System.Drawing.Size(40, 40);
            this.PicButGithub.TabIndex = 11;
            this.PicButGithub.TabStop = false;
            this.PicButGithub.Click += new System.EventHandler(this.PicButGithub_Click);
            this.PicButGithub.MouseEnter += new System.EventHandler(this.PicButGithub_MouseEnter);
            this.PicButGithub.MouseLeave += new System.EventHandler(this.PicButGithub_MouseLeave);
            // 
            // PicButDiscord
            // 
            this.PicButDiscord.BackColor = System.Drawing.Color.Transparent;
            this.PicButDiscord.Image = global::AAEmu.Launcher.Properties.Resources.Discord_Logo_Only;
            this.PicButDiscord.Location = new System.Drawing.Point(58, 154);
            this.PicButDiscord.Name = "PicButDiscord";
            this.PicButDiscord.Size = new System.Drawing.Size(40, 40);
            this.PicButDiscord.TabIndex = 12;
            this.PicButDiscord.TabStop = false;
            this.PicButDiscord.Click += new System.EventHandler(this.PicButDiscord_Click);
            this.PicButDiscord.MouseEnter += new System.EventHandler(this.PicButDiscord_MouseEnter);
            this.PicButDiscord.MouseLeave += new System.EventHandler(this.PicButDiscord_MouseLeave);
            // 
            // gbSettings
            // 
            this.gbSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.gbSettings.Controls.Add(this.ButSettingCancel);
            this.gbSettings.Controls.Add(this.ButSettingSave);
            this.gbSettings.Controls.Add(this.cbHideSplashLogo);
            this.gbSettings.Controls.Add(this.cbSkipIntro);
            this.gbSettings.Controls.Add(this.ButPathToGame);
            this.gbSettings.Controls.Add(this.txtPathToGame);
            this.gbSettings.Controls.Add(this.cbSaveLogin);
            this.gbSettings.Controls.Add(this.txtServerIP);
            this.gbSettings.Controls.Add(this.LblPathToGame);
            this.gbSettings.Controls.Add(this.label2);
            this.gbSettings.Controls.Add(this.LblIPAddress);
            this.gbSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbSettings.ForeColor = System.Drawing.Color.White;
            this.gbSettings.Location = new System.Drawing.Point(12, 209);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(599, 170);
            this.gbSettings.TabIndex = 13;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Настройки";
            this.gbSettings.Visible = false;
            // 
            // ButSettingCancel
            // 
            this.ButSettingCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.ButSettingCancel.Location = new System.Drawing.Point(329, 138);
            this.ButSettingCancel.Name = "ButSettingCancel";
            this.ButSettingCancel.Size = new System.Drawing.Size(81, 23);
            this.ButSettingCancel.TabIndex = 13;
            this.ButSettingCancel.Text = "Отмена";
            this.ButSettingCancel.UseVisualStyleBackColor = true;
            this.ButSettingCancel.Click += new System.EventHandler(this.ButSettingCancel_Click);
            // 
            // ButSettingSave
            // 
            this.ButSettingSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.ButSettingSave.Location = new System.Drawing.Point(241, 138);
            this.ButSettingSave.Name = "ButSettingSave";
            this.ButSettingSave.Size = new System.Drawing.Size(81, 23);
            this.ButSettingSave.TabIndex = 12;
            this.ButSettingSave.Text = "Сохранить";
            this.ButSettingSave.UseVisualStyleBackColor = true;
            this.ButSettingSave.Click += new System.EventHandler(this.ButSettingSave_Click);
            // 
            // cbHideSplashLogo
            // 
            this.cbHideSplashLogo.AutoSize = true;
            this.cbHideSplashLogo.Location = new System.Drawing.Point(222, 114);
            this.cbHideSplashLogo.Name = "cbHideSplashLogo";
            this.cbHideSplashLogo.Size = new System.Drawing.Size(254, 20);
            this.cbHideSplashLogo.TabIndex = 11;
            this.cbHideSplashLogo.Text = "Показывать логотип загрузки";
            this.cbHideSplashLogo.UseVisualStyleBackColor = true;
            // 
            // cbSkipIntro
            // 
            this.cbSkipIntro.AutoSize = true;
            this.cbSkipIntro.Location = new System.Drawing.Point(222, 91);
            this.cbSkipIntro.Name = "cbSkipIntro";
            this.cbSkipIntro.Size = new System.Drawing.Size(188, 20);
            this.cbSkipIntro.TabIndex = 10;
            this.cbSkipIntro.Text = "Пропустить заставку";
            this.cbSkipIntro.UseVisualStyleBackColor = true;
            // 
            // ButPathToGame
            // 
            this.ButPathToGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.ButPathToGame.Location = new System.Drawing.Point(552, 41);
            this.ButPathToGame.Name = "ButPathToGame";
            this.ButPathToGame.Size = new System.Drawing.Size(28, 23);
            this.ButPathToGame.TabIndex = 8;
            this.ButPathToGame.Text = "...";
            this.ButPathToGame.UseVisualStyleBackColor = true;
            this.ButPathToGame.Click += new System.EventHandler(this.ButPathToGame_Click);
            // 
            // txtPathToGame
            // 
            this.txtPathToGame.Enabled = false;
            this.txtPathToGame.Location = new System.Drawing.Point(223, 42);
            this.txtPathToGame.Name = "txtPathToGame";
            this.txtPathToGame.Size = new System.Drawing.Size(323, 21);
            this.txtPathToGame.TabIndex = 7;
            this.txtPathToGame.Text = "c:\\ArcheAge\\bin32\\Archeage.exe";
            // 
            // cbSaveLogin
            // 
            this.cbSaveLogin.AutoSize = true;
            this.cbSaveLogin.Location = new System.Drawing.Point(222, 68);
            this.cbSaveLogin.Name = "cbSaveLogin";
            this.cbSaveLogin.Size = new System.Drawing.Size(230, 20);
            this.cbSaveLogin.TabIndex = 6;
            this.cbSaveLogin.Text = "Сохранять учетные данные";
            this.cbSaveLogin.UseVisualStyleBackColor = true;
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(223, 14);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(100, 21);
            this.txtServerIP.TabIndex = 5;
            this.txtServerIP.Text = "127.0.0.1";
            // 
            // LblPathToGame
            // 
            this.LblPathToGame.Location = new System.Drawing.Point(6, 45);
            this.LblPathToGame.Name = "LblPathToGame";
            this.LblPathToGame.Size = new System.Drawing.Size(211, 16);
            this.LblPathToGame.TabIndex = 2;
            this.LblPathToGame.Text = "Путь к игровому клиенту:";
            this.LblPathToGame.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 16);
            this.label2.TabIndex = 1;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblIPAddress
            // 
            this.LblIPAddress.Location = new System.Drawing.Point(6, 19);
            this.LblIPAddress.Name = "LblIPAddress";
            this.LblIPAddress.Size = new System.Drawing.Size(211, 16);
            this.LblIPAddress.TabIndex = 0;
            this.LblIPAddress.Text = "IP адрес сервера:";
            this.LblIPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(960, 490);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.PicButDiscord);
            this.Controls.Add(this.PicButGithub);
            this.Controls.Add(this.PicButExit);
            this.Controls.Add(this.PicButSetting);
            this.Controls.Add(this.PicButLangChange);
            this.Controls.Add(this.PicButEnter);
            this.Controls.Add(this.LblPassword);
            this.Controls.Add(this.LblLogin);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.webBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LauncherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ArcheAge Launcher";
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.Load += new System.EventHandler(this.LauncherForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicButEnter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButLangChange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButGithub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButDiscord)).EndInit();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label LblLogin;
        private System.Windows.Forms.Label LblPassword;
        private System.Windows.Forms.PictureBox PicButEnter;
        private System.Windows.Forms.PictureBox PicButLangChange;
        private System.Windows.Forms.PictureBox PicButSetting;
        private System.Windows.Forms.PictureBox PicButExit;
        private System.Windows.Forms.PictureBox PicButGithub;
        private System.Windows.Forms.PictureBox PicButDiscord;
        public System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.Button ButSettingCancel;
        private System.Windows.Forms.Button ButSettingSave;
        private System.Windows.Forms.CheckBox cbHideSplashLogo;
        private System.Windows.Forms.CheckBox cbSkipIntro;
        private System.Windows.Forms.Button ButPathToGame;
        private System.Windows.Forms.TextBox txtPathToGame;
        private System.Windows.Forms.CheckBox cbSaveLogin;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label LblPathToGame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LblIPAddress;
    }
}

