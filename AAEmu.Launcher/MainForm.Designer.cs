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
            this.label1 = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.ComboBox();
            this.PicButMinimize = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PicButEnter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButLangChange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButGithub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButDiscord)).BeginInit();
            this.gbSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicButMinimize)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(12, 219);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(304, 228);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Url = new System.Uri("http://aaemu.pw/updater/", System.UriKind.Absolute);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtPassword.Location = new System.Drawing.Point(393, 121);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(209, 35);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyUp);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 474);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(306, 14);
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(13, 458);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(305, 10);
            this.progressBar2.TabIndex = 4;
            this.progressBar2.Visible = false;
            // 
            // LblLogin
            // 
            this.LblLogin.BackColor = System.Drawing.Color.Transparent;
            this.LblLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLogin.ForeColor = System.Drawing.Color.SpringGreen;
            this.LblLogin.Location = new System.Drawing.Point(58, 79);
            this.LblLogin.Name = "LblLogin";
            this.LblLogin.Size = new System.Drawing.Size(329, 35);
            this.LblLogin.TabIndex = 5;
            this.LblLogin.Text = "Логин:";
            this.LblLogin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblPassword
            // 
            this.LblPassword.BackColor = System.Drawing.Color.Transparent;
            this.LblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPassword.ForeColor = System.Drawing.Color.SpringGreen;
            this.LblPassword.Location = new System.Drawing.Point(63, 121);
            this.LblPassword.Name = "LblPassword";
            this.LblPassword.Size = new System.Drawing.Size(324, 35);
            this.LblPassword.TabIndex = 6;
            this.LblPassword.Text = "Пароль:";
            this.LblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PicButEnter
            // 
            this.PicButEnter.BackColor = System.Drawing.Color.Transparent;
            this.PicButEnter.BackgroundImage = global::AAEmu.Launcher.Properties.Resources.Logo;
            this.PicButEnter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicButEnter.Location = new System.Drawing.Point(393, 219);
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
            this.PicButLangChange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicButLangChange.Image = global::AAEmu.Launcher.Properties.Resources.But_Lang_Ru;
            this.PicButLangChange.Location = new System.Drawing.Point(12, 61);
            this.PicButLangChange.Name = "PicButLangChange";
            this.PicButLangChange.Size = new System.Drawing.Size(40, 40);
            this.PicButLangChange.TabIndex = 8;
            this.PicButLangChange.TabStop = false;
            this.PicButLangChange.Click += new System.EventHandler(this.PicButLangChange_Click);
            // 
            // PicButSetting
            // 
            this.PicButSetting.BackColor = System.Drawing.Color.Transparent;
            this.PicButSetting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicButSetting.Image = global::AAEmu.Launcher.Properties.Resources.btn_conf;
            this.PicButSetting.Location = new System.Drawing.Point(748, 115);
            this.PicButSetting.Name = "PicButSetting";
            this.PicButSetting.Size = new System.Drawing.Size(48, 48);
            this.PicButSetting.TabIndex = 9;
            this.PicButSetting.TabStop = false;
            this.PicButSetting.Click += new System.EventHandler(this.PicButSetting_Click);
            this.PicButSetting.MouseEnter += new System.EventHandler(this.PicButSetting_MouseEnter);
            this.PicButSetting.MouseLeave += new System.EventHandler(this.PicButSetting_MouseLeave);
            // 
            // PicButExit
            // 
            this.PicButExit.BackColor = System.Drawing.Color.Transparent;
            this.PicButExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicButExit.Image = global::AAEmu.Launcher.Properties.Resources.btn_portal_exit;
            this.PicButExit.Location = new System.Drawing.Point(748, 61);
            this.PicButExit.Name = "PicButExit";
            this.PicButExit.Size = new System.Drawing.Size(48, 48);
            this.PicButExit.TabIndex = 10;
            this.PicButExit.TabStop = false;
            this.PicButExit.Click += new System.EventHandler(this.PicButExit_Click);
            this.PicButExit.MouseEnter += new System.EventHandler(this.PicButExit_MouseEnter);
            this.PicButExit.MouseLeave += new System.EventHandler(this.PicButExit_MouseLeave);
            // 
            // PicButGithub
            // 
            this.PicButGithub.BackColor = System.Drawing.Color.Transparent;
            this.PicButGithub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicButGithub.Image = ((System.Drawing.Image)(resources.GetObject("PicButGithub.Image")));
            this.PicButGithub.Location = new System.Drawing.Point(702, 383);
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
            this.PicButDiscord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicButDiscord.Image = global::AAEmu.Launcher.Properties.Resources.Discord_Logo_Only;
            this.PicButDiscord.Location = new System.Drawing.Point(748, 383);
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
            this.gbSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
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
            this.gbSettings.Location = new System.Drawing.Point(96, 171);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(646, 195);
            this.gbSettings.TabIndex = 13;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Настройки";
            this.gbSettings.Visible = false;
            // 
            // ButSettingCancel
            // 
            this.ButSettingCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.ButSettingCancel.Location = new System.Drawing.Point(384, 157);
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
            this.ButSettingSave.Location = new System.Drawing.Point(296, 157);
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
            this.cbHideSplashLogo.Location = new System.Drawing.Point(276, 114);
            this.cbHideSplashLogo.Name = "cbHideSplashLogo";
            this.cbHideSplashLogo.Size = new System.Drawing.Size(254, 20);
            this.cbHideSplashLogo.TabIndex = 11;
            this.cbHideSplashLogo.Text = "Показывать логотип загрузки";
            this.cbHideSplashLogo.UseVisualStyleBackColor = true;
            // 
            // cbSkipIntro
            // 
            this.cbSkipIntro.AutoSize = true;
            this.cbSkipIntro.Location = new System.Drawing.Point(276, 91);
            this.cbSkipIntro.Name = "cbSkipIntro";
            this.cbSkipIntro.Size = new System.Drawing.Size(188, 20);
            this.cbSkipIntro.TabIndex = 10;
            this.cbSkipIntro.Text = "Пропустить заставку";
            this.cbSkipIntro.UseVisualStyleBackColor = true;
            // 
            // ButPathToGame
            // 
            this.ButPathToGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.ButPathToGame.Location = new System.Drawing.Point(606, 41);
            this.ButPathToGame.Name = "ButPathToGame";
            this.ButPathToGame.Size = new System.Drawing.Size(28, 23);
            this.ButPathToGame.TabIndex = 8;
            this.ButPathToGame.Text = "...";
            this.ButPathToGame.UseVisualStyleBackColor = true;
            this.ButPathToGame.Click += new System.EventHandler(this.ButPathToGame_Click);
            // 
            // txtPathToGame
            // 
            this.txtPathToGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtPathToGame.Enabled = false;
            this.txtPathToGame.ForeColor = System.Drawing.Color.White;
            this.txtPathToGame.Location = new System.Drawing.Point(277, 42);
            this.txtPathToGame.Name = "txtPathToGame";
            this.txtPathToGame.Size = new System.Drawing.Size(323, 21);
            this.txtPathToGame.TabIndex = 7;
            this.txtPathToGame.Text = "c:\\ArcheAge\\bin32\\Archeage.exe";
            // 
            // cbSaveLogin
            // 
            this.cbSaveLogin.AutoSize = true;
            this.cbSaveLogin.Location = new System.Drawing.Point(276, 68);
            this.cbSaveLogin.Name = "cbSaveLogin";
            this.cbSaveLogin.Size = new System.Drawing.Size(230, 20);
            this.cbSaveLogin.TabIndex = 6;
            this.cbSaveLogin.Text = "Сохранять учетные данные";
            this.cbSaveLogin.UseVisualStyleBackColor = true;
            // 
            // txtServerIP
            // 
            this.txtServerIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtServerIP.ForeColor = System.Drawing.Color.White;
            this.txtServerIP.Location = new System.Drawing.Point(277, 14);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(100, 21);
            this.txtServerIP.TabIndex = 5;
            this.txtServerIP.Text = "127.0.0.1";
            // 
            // LblPathToGame
            // 
            this.LblPathToGame.Location = new System.Drawing.Point(6, 45);
            this.LblPathToGame.Name = "LblPathToGame";
            this.LblPathToGame.Size = new System.Drawing.Size(265, 16);
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
            this.LblIPAddress.Size = new System.Drawing.Size(265, 16);
            this.LblIPAddress.TabIndex = 0;
            this.LblIPAddress.Text = "IP адрес сервера:";
            this.LblIPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.22642F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "News";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtLogin
            // 
            this.txtLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.30189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtLogin.FormattingEnabled = true;
            this.txtLogin.Location = new System.Drawing.Point(393, 77);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(209, 37);
            this.txtLogin.TabIndex = 15;
            this.txtLogin.Text = "Test";
            this.txtLogin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtLogin_KeyUp);
            // 
            // PicButMinimize
            // 
            this.PicButMinimize.BackColor = System.Drawing.Color.Transparent;
            this.PicButMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PicButMinimize.Image = global::AAEmu.Launcher.Properties.Resources.btn_pickup;
            this.PicButMinimize.Location = new System.Drawing.Point(694, 61);
            this.PicButMinimize.Name = "PicButMinimize";
            this.PicButMinimize.Size = new System.Drawing.Size(48, 48);
            this.PicButMinimize.TabIndex = 16;
            this.PicButMinimize.TabStop = false;
            this.PicButMinimize.Click += new System.EventHandler(this.PicButMinimize_Click);
            this.PicButMinimize.MouseEnter += new System.EventHandler(this.PicButMinimize_MouseEnter);
            this.PicButMinimize.MouseLeave += new System.EventHandler(this.PicButMinimize_MouseLeave);
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(106F, 106F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.PicButMinimize);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.LblLogin);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PicButDiscord);
            this.Controls.Add(this.PicButGithub);
            this.Controls.Add(this.PicButExit);
            this.Controls.Add(this.PicButSetting);
            this.Controls.Add(this.PicButLangChange);
            this.Controls.Add(this.PicButEnter);
            this.Controls.Add(this.LblPassword);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.webBrowser);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 500);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "LauncherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AAEmu Launcher";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LauncherForm_FormClosed);
            this.Load += new System.EventHandler(this.LauncherForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.PicButEnter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButLangChange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButGithub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicButDiscord)).EndInit();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicButMinimize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox txtLogin;
        private System.Windows.Forms.PictureBox PicButMinimize;
    }
}

