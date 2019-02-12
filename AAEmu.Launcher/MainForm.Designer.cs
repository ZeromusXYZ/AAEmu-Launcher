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
            this.ePassword = new System.Windows.Forms.TextBox();
            this.pb1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pb2 = new System.Windows.Forms.ProgressBar();
            this.btnLangChange = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.PictureBox();
            this.btnGithub = new System.Windows.Forms.PictureBox();
            this.btnDiscord = new System.Windows.Forms.PictureBox();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.btnSettingsCancel = new System.Windows.Forms.Button();
            this.btnSettingsSave = new System.Windows.Forms.Button();
            this.cbHideSplashLogo = new System.Windows.Forms.CheckBox();
            this.cbSkipIntro = new System.Windows.Forms.CheckBox();
            this.btnPathToGame = new System.Windows.Forms.Button();
            this.ePathToGame = new System.Windows.Forms.TextBox();
            this.cbSaveLogin = new System.Windows.Forms.CheckBox();
            this.eServerIP = new System.Windows.Forms.TextBox();
            this.lPathToGame = new System.Windows.Forms.Label();
            this.lIPAddress = new System.Windows.Forms.Label();
            this.cbLoginList = new System.Windows.Forms.ComboBox();
            this.btnMinimize = new System.Windows.Forms.PictureBox();
            this.lAppVersion = new System.Windows.Forms.Label();
            this.lLogin = new System.Windows.Forms.Label();
            this.lPassword = new System.Windows.Forms.Label();
            this.lFakePassword = new System.Windows.Forms.Label();
            this.eLogin = new System.Windows.Forms.TextBox();
            this.lFakeUser = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Label();
            this.btnWebsite = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnLangChange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGithub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDiscord)).BeginInit();
            this.gbSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ePassword
            // 
            this.ePassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            this.ePassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ePassword.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ePassword.ForeColor = System.Drawing.Color.White;
            this.ePassword.Location = new System.Drawing.Point(303, 396);
            this.ePassword.Name = "ePassword";
            this.ePassword.PasswordChar = '*';
            this.ePassword.Size = new System.Drawing.Size(213, 19);
            this.ePassword.TabIndex = 2;
            this.ePassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ePassword.UseSystemPasswordChar = true;
            this.ePassword.Visible = false;
            this.ePassword.WordWrap = false;
            this.ePassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyUp);
            this.ePassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            this.ePassword.MouseLeave += new System.EventHandler(this.txtPassword_MouseLeave);
            // 
            // pb1
            // 
            this.pb1.Location = new System.Drawing.Point(145, 25);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(277, 13);
            this.pb1.TabIndex = 3;
            this.pb1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pb2
            // 
            this.pb2.Location = new System.Drawing.Point(146, 7);
            this.pb2.Name = "pb2";
            this.pb2.Size = new System.Drawing.Size(276, 9);
            this.pb2.TabIndex = 4;
            this.pb2.Visible = false;
            // 
            // btnLangChange
            // 
            this.btnLangChange.BackColor = System.Drawing.Color.Transparent;
            this.btnLangChange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLangChange.Image = global::AAEmu.Launcher.Properties.Resources.But_Lang_Ru;
            this.btnLangChange.Location = new System.Drawing.Point(850, 2);
            this.btnLangChange.Name = "btnLangChange";
            this.btnLangChange.Size = new System.Drawing.Size(40, 40);
            this.btnLangChange.TabIndex = 8;
            this.btnLangChange.TabStop = false;
            this.btnLangChange.Click += new System.EventHandler(this.PicButLangChange_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Image = global::AAEmu.Launcher.Properties.Resources.btn_portal_exit;
            this.btnExit.Location = new System.Drawing.Point(796, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(48, 48);
            this.btnExit.TabIndex = 10;
            this.btnExit.TabStop = false;
            this.btnExit.Click += new System.EventHandler(this.PicButExit_Click);
            this.btnExit.MouseEnter += new System.EventHandler(this.PicButExit_MouseEnter);
            this.btnExit.MouseLeave += new System.EventHandler(this.PicButExit_MouseLeave);
            // 
            // btnGithub
            // 
            this.btnGithub.BackColor = System.Drawing.Color.Transparent;
            this.btnGithub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGithub.Image = ((System.Drawing.Image)(resources.GetObject("btnGithub.Image")));
            this.btnGithub.Location = new System.Drawing.Point(789, 461);
            this.btnGithub.Name = "btnGithub";
            this.btnGithub.Size = new System.Drawing.Size(40, 40);
            this.btnGithub.TabIndex = 11;
            this.btnGithub.TabStop = false;
            this.btnGithub.Click += new System.EventHandler(this.PicButGithub_Click);
            this.btnGithub.MouseEnter += new System.EventHandler(this.PicButGithub_MouseEnter);
            this.btnGithub.MouseLeave += new System.EventHandler(this.PicButGithub_MouseLeave);
            // 
            // btnDiscord
            // 
            this.btnDiscord.BackColor = System.Drawing.Color.Transparent;
            this.btnDiscord.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDiscord.Image = global::AAEmu.Launcher.Properties.Resources.Discord_Logo_Only;
            this.btnDiscord.Location = new System.Drawing.Point(743, 461);
            this.btnDiscord.Name = "btnDiscord";
            this.btnDiscord.Size = new System.Drawing.Size(40, 40);
            this.btnDiscord.TabIndex = 12;
            this.btnDiscord.TabStop = false;
            this.btnDiscord.Click += new System.EventHandler(this.PicButDiscord_Click);
            this.btnDiscord.MouseEnter += new System.EventHandler(this.PicButDiscord_MouseEnter);
            this.btnDiscord.MouseLeave += new System.EventHandler(this.PicButDiscord_MouseLeave);
            // 
            // gbSettings
            // 
            this.gbSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.gbSettings.Controls.Add(this.btnSettingsCancel);
            this.gbSettings.Controls.Add(this.btnSettingsSave);
            this.gbSettings.Controls.Add(this.cbHideSplashLogo);
            this.gbSettings.Controls.Add(this.cbSkipIntro);
            this.gbSettings.Controls.Add(this.btnPathToGame);
            this.gbSettings.Controls.Add(this.ePathToGame);
            this.gbSettings.Controls.Add(this.cbSaveLogin);
            this.gbSettings.Controls.Add(this.eServerIP);
            this.gbSettings.Controls.Add(this.lPathToGame);
            this.gbSettings.Controls.Add(this.lIPAddress);
            this.gbSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gbSettings.ForeColor = System.Drawing.Color.White;
            this.gbSettings.Location = new System.Drawing.Point(84, 126);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(585, 177);
            this.gbSettings.TabIndex = 13;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Настройки";
            this.gbSettings.Visible = false;
            // 
            // btnSettingsCancel
            // 
            this.btnSettingsCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.btnSettingsCancel.Location = new System.Drawing.Point(348, 142);
            this.btnSettingsCancel.Name = "btnSettingsCancel";
            this.btnSettingsCancel.Size = new System.Drawing.Size(73, 21);
            this.btnSettingsCancel.TabIndex = 13;
            this.btnSettingsCancel.Text = "Отмена";
            this.btnSettingsCancel.UseVisualStyleBackColor = true;
            this.btnSettingsCancel.Click += new System.EventHandler(this.ButSettingCancel_Click);
            // 
            // btnSettingsSave
            // 
            this.btnSettingsSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.btnSettingsSave.Location = new System.Drawing.Point(268, 142);
            this.btnSettingsSave.Name = "btnSettingsSave";
            this.btnSettingsSave.Size = new System.Drawing.Size(73, 21);
            this.btnSettingsSave.TabIndex = 12;
            this.btnSettingsSave.Text = "Сохранить";
            this.btnSettingsSave.UseVisualStyleBackColor = true;
            this.btnSettingsSave.Click += new System.EventHandler(this.ButSettingSave_Click);
            // 
            // cbHideSplashLogo
            // 
            this.cbHideSplashLogo.AutoSize = true;
            this.cbHideSplashLogo.Location = new System.Drawing.Point(250, 103);
            this.cbHideSplashLogo.Name = "cbHideSplashLogo";
            this.cbHideSplashLogo.Size = new System.Drawing.Size(208, 17);
            this.cbHideSplashLogo.TabIndex = 11;
            this.cbHideSplashLogo.Text = "Показывать логотип загрузки";
            this.cbHideSplashLogo.UseVisualStyleBackColor = true;
            // 
            // cbSkipIntro
            // 
            this.cbSkipIntro.AutoSize = true;
            this.cbSkipIntro.Location = new System.Drawing.Point(250, 82);
            this.cbSkipIntro.Name = "cbSkipIntro";
            this.cbSkipIntro.Size = new System.Drawing.Size(153, 17);
            this.cbSkipIntro.TabIndex = 10;
            this.cbSkipIntro.Text = "Пропустить заставку";
            this.cbSkipIntro.UseVisualStyleBackColor = true;
            // 
            // btnPathToGame
            // 
            this.btnPathToGame.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(56)))), ((int)(((byte)(66)))));
            this.btnPathToGame.Location = new System.Drawing.Point(549, 37);
            this.btnPathToGame.Name = "btnPathToGame";
            this.btnPathToGame.Size = new System.Drawing.Size(25, 21);
            this.btnPathToGame.TabIndex = 8;
            this.btnPathToGame.Text = "...";
            this.btnPathToGame.UseVisualStyleBackColor = true;
            this.btnPathToGame.Click += new System.EventHandler(this.ButPathToGame_Click);
            // 
            // ePathToGame
            // 
            this.ePathToGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ePathToGame.Enabled = false;
            this.ePathToGame.ForeColor = System.Drawing.Color.White;
            this.ePathToGame.Location = new System.Drawing.Point(251, 38);
            this.ePathToGame.Name = "ePathToGame";
            this.ePathToGame.Size = new System.Drawing.Size(293, 20);
            this.ePathToGame.TabIndex = 7;
            this.ePathToGame.Text = "c:\\ArcheAge\\bin32\\Archeage.exe";
            // 
            // cbSaveLogin
            // 
            this.cbSaveLogin.AutoSize = true;
            this.cbSaveLogin.Location = new System.Drawing.Point(250, 62);
            this.cbSaveLogin.Name = "cbSaveLogin";
            this.cbSaveLogin.Size = new System.Drawing.Size(188, 17);
            this.cbSaveLogin.TabIndex = 6;
            this.cbSaveLogin.Text = "Сохранять учетные данные";
            this.cbSaveLogin.UseVisualStyleBackColor = true;
            // 
            // eServerIP
            // 
            this.eServerIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.eServerIP.ForeColor = System.Drawing.Color.White;
            this.eServerIP.Location = new System.Drawing.Point(251, 13);
            this.eServerIP.Name = "eServerIP";
            this.eServerIP.Size = new System.Drawing.Size(91, 20);
            this.eServerIP.TabIndex = 5;
            this.eServerIP.Text = "127.0.0.1";
            // 
            // lPathToGame
            // 
            this.lPathToGame.Location = new System.Drawing.Point(5, 41);
            this.lPathToGame.Name = "lPathToGame";
            this.lPathToGame.Size = new System.Drawing.Size(240, 14);
            this.lPathToGame.TabIndex = 2;
            this.lPathToGame.Text = "Путь к игровому клиенту:";
            this.lPathToGame.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lIPAddress
            // 
            this.lIPAddress.Location = new System.Drawing.Point(5, 17);
            this.lIPAddress.Name = "lIPAddress";
            this.lIPAddress.Size = new System.Drawing.Size(240, 14);
            this.lIPAddress.TabIndex = 0;
            this.lIPAddress.Text = "IP адрес сервера:";
            this.lIPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbLoginList
            // 
            this.cbLoginList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            this.cbLoginList.DropDownHeight = 140;
            this.cbLoginList.DropDownWidth = 200;
            this.cbLoginList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLoginList.Font = new System.Drawing.Font("Georgia", 10.18868F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLoginList.ForeColor = System.Drawing.Color.White;
            this.cbLoginList.FormattingEnabled = true;
            this.cbLoginList.IntegralHeight = false;
            this.cbLoginList.Location = new System.Drawing.Point(47, 396);
            this.cbLoginList.Name = "cbLoginList";
            this.cbLoginList.Size = new System.Drawing.Size(18, 25);
            this.cbLoginList.TabIndex = 15;
            this.cbLoginList.Visible = false;
            this.cbLoginList.SelectedValueChanged += new System.EventHandler(this.txtLoginList_SelectedValueChanged);
            this.cbLoginList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtLogin_KeyUp);
            // 
            // btnMinimize
            // 
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.Image = global::AAEmu.Launcher.Properties.Resources.btn_pickup;
            this.btnMinimize.Location = new System.Drawing.Point(742, 2);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(48, 48);
            this.btnMinimize.TabIndex = 16;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Click += new System.EventHandler(this.PicButMinimize_Click);
            this.btnMinimize.MouseEnter += new System.EventHandler(this.PicButMinimize_MouseEnter);
            this.btnMinimize.MouseLeave += new System.EventHandler(this.PicButMinimize_MouseLeave);
            // 
            // lAppVersion
            // 
            this.lAppVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lAppVersion.BackColor = System.Drawing.Color.Transparent;
            this.lAppVersion.ForeColor = System.Drawing.Color.White;
            this.lAppVersion.Location = new System.Drawing.Point(835, 480);
            this.lAppVersion.Name = "lAppVersion";
            this.lAppVersion.Size = new System.Drawing.Size(110, 21);
            this.lAppVersion.TabIndex = 17;
            this.lAppVersion.Text = "AAEmu 2019";
            this.lAppVersion.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lLogin
            // 
            this.lLogin.BackColor = System.Drawing.Color.Transparent;
            this.lLogin.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lLogin.ForeColor = System.Drawing.Color.White;
            this.lLogin.Location = new System.Drawing.Point(36, 360);
            this.lLogin.Name = "lLogin";
            this.lLogin.Size = new System.Drawing.Size(110, 25);
            this.lLogin.TabIndex = 18;
            this.lLogin.Text = "Username";
            this.lLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lPassword
            // 
            this.lPassword.BackColor = System.Drawing.Color.Transparent;
            this.lPassword.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPassword.ForeColor = System.Drawing.Color.White;
            this.lPassword.Location = new System.Drawing.Point(300, 360);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(110, 25);
            this.lPassword.TabIndex = 19;
            this.lPassword.Text = "Password";
            this.lPassword.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFakePassword
            // 
            this.lFakePassword.BackColor = System.Drawing.Color.Transparent;
            this.lFakePassword.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFakePassword.ForeColor = System.Drawing.Color.White;
            this.lFakePassword.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lFakePassword.Location = new System.Drawing.Point(299, 393);
            this.lFakePassword.Name = "lFakePassword";
            this.lFakePassword.Size = new System.Drawing.Size(213, 24);
            this.lFakePassword.TabIndex = 20;
            this.lFakePassword.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lFakePassword.MouseEnter += new System.EventHandler(this.LFakePassword_MouseEnter);
            this.lFakePassword.MouseLeave += new System.EventHandler(this.txtPassword_MouseLeave);
            // 
            // eLogin
            // 
            this.eLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(45)))), ((int)(((byte)(65)))));
            this.eLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.eLogin.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eLogin.ForeColor = System.Drawing.Color.White;
            this.eLogin.Location = new System.Drawing.Point(39, 398);
            this.eLogin.Name = "eLogin";
            this.eLogin.Size = new System.Drawing.Size(213, 19);
            this.eLogin.TabIndex = 21;
            this.eLogin.Text = "test";
            this.eLogin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.eLogin.Visible = false;
            this.eLogin.WordWrap = false;
            this.eLogin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtLogin_KeyUp);
            this.eLogin.Leave += new System.EventHandler(this.txtLogin_Leave);
            this.eLogin.MouseLeave += new System.EventHandler(this.txtLogin_MouseLeave);
            // 
            // lFakeUser
            // 
            this.lFakeUser.BackColor = System.Drawing.Color.Transparent;
            this.lFakeUser.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lFakeUser.ForeColor = System.Drawing.Color.White;
            this.lFakeUser.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lFakeUser.Location = new System.Drawing.Point(35, 397);
            this.lFakeUser.Name = "lFakeUser";
            this.lFakeUser.Size = new System.Drawing.Size(213, 24);
            this.lFakeUser.TabIndex = 22;
            this.lFakeUser.Text = "test";
            this.lFakeUser.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lFakeUser.MouseEnter += new System.EventHandler(this.LFakeUser_MouseEnter);
            this.lFakeUser.MouseLeave += new System.EventHandler(this.txtLogin_MouseLeave);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Font = new System.Drawing.Font("Georgia", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.ForeColor = System.Drawing.Color.White;
            this.btnPlay.Image = global::AAEmu.Launcher.Properties.Resources.btn_green;
            this.btnPlay.Location = new System.Drawing.Point(679, 373);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(227, 67);
            this.btnPlay.TabIndex = 23;
            this.btnPlay.Text = "Play";
            this.btnPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            this.btnPlay.MouseEnter += new System.EventHandler(this.btnPlay_MouseEnter);
            this.btnPlay.MouseLeave += new System.EventHandler(this.btnPlay_MouseLeave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Image = global::AAEmu.Launcher.Properties.Resources.bg_newsfeed;
            this.label1.Location = new System.Drawing.Point(685, 85);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 263);
            this.label1.TabIndex = 24;
            this.label1.Text = "Recent News\r\n\r\nHead 1\r\ntext 1\r\n\r\nHead 2\r\ntext 2\r\n\r\nHead 3\r\ntext 3\r\n\r\nHead 4\r\ntext" +
    " 4\r\n\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Image = global::AAEmu.Launcher.Properties.Resources.btn_red_small;
            this.btnSettings.Location = new System.Drawing.Point(687, 350);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(88, 26);
            this.btnSettings.TabIndex = 25;
            this.btnSettings.Text = "Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnWebsite
            // 
            this.btnWebsite.BackColor = System.Drawing.Color.Transparent;
            this.btnWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWebsite.Font = new System.Drawing.Font("Georgia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWebsite.ForeColor = System.Drawing.Color.White;
            this.btnWebsite.Image = global::AAEmu.Launcher.Properties.Resources.btn_green_small;
            this.btnWebsite.Location = new System.Drawing.Point(796, 350);
            this.btnWebsite.Name = "btnWebsite";
            this.btnWebsite.Size = new System.Drawing.Size(94, 26);
            this.btnWebsite.TabIndex = 26;
            this.btnWebsite.Text = "Website";
            this.btnWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnWebsite.Click += new System.EventHandler(this.btnWebsite_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(25, 85);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(573, 243);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::AAEmu.Launcher.Properties.Resources.bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(950, 510);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnWebsite);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.cbLoginList);
            this.Controls.Add(this.lFakeUser);
            this.Controls.Add(this.eLogin);
            this.Controls.Add(this.ePassword);
            this.Controls.Add(this.lFakePassword);
            this.Controls.Add(this.lPassword);
            this.Controls.Add(this.lLogin);
            this.Controls.Add(this.lAppVersion);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.btnDiscord);
            this.Controls.Add(this.btnGithub);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLangChange);
            this.Controls.Add(this.pb2);
            this.Controls.Add(this.pb1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LauncherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AAEmu Launcher";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LauncherForm_FormClosed);
            this.Load += new System.EventHandler(this.LauncherForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LauncherForm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.btnLangChange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGithub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDiscord)).EndInit();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ePassword;
        private System.Windows.Forms.ProgressBar pb1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ProgressBar pb2;
        private System.Windows.Forms.PictureBox btnLangChange;
        private System.Windows.Forms.PictureBox btnExit;
        private System.Windows.Forms.PictureBox btnGithub;
        private System.Windows.Forms.PictureBox btnDiscord;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.Button btnSettingsCancel;
        private System.Windows.Forms.Button btnSettingsSave;
        private System.Windows.Forms.CheckBox cbHideSplashLogo;
        private System.Windows.Forms.CheckBox cbSkipIntro;
        private System.Windows.Forms.Button btnPathToGame;
        private System.Windows.Forms.TextBox ePathToGame;
        private System.Windows.Forms.CheckBox cbSaveLogin;
        private System.Windows.Forms.TextBox eServerIP;
        private System.Windows.Forms.Label lPathToGame;
        private System.Windows.Forms.Label lIPAddress;
        private System.Windows.Forms.ComboBox cbLoginList;
        private System.Windows.Forms.PictureBox btnMinimize;
        private System.Windows.Forms.Label lAppVersion;
        private System.Windows.Forms.Label lLogin;
        private System.Windows.Forms.Label lPassword;
        private System.Windows.Forms.Label lFakePassword;
        private System.Windows.Forms.TextBox eLogin;
        private System.Windows.Forms.Label lFakeUser;
        private System.Windows.Forms.Label btnPlay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label btnSettings;
        private System.Windows.Forms.Label btnWebsite;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

