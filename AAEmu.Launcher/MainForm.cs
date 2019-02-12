using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AAEmu.Launcher
{
    public partial class LauncherForm : Form
    {
        public partial class Settings
        {
            [JsonProperty("lang")]
            public string Lang { get; set; }

            [JsonProperty("pathToGame")]
            public string PathToGame { get; set; }

            [JsonProperty("serverIPAddress")]
            public string ServerIpAddress { get; set; }

            [JsonProperty("saveLoginAndPassword")]
            public string SaveLoginAndPassword { get; set; }

            [JsonProperty("skipIntro")]
            public string SkipIntro { get; set; }

            [JsonProperty("hideSplashLogo")]
            public string HideSplashLogo { get; set; }

            [JsonProperty("lastLoginUser")]
            public string LastLoginUser { get; set; }

            [JsonProperty("lastLoginPass")]
            public string LastLoginPass { get; set; }

            [JsonProperty("userHistory")]
            public List<string> UserHistory { get; set; }
        }

        public Settings Setting = new Settings();

        const string launcherConfigFile = "launcher.config";
        const string urlGitHub = "https://github.com/atel0/AAEmu";
        const string urlDiscordInvite = "https://discord.gg/vn8E8E6";
        const string urlNews = "https://aaemu.pw/updater/";
        const string urlWebsite = "https://aaemu.info/";
        // const string urlNews = "https://cl2.widgetbot.io/channels/479677351618281472/481782245087248400";
        private bool formMouseDown;
        private Point lastLocation;

        public LauncherForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
            if (progressBar1.Value >= 100)
                progressBar1.Value = 0;
            if (progressBar2.Value >= 100)
                progressBar2.Value = 0;
            progressBar1.Value += 1;
            progressBar2.Value += 2;
            */
        }


        private void UpdateFormLanguageElements()
        {

            switch (Setting.Lang)
            {
                case "ru":
                    btnLangChange.Image = Properties.Resources.But_Lang_Ru;
                    lLogin.Text = "Логин";
                    lPassword.Text = "Пароль";
                    lIPAddress.Text = "IP адрес сервера:";
                    lPathToGame.Text = "Путь к игровому клиенту:";
                    cbSaveLogin.Text = "Сохранять учетные данные";
                    cbSkipIntro.Text = "Пропустить заставку";
                    cbHideSplashLogo.Text = "Показывать логотип загрузки";
                    btnSettingsSave.Text = "Сохранить";
                    btnSettingsCancel.Text = "Отмена";
                    btnSettings.Text = "Настройки";
                    gbSettings.Text = btnSettings.Text;
                    btnWebsite.Text = "сайт";
                    btnPlay.Text = "играть";
                    break;
                case "de":
                    btnLangChange.Image = Properties.Resources.But_Lang_De;
                    lLogin.Text = "Benutzername";
                    lPassword.Text = "Passwort";
                    lIPAddress.Text = "Server IP Adresse:";
                    lPathToGame.Text = "Pfad zum Game Client:";
                    cbSaveLogin.Text = "Speichern sie Benutzername und Passwort";
                    cbSkipIntro.Text = "Intro überspringen";
                    cbHideSplashLogo.Text = "Begrüßungsbildschirm ausblenden";
                    btnSettingsSave.Text = "Speichern";
                    btnSettingsCancel.Text = "Abbrechen";
                    btnSettings.Text = "Einstellungen";
                    gbSettings.Text = btnSettings.Text;
                    btnWebsite.Text = "Website";
                    btnPlay.Text = "Spielen" ;
                    break;
                case "en":
                default:
                    Setting.Lang = "en";
                    btnLangChange.Image = Properties.Resources.But_Lang_En;
                    lLogin.Text = "Username";
                    lPassword.Text = "Password";
                    lIPAddress.Text = "Server IP Address:";
                    lPathToGame.Text = "Path to Game Client:";
                    cbSaveLogin.Text = "Save Login & Password";
                    cbSkipIntro.Text = "Skip Intro";
                    cbHideSplashLogo.Text = "Hide Splash Screen";
                    btnSettingsSave.Text = "Save";
                    btnSettingsCancel.Text = "Cancel";
                    btnSettings.Text = "Settings";
                    gbSettings.Text = btnSettings.Text;
                    btnWebsite.Text = "Website";
                    btnPlay.Text = "Play";
                    break;
            }

            btnLangChange.Refresh();

        }

        private void PicButLangChange_Click(object sender, EventArgs e)
        {
            switch(Setting.Lang)
            {
                case "ru":
                    Setting.Lang = "en";
                    break;
                case "en":
                    Setting.Lang = "de";
                    break;
                case "de":
                    Setting.Lang = "ru";
                    break;
            }
            Console.WriteLine("Updating Language: {0}",Setting.Lang);
            UpdateFormLanguageElements();
            btnLangChange.Refresh();
        }

        private void PicButGithub_MouseEnter(object sender, EventArgs e)
        {
            btnGithub.Image = Properties.Resources.GitHub_Logo_Only_Active;
        }

        private void PicButGithub_MouseLeave(object sender, EventArgs e)
        {
            btnGithub.Image = Properties.Resources.GitHub_Logo_Only;
        }

        private void PicButDiscord_MouseEnter(object sender, EventArgs e)
        {
            btnDiscord.Image = Properties.Resources.Discord_Logo_Only_Active;
        }

        private void PicButDiscord_MouseLeave(object sender, EventArgs e)
        {
            btnDiscord.Image = Properties.Resources.Discord_Logo_Only;
        }

        private void PicButGithub_Click(object sender, EventArgs e)
        {
            Process.Start(urlGitHub);
        }

        private void PicButDiscord_Click(object sender, EventArgs e)
        {
            Process.Start(urlDiscordInvite);
        }

        private void LauncherForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        { 
            StreamReader reader = null ;
            Console.WriteLine(Application.StartupPath + "\\" + launcherConfigFile);
            try
            {
                reader = new StreamReader(Application.StartupPath + "\\" + launcherConfigFile);
                var ConfigFile = reader.ReadToEnd();
                Console.Write(ConfigFile.ToString());

                Setting = JsonConvert.DeserializeObject<Settings>(ConfigFile);
            }
            catch
            {
                // If loading fails, just put in some defaults instead
                Setting.PathToGame = "";
                Setting.ServerIpAddress = "127.0.0.1";
                Setting.Lang = "en";
                Setting.LastLoginUser = "test";
                Setting.LastLoginPass = "";
                Setting.UserHistory = new List<string>();
                Setting.UserHistory.Clear();
            }
            try
            {
                // Make sure we close out stream so the file won't be in use when we need to save it
                reader.Close();
            }
            catch
            {

            }

            ePathToGame.Text = Setting.PathToGame;
            eServerIP.Text = Setting.ServerIpAddress;

            cbLoginList.Items.Clear();
            if (Setting.UserHistory != null)
            foreach (string s in Setting.UserHistory)
                cbLoginList.Items.Add(s);

            cbLoginList.Text = Setting.LastLoginUser;
            lFakeUser.Text = cbLoginList.Text;
            ePassword.Text = Setting.LastLoginPass;
            lFakePassword.Text = new String('*', 10);

            UpdateFormLanguageElements();

            cbSaveLogin.Checked = (Setting.SaveLoginAndPassword == "True");
            cbSkipIntro.Checked = (Setting.SkipIntro == "True");
            cbHideSplashLogo.Checked = (Setting.HideSplashLogo == "True");
        }

        private void PicButSetting_MouseEnter(object sender, EventArgs e)
        {
        }

        private void PicButSetting_MouseLeave(object sender, EventArgs e)
        {
        }

        private void PicButExit_MouseEnter(object sender, EventArgs e)
        {
            btnExit.Image = Properties.Resources.btn_portal_exit_a;
        }

        private void PicButExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.Image = Properties.Resources.btn_portal_exit;
        }

        private void StartGame()
        { 
            Application.UseWaitCursor = true;
            if (Setting.PathToGame != "")
            {
                if (cbLoginList.Text != "" && ePassword.Text != "")
                {
                    byte[] data = Encoding.Default.GetBytes(ePassword.Text);
                    var result = new SHA256Managed().ComputeHash(data);

                    string LoginArg = "-r +auth_ip "+ eServerIP.Text + " -uid " + cbLoginList.Text + " -token " + BitConverter.ToString(result).Replace("-", "").ToLower();
                    string HShield = " +acpxmk";

                    ProcessStartInfo GameClient = new ProcessStartInfo();

                    GameClient.FileName = Setting.PathToGame;
                    GameClient.Arguments = LoginArg + HShield;
                    try
                    {
                        Process.Start(GameClient);

                        if (Setting.SaveLoginAndPassword == "true")
                            SaveSettings();

                        // Minimize after launching AA
                        WindowState = FormWindowState.Minimized;
                    }
                    catch {
                        MessageBox.Show("Error: Failed to start the game");
                        // MessageBox.Show("Ошибка: Проверьте указанный путь до клиента игры!");
                    }

                    } else
                {
                    MessageBox.Show("Please enter a username and password");
                    // MessageBox.Show("Логин и пароль должны быть заполнены!");
                }
            } else
            {
                MessageBox.Show("Error: No game path set");
                // MessageBox.Show("Не указан путь размещения клиента игры!");
            }
            Application.UseWaitCursor = false;
        }

        private void PicButSetting_Click(object sender, EventArgs e)
        {
        }

        private void ButPathToGame_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\ArcheAge\\Working\\Bin32";
            openFileDialog.Filter = "ArcheAge Game|archeage.exe|Executeable|*.exe|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                Setting.PathToGame = openFileDialog.FileName;
                ePathToGame.Text = Setting.PathToGame;
                Console.WriteLine(openFileDialog.OpenFile());
            }
        }

        private void ButSettingSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            gbSettings.Visible = false;
        }

        private void ButSettingCancel_Click(object sender, EventArgs e)
        {
            gbSettings.Visible = false;
        }

        private void LauncherForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Code for form drag
            formMouseDown = true;
            lastLocation = e.Location;
        }

        private void LauncherForm_MouseMove(object sender, MouseEventArgs e)
        {
            // Code for form drag
            if (formMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void LauncherForm_MouseUp(object sender, MouseEventArgs e)
        {
            // Code for form drag
            formMouseDown = false;
        }

        private void txtLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lFakePassword.Hide();
                ePassword.Show();
                ePassword.Focus();
                ePassword.SelectAll();
                eLogin.Hide();
                cbLoginList.Hide();
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnPlay_Click(null,null);
            }

        }

        private void PicButMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void PicButMinimize_MouseEnter(object sender, EventArgs e)
        {
            btnMinimize.Image = Properties.Resources.btn_pickup_a;
        }

        private void PicButMinimize_MouseLeave(object sender, EventArgs e)
        {
            btnMinimize.Image = Properties.Resources.btn_pickup;
        }

        private void PicButExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LauncherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            Setting.PathToGame = ePathToGame.Text;
            Setting.SaveLoginAndPassword = cbSaveLogin.Checked.ToString();
            Setting.ServerIpAddress = eServerIP.Text;
            Setting.SkipIntro = cbSkipIntro.Checked.ToString();
            Setting.HideSplashLogo = cbHideSplashLogo.Checked.ToString();

            if (cbSaveLogin.Checked)
            {
                Setting.LastLoginUser = cbLoginList.Text;
                // TODO: Save the password in a somewhat more safe way
                Setting.LastLoginPass = ""; // don't save password, unsafe
                if (!cbLoginList.Items.Contains(cbLoginList.Text))
                {
                    cbLoginList.Items.Add(cbLoginList.Text);
                }
            } else
            {
                Setting.LastLoginUser = "";
                Setting.LastLoginPass = "";
            }
            Setting.UserHistory = new List<string>();
            Setting.UserHistory.Clear();
            foreach(Object o in cbLoginList.Items)
                Setting.UserHistory.Add(cbLoginList.GetItemText(o));

            var SettingJson = JsonConvert.SerializeObject(Setting);
            Console.Write("Saving Settings:\n" + SettingJson);
            // Console.Write("Настройки:\n" + SettingJson);
            File.WriteAllText(Application.StartupPath + "\\" + launcherConfigFile, SettingJson);
        }

        private void LFakePassword_MouseEnter(object sender, EventArgs e)
        {
            lFakePassword.Hide();
            ePassword.Show();
            ePassword.Focus();

            cbLoginList.Hide();
        }

        private void txtPassword_MouseLeave(object sender, EventArgs e)
        {
            Focus();
            ePassword.Hide();
            lFakePassword.Text = new String('*', ePassword.TextLength);
            lFakePassword.Show();            
        }

        private void LFakeUser_MouseEnter(object sender, EventArgs e)
        {
            lFakeUser.Hide();
            eLogin.Show();
            eLogin.Focus();
            cbLoginList.Show();
        }

        private void txtLogin_MouseLeave(object sender, EventArgs e)
        {
            Focus();
            eLogin.Hide();
            //txtLoginList.Hide();
            lFakeUser.Text = eLogin.Text;
            lFakeUser.Show();
        }

        private void txtLoginList_SelectedValueChanged(object sender, EventArgs e)
        {
            eLogin.Text = cbLoginList.Text;
            lFakeUser.Text = cbLoginList.Text;
            cbLoginList.Hide();
        }

        private void txtLogin_Leave(object sender, EventArgs e)
        {
            lFakeUser.Text = eLogin.Text;

        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            lFakePassword.Text = new String('*', ePassword.TextLength);
            lFakePassword.Show();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void btnPlay_MouseEnter(object sender, EventArgs e)
        {
            btnPlay.Image = Properties.Resources.btn_green_a;
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            btnPlay.Image = Properties.Resources.btn_green;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            gbSettings.Visible = !gbSettings.Visible;
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            Process.Start(urlWebsite);
        }
    }
}
