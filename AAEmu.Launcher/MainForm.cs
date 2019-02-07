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
        }

        public Settings Setting = new Settings();

        public LauncherForm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value >= 100)
                progressBar1.Value = 0;
            if (progressBar2.Value >= 100)
                progressBar2.Value = 0;
            progressBar1.Value += 1;
            progressBar2.Value += 2;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PicButLangChange_Click(object sender, EventArgs e)
        {
            switch(Setting.Lang)
            {
                case "Ru":
                    PicButLangChange.Image = Properties.Resources.But_Lang_En;
                    Setting.Lang = "En";
                    LblLogin.Text = "Login:";
                    LblPassword.Text = "Password:";
                    LblIPAddress.Text = "IP Address Server:";
                    LblPathToGame.Text = "Path to Game client:";
                    cbSaveLogin.Text = "Save Login & Password";
                    cbSkipIntro.Text = "Skip Intro";
                    cbHideSplashLogo.Text = "Hide Splash loading logo";
                    ButSettingSave.Text = "Save";
                    ButSettingCancel.Text = "Cancel";
                    gbSettings.Text = "Settings:";
                    Console.WriteLine(Setting.Lang);
                    break;
                case "En":
                    PicButLangChange.Image = Properties.Resources.But_Lang_De;
                    Setting.Lang = "De";
                    LblLogin.Text = "Login:";
                    LblPassword.Text = "Passwort:";
                    LblIPAddress.Text = "IP Address Server:";
                    LblPathToGame.Text = "Path to Game client:";
                    cbSaveLogin.Text = "Save Login & Password";
                    cbSkipIntro.Text = "Skip Intro";
                    cbHideSplashLogo.Text = "Hide Splash loading logo";
                    ButSettingSave.Text = "Save";
                    ButSettingCancel.Text = "Cancel";
                    gbSettings.Text = "Settings:";
                    Console.WriteLine(Setting.Lang);
                    break;
                case "De":
                    PicButLangChange.Image = Properties.Resources.But_Lang_Ru;
                    Setting.Lang = "Ru";
                    LblLogin.Text = "Логин:";
                    LblPassword.Text = "Пароль:";
                    LblIPAddress.Text = "IP адрес сервера:";
                    LblPathToGame.Text = "Путь к игровому клиенту:";
                    cbSaveLogin.Text = "Сохранять учетные данные";
                    cbSkipIntro.Text = "Пропустить заставку";
                    cbHideSplashLogo.Text = "Показывать логотип загрузки";
                    ButSettingSave.Text = "Сохранить";
                    ButSettingCancel.Text = "Отмена";
                    gbSettings.Text = "Настройки:";
                    Console.WriteLine(Setting.Lang);
                    break;
            }
            
            PicButLangChange.Refresh();
        }

        private void PicButEnter_MouseEnter(object sender, EventArgs e)
        {
            PicButEnter.Image = Properties.Resources.Logo_Active;
        }

        private void PicButEnter_MouseLeave(object sender, EventArgs e)
        {
            PicButEnter.Image = Properties.Resources.Logo;
        }

        private void PicButGithub_MouseEnter(object sender, EventArgs e)
        {
            PicButGithub.Image = Properties.Resources.GitHub_Logo_Only_Active;
        }

        private void PicButGithub_MouseLeave(object sender, EventArgs e)
        {
            PicButGithub.Image = Properties.Resources.GitHub_Logo_Only;
        }

        private void PicButDiscord_MouseEnter(object sender, EventArgs e)
        {
            PicButDiscord.Image = Properties.Resources.Discord_Logo_Only_Active;
        }

        private void PicButDiscord_MouseLeave(object sender, EventArgs e)
        {
            PicButDiscord.Image = Properties.Resources.Discord_Logo_Only;
        }

        private void PicButGithub_Click(object sender, EventArgs e)
        {

        }

        private void PicButDiscord_Click(object sender, EventArgs e)
        {

        }

        private void LauncherForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine(Application.StartupPath + "\\" + "Launcher.Config");
            try
            {
                StreamReader reader = new StreamReader(Application.StartupPath + "\\" + "Launcher.Config");
                var ConfigFile = reader.ReadToEnd();
                Console.Write(ConfigFile.ToString());

                Setting = JsonConvert.DeserializeObject<Settings>(ConfigFile);
            }
            catch
            {
                // If loading fails, just put in some defaults instead
                Setting.PathToGame = "";
                Setting.ServerIpAddress = "127.0.0.1";
                Setting.Lang = "En";
            }


            txtPathToGame.Text = Setting.PathToGame;
            txtServerIP.Text = Setting.ServerIpAddress;

            switch (Setting.Lang)
            {
                case "En":
                    PicButLangChange.Image = Properties.Resources.But_Lang_En;
                    Setting.Lang = "En";
                    LblLogin.Text = "Login:";
                    LblPassword.Text = "Password:";
                    LblIPAddress.Text = "IP Address Server:";
                    LblPathToGame.Text = "Path to Game client:";
                    cbSaveLogin.Text = "Save Login & Password";
                    cbSkipIntro.Text = "Skip Intro";
                    cbHideSplashLogo.Text = "Hide Splash loading logo";
                    ButSettingSave.Text = "Save";
                    ButSettingCancel.Text = "Cancel";
                    gbSettings.Text = "Settings:";
                    Console.WriteLine(Setting.Lang);
                    break;
                case "De":
                    PicButLangChange.Image = Properties.Resources.But_Lang_De;
                    Setting.Lang = "De";
                    LblLogin.Text = "Login:";
                    LblPassword.Text = "Passwort:";
                    LblIPAddress.Text = "IP Address Server:";
                    LblPathToGame.Text = "Path to Game client:";
                    cbSaveLogin.Text = "Save Login & Password";
                    cbSkipIntro.Text = "Skip Intro";
                    cbHideSplashLogo.Text = "Hide Splash loading logo";
                    ButSettingSave.Text = "Save";
                    ButSettingCancel.Text = "Cancel";
                    gbSettings.Text = "Settings:";
                    Console.WriteLine(Setting.Lang);
                    break;
                case "Ru":
                    PicButLangChange.Image = Properties.Resources.But_Lang_Ru;
                    Setting.Lang = "Ru";
                    LblLogin.Text = "Логин:";
                    LblPassword.Text = "Пароль:";
                    LblIPAddress.Text = "IP адрес сервера:";
                    LblPathToGame.Text = "Путь к игровому клиенту:";
                    cbSaveLogin.Text = "Сохранять учетные данные";
                    cbSkipIntro.Text = "Пропустить заставку";
                    cbHideSplashLogo.Text = "Показывать логотип загрузки";
                    ButSettingSave.Text = "Сохранить";
                    ButSettingCancel.Text = "Отмена";
                    gbSettings.Text = "Настройки:";
                    Console.WriteLine(Setting.Lang);
                    break;
            }

            if (Setting.SaveLoginAndPassword == "True")
                cbSaveLogin.Checked = true;
            if (Setting.SkipIntro == "True")
                cbSkipIntro.Checked = true;
            if (Setting.HideSplashLogo == "True")
                cbHideSplashLogo.Checked = true;
        }

        private void PicButSetting_MouseEnter(object sender, EventArgs e)
        {
            PicButSetting.Image = Properties.Resources.But_Settings_Active;
        }

        private void PicButSetting_MouseLeave(object sender, EventArgs e)
        {
            PicButSetting.Image = Properties.Resources.But_Settings;
        }

        private void PicButExit_MouseEnter(object sender, EventArgs e)
        {
            PicButExit.Image = Properties.Resources.But_Power_Active;
        }

        private void PicButExit_MouseLeave(object sender, EventArgs e)
        {
            PicButExit.Image = Properties.Resources.But_Power;
        }

        private void PicButEnter_Click(object sender, EventArgs e)
        {
            if (Setting.PathToGame != "")
            {
                if (txtLogin.Text != "" && txtPassword.Text != "")
                {
                    byte[] data = Encoding.Default.GetBytes(txtPassword.Text);
                    var result = new SHA256Managed().ComputeHash(data);

                    string LoginArg = "-r +auth_ip "+ txtServerIP.Text + " -uid " + txtLogin.Text + " -token " + BitConverter.ToString(result).Replace("-", "").ToLower();
                    string HShield = " +acpxmk";

                    ProcessStartInfo GameClient = new ProcessStartInfo();

                    GameClient.FileName = Setting.PathToGame;
                    GameClient.Arguments = LoginArg + HShield;
                    try
                    {
                        Process.Start(GameClient);
                    }
                    catch {
                        MessageBox.Show("Ошибка: Проверьте указанный путь до клиента игры!");
                    }

                    } else
                {
                    MessageBox.Show("Логин и пароль должны быть заполнены!");
                }
            } else
            {
                MessageBox.Show("Не указан путь размещения клиента игры!");
            }
        }

        private void PicButSetting_Click(object sender, EventArgs e)
        {
            gbSettings.Visible = !gbSettings.Visible;
        }

        private void ButPathToGame_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                Setting.PathToGame = openFileDialog.FileName;
                txtPathToGame.Text = Setting.PathToGame;
                Console.WriteLine(openFileDialog.OpenFile());
            }
        }

        private void ButSettingSave_Click(object sender, EventArgs e)
        {
            Setting.PathToGame = txtPathToGame.Text;
            Setting.SaveLoginAndPassword = cbSaveLogin.Checked.ToString();
            Setting.ServerIpAddress = txtServerIP.Text;
            Setting.SkipIntro = cbSkipIntro.Checked.ToString();
            Setting.HideSplashLogo = cbHideSplashLogo.Checked.ToString();
            var SettingJson = JsonConvert.SerializeObject(Setting);
            Console.Write("Настройки:\n" + SettingJson);
            File.WriteAllText(Application.StartupPath + "\\" + "Launcher.Config", SettingJson);
            gbSettings.Visible = false;
        }

        private void ButSettingCancel_Click(object sender, EventArgs e)
        {
            gbSettings.Visible = false;
        }
    }
}
