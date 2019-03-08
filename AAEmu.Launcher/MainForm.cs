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
using System.Threading;
using System.Security.AccessControl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Net.Sockets;
using AA.Trion.Launcher;
using System.IO.MemoryMappedFiles;

namespace AAEmu.Launcher
{

    public partial class LauncherForm : Form
    {
        // I'm keeping the CreateProcess structs and imports here in case I'll need'm again later
        //------------------------------------------------------------------------------------------------------
        /*
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        public struct SECURITY_ATTRIBUTES
        {
            public int length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [DllImport("Kernel32.dll")]
        private static extern bool CreateProcess(
            string lpApplicationName, 
            string lpCommandLine, 
            IntPtr lpProcessAttributes, 
            IntPtr lpThreadAttributes, 
            bool bInheritHandles, 
            uint dwCreationFlags, 
            IntPtr lpEnvironment, 
            string lpCurrentDirectory, 
            ref STARTUPINFO lpStartupInfo, 
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("Kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);
        */

        /*
        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateFileMapping(
            IntPtr hFile,
            IntPtr lpFileMappingAttributes,
            uint flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            StringBuilder lpName);
            //[MarshalAs(UnmanagedType.LPStr)] string lpName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenFileMapping(int dwDesiredAccess,
            bool bInheritHandle, StringBuilder lpName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr MapViewOfFile(IntPtr hFileMapping,
            uint dwDesiredAccess, 
            uint dwFileOffsetHigh, 
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [DllImport("kernel32.dll")]
        private static extern bool UnmapViewOfFile(IntPtr map);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
        */

        public static int FILE_MAP_READ = 0x0004;
        public static uint INVALID_HANDLE_VALUE = 0xFFFFFFFF;
        public static uint PAGE_READWRITE = 0x04;
        public static uint FILE_MAP_ALL_ACCESS = 0x04;

        static int savedID1FileMap = -1 ;

        //------------------------------------------------------------------------------------------------------
        // Imports for using ToolsA.dll, currently required to be able to use Trion-style login authentication
        //------------------------------------------------------------------------------------------------------
        // [DllImport("ToolsA.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?_g3@@YA_NPAEH0HPAPAX1@Z")] // named entrypoint is ugly
        [DllImport("ToolsA.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#15")]
        public static extern bool generateHandlesWithToolsA(byte[] byte_0, int int_28, byte[] byte_1, int int_29, ref int uint_0, ref int uint_1);
        //------------------------------------------------------------------------------------------------------

        public partial class Settings
        {
            [JsonProperty("configName")]
            public string configName { get; set; }

            [JsonProperty("lang")]
            public string Lang { get; set; }

            [JsonProperty("launcherLang")]
            public string LauncherLang { get; set; }

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

            [JsonProperty("loginType")]
            public string ClientLoginType { get; set; }

            [JsonProperty("updateLocale")]
            public string UpdateLocale { get; set; }

            [JsonProperty("allowGameUpdates")]
            public string AllowGameUpdates { get; set; }

            [JsonProperty("serverGameUpdateURL")]
            public string ServerGameUpdateURL { get; set; }

            [JsonProperty("serverWebsiteURL")]
            public string ServerWebSiteURL { get; set; }

            [JsonProperty("serverNewsFeedURL")]
            public string ServerNewsFeedURL { get; set; }

            [JsonProperty("userHistory")]
            public List<string> UserHistory { get; set; }
        }

        public partial class ClientLookupHelper
        {
            [JsonProperty("serverName")]
            public List<string> serverNames { get; set; }

            [JsonProperty("clientLocation")]
            public List<string> clientLocations { get; set; }
        }

        // Some strings for our language settings
        private const string settingsLangEN_US = "en_us"; // English US
        private const string settingsLangRU = "ru"; // Russian
        private const string settingsLangDE = "de"; // German
        // unused for launcher
        private const string settingsLangFR = "fr"; // French
        // unused in launcher
        private const string settingsLangKR = "ko"; // Korean
        private const string settingsLangJP = "ja"; // Japanese
        private const string settingsLangZH_CN = "zh_cn"; // Chinese simplified
        private const string settingsLangZH_TW = "zh_tw"; // Chinese traditional // Not sure how I'd do this with flags if ever

        public partial class LanguageSettings
        {
            [JsonProperty("lang")]
            public string Lang { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }

            [JsonProperty("serveraddress")]
            public string ServerAddress { get; set; }

            [JsonProperty("pathtogame")]
            public string PathToGame { get; set; }

            [JsonProperty("savecredentials")]
            public string SaveCredentials { get; set; }

            [JsonProperty("skipintro")]
            public string SkipIntro { get; set; }

            [JsonProperty("hidesplashscreen")]
            public string HideSplashScreen { get; set; }

            [JsonProperty("saveSettings")]
            public string SaveSettings { get; set; }

            [JsonProperty("cancel")]
            public string Cancel { get; set; }

            [JsonProperty("settings")]
            public string Settings { get; set; }

            [JsonProperty("website")]
            public string Website { get; set; }

            [JsonProperty("play")]
            public string Play { get; set; }

            [JsonProperty("online")]
            public string Online { get; set; }

            [JsonProperty("offline")]
            public string Offline { get; set; }

            [JsonProperty("updatelocale")]
            public string UpdateLocale { get; set; }

            [JsonProperty("allowupdates")]
            public string AllowUpdates { get; set; }

            [JsonProperty("toolsafailed")]
            public string ToolsAFailed { get; set; }

            [JsonProperty("unknownlauncherprotocol")]
            public string UnknownLauncherProtocol { get; set; }

            [JsonProperty("nouserorpassword")]
            public string NoUserOrPassword { get; set; }

            [JsonProperty("missinggame")]
            public string MissingGame { get; set; }

            [JsonProperty("errorupdatingfile")]
            public string ErrorUpdatingFile { get; set; }

            [JsonProperty("minimize")]
            public string Minimize { get; set; }

            [JsonProperty("closeprogram")]
            public string CloseProgram { get; set; }
        }


        public Settings Setting = new Settings();
        public Settings RemoteSetting = new Settings();
        public static LanguageSettings L = new LanguageSettings();
        public ClientLookupHelper ClientLookup = new ClientLookupHelper();

        const string archeAgeEXE = "archeage.exe";
        const ushort defaultAuthPort = 1237 ;
        const string launcherDefaultConfigFile = "settings.aelcf"; // .aelcf = ArcheAge Emu Launcher Configuration File
        const string clientLookupDefaultFile = "clientslist.json";
        string launcherOpenedConfigFile = "";
        const string urlAAEmuGitHub = "https://github.com/atel0/AAEmu";
        const string urlLauncherGitHub = "https://github.com/ZeromusXYZ/AAEmu-Launcher";
        const string urlDiscordInvite = "https://discord.gg/vn8E8E6";
        const string urlNews = "https://aaemu.pw/updater/";
        const string urlWebsite = "https://aaemu.info/";
        // const string urlNews = "https://cl2.widgetbot.io/channels/479677351618281472/481782245087248400";
        const string dx9downloadURL = "https://www.microsoft.com/en-us/download/confirmation.aspx?id=35";


        // launcher protocol indentifiers
        private const string stringMailRu_1_0 = "mailru_1_0";
        private const string stringTrino_1_2 = "trino_1_2";

        // Stuff for dragable form
        private bool formMouseDown;
        private Point lastLocation;

        private int currentPanel = 0;
        private int nextServerCheck = -1;
        private int serverCheckStatus = 2; // 0 = Offline ; 1 = Online ; 2 = Unknown (not checked)
        private bool checkNews = false;
        AAEmuNewsFeed newsFeed = null ;
        private int bigNewsIndex = -1;
        private int bigNewsTimer = -1;

        public LauncherForm()
        {
            InitializeComponent();
        }

        private void InitDefaultLanguage()
        {
            L.Lang = settingsLangEN_US;
            btnLauncherLangChange.Image = Properties.Resources.flag_english;
            L.Username = "Username";
            L.Password = "Password";
            L.ServerAddress = "Server Address";
            L.PathToGame = "Path to Game";
            L.SaveCredentials = "Save Credentials";
            L.SkipIntro = "Skip Intro";
            L.HideSplashScreen = "Hide Splash Screen";
            L.SaveSettings = "Save";
            L.Cancel = "Cancel";
            L.Settings = "Settings";
            L.Website = "Website";
            L.Play = "Play";
            L.Online = "Online";
            L.Offline = "Offline";
            L.UpdateLocale = "Update locale";
            L.AllowUpdates = "Allow Updates";
            L.ToolsAFailed = "Failed to load ToolsA.DLL, you might have a debugger open, if so, please close it and restart the launcher";
            L.UnknownLauncherProtocol = "Unknown launcher protocol: {0}";
            L.NoUserOrPassword = "Please enter a username and password";
            L.MissingGame = "No valid game path set!";
            L.ErrorUpdatingFile = "ERROR updating {0}";
            L.Minimize = "Minimize";
            L.CloseProgram = "Close";
        }

        private void LoadLanguageFromFile(string languageID)
        {
            bool res = false;

            string lngFileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\lng\\" + languageID + ".lng";

            StreamReader reader = null;
            Console.WriteLine(lngFileName);
            try
            {
                reader = new StreamReader(lngFileName);
                var ConfigFile = reader.ReadToEnd();
                Console.Write(ConfigFile.ToString());

                L = JsonConvert.DeserializeObject<LanguageSettings>(ConfigFile);
                res = true;
            }
            catch
            {
                res = false;
            }
            finally
            {
                // Make sure we close our stream so the file won't be in use when we need to save it
                if (reader != null)
                {
                    reader.Close();
                }
            }
            if (res == false)
            {
                InitDefaultLanguage();
            }

        }

        private void SetCustomCheckBox(Label targetLabel, string checkState)
        {
            if (checkState == "True")
            {
                targetLabel.Text = "✓";
            }
            else
            {
                targetLabel.Text = " ";
            }
        }

        private string ToggleSettingCheckBox(Label targetLabel, string settingString)
        {
            string s = "x";
            if (settingString == "True")
            {
                s = "False";
            }
            else
            {
                s = "True";
            }

            SetCustomCheckBox(targetLabel, s);
            return s;
        }

        private void ShowPanelControls(byte panelID)
        {
            // The reason for doing things this way is because drawing reliable transparent stuff
            // onto a panel is hard as hell. It's easier to just put EVERYTHING on the form, and
            // Just show/hide what you need (swapping out background where needed)

            // 0: Main login "panel"
            panelLoginAndNews.Visible = (panelID == 0);
            panelLoginAndNews.Location = new Point(0, 0);
            panelLoginAndNews.Size = this.Size;

            cbLoginList.Visible = ((panelID == 0) && (cbLoginList.Items.Count > 0));
            lBigNewsImage.Visible = ((panelID == 0) && (lBigNewsImage.Tag != null) && (lBigNewsImage.Tag.ToString() != ""));
            wbNews.Visible = ((panelID == 0) && (wbNews.Tag != null) && (wbNews.Tag.ToString() == "1"));

            // 1: Settings "panel"
            panelSettings.Visible = (panelID == 1);
            panelSettings.Location = new Point(0, 0);
            panelSettings.Size = this.Size;

            // Gray out this setting if no update url is set
            if ((Setting.ServerGameUpdateURL != null) && (Setting.ServerGameUpdateURL != ""))
            {
                lAllowUpdates.ForeColor = Color.White;
                cbAllowUpdates.ForeColor = Color.White;
                cbAllowUpdates.Cursor = Cursors.Hand;
            }
            else
            {
                lAllowUpdates.ForeColor = Color.Gray;
                cbAllowUpdates.ForeColor = Color.Gray;
                cbAllowUpdates.Cursor = Cursors.No;
            }

            // If we don't change this, transparancy effects that aren't on the panels will show wrong because of gray background
            switch (panelID)
            {
                case 0:
                    BackgroundImage = Properties.Resources.bg_login;
                    break;
                case 1:
                    BackgroundImage = Properties.Resources.bg_setup;
                    break;
                default:
                    BackgroundImage = Properties.Resources.bg;
                    break;
            }
            

            currentPanel = panelID;
        }

        private void UpdateLauncherLanguage()
        {

            switch (Setting.LauncherLang)
            {
                // Add new launcher languages here
                case settingsLangRU:
                    btnLauncherLangChange.Image = Properties.Resources.flag_ru;
                    break;
                case settingsLangDE:
                    btnLauncherLangChange.Image = Properties.Resources.flag_de;
                    break;
                case settingsLangFR:
                    btnLauncherLangChange.Image = Properties.Resources.flag_fr;
                    break;
                case settingsLangEN_US:
                default:
                    Setting.LauncherLang = settingsLangEN_US;
                    btnLauncherLangChange.Image = Properties.Resources.flag_english;
                    break;
            }
            LoadLanguageFromFile(Setting.LauncherLang);

            lLogin.Text = L.Username ;
            lPassword.Text = L.Password ;
            lIPAddress.Text = L.ServerAddress;
            lPathToGameLabel.Text = L.PathToGame;
            lSaveUser.Text = L.SaveCredentials ;
            lSkipIntro.Text = L.SkipIntro;
            lHideSplash.Text = L.HideSplashScreen;
            lSettingsBack.Text = L.SaveSettings;
            btnSettings.Text = L.Settings;
            btnWebsite.Text = L.Website;
            lUpdateLocale.Text = L.UpdateLocale ;
            lAllowUpdates.Text = L.AllowUpdates;
            minimizeToolStripMenuItem.Text = L.Minimize;
            closeToolStripMenuItem.Text = L.CloseProgram;

            updatePlayButton(serverCheckStatus, false);

            btnLauncherLangChange.Refresh();
        }

        private void UpdateLocaleLanguage()
        {

            switch (Setting.Lang)
            {
                // Add new game languages here
                case settingsLangKR:
                    btnLocaleLang.Image = Properties.Resources.mini_locale_kr;
                    break;
                case settingsLangRU:
                    btnLocaleLang.Image = Properties.Resources.mini_locale_ru;
                    break;
                case settingsLangDE:
                    btnLocaleLang.Image = Properties.Resources.mini_locale_de;
                    break;
                case settingsLangFR:
                    btnLocaleLang.Image = Properties.Resources.mini_locale_fr;
                    break;
                case settingsLangEN_US:
                default:
                    Setting.Lang = settingsLangEN_US;
                    btnLocaleLang.Image = Properties.Resources.mini_locale_en_us;
                    break;
            }

            btnLocaleLang.Refresh();
        }


        private void PicButLangChange_Click(object sender, EventArgs e)
        {
            switch(Setting.LauncherLang)
            {
                case settingsLangRU:
                    Setting.LauncherLang = settingsLangEN_US;
                    break;
                case settingsLangEN_US:
                    Setting.LauncherLang = settingsLangDE;
                    break;
                case settingsLangDE:
                    Setting.LauncherLang = settingsLangRU;
                    break;
            }
            Console.WriteLine("Updating Language: {0}",Setting.LauncherLang);
            UpdateLauncherLanguage();
            btnLauncherLangChange.Refresh();
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
            cmsGitHub.Show(btnGithub, new Point(0, btnGithub.Height));
            // Process.Start(urlAAEmuGitHub);
        }

        private void PicButDiscord_Click(object sender, EventArgs e)
        {
            Process.Start(urlDiscordInvite);
        }

        private void LauncherForm_Load(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            InitDefaultLanguage();

            // Helps to keep the editing window cleaner
            imgBigNews.SizeMode = PictureBoxSizeMode.Normal;
            imgBigNews.Size = imgBigNews.Image.Size ;
            imgBigNews.Invalidate();

            string openCommandLineSettingsFile = "";
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                // No additional possible settings yet, only check if a argument is a valid file
                if (File.Exists(arg))
                {
                    openCommandLineSettingsFile = arg;
                }
            }

            LoadClientLookup();

            if (openCommandLineSettingsFile != "")
            {
                if (!LoadSettings(openCommandLineSettingsFile)) openCommandLineSettingsFile = "";
            }
            // Load local settings file if no external config specified, or if it failed to load
            if (openCommandLineSettingsFile == "")
            {
                LoadSettings(Application.StartupPath + "\\" + launcherDefaultConfigFile);
            }

            if ((Setting.PathToGame == null) || (Setting.PathToGame == "") || (File.Exists(Setting.PathToGame) == false))
            {
                Setting.PathToGame = "";
                TryAutoFindGameExe();
            }

            if ((Setting.PathToGame == "") || (File.Exists(Setting.PathToGame) == false))
            {
                // open settings if no valid game file
                ShowPanelControls(1);
            }
            else 
            {
                ShowPanelControls(0);
                if (eLogin.Text != "")
                {
                    ePassword.Focus();
                    ePassword.SelectionStart = 0;
                    ePassword.SelectionLength = 0;
                }
                else
                {
                    eLogin.Focus();
                    eLogin.SelectionStart = 0;
                    eLogin.SelectionLength = 0;
                }

            }

            if ((Setting.ServerNewsFeedURL != null) && (Setting.ServerNewsFeedURL != ""))
            {
                checkNews = true;
            }

            if ((Setting.ServerIpAddress != null) && (Setting.ServerIpAddress != ""))
            {
                nextServerCheck = 1000 * 1;
                // the next server check will put the wait cursor off
            }
            else
            {
                nextServerCheck = -1;
                Application.UseWaitCursor = false;
            }
        }

        private bool LoadClientLookup()
        {
            bool res = false;
            string configFileName = Application.StartupPath + "\\" + clientLookupDefaultFile;

            StreamReader reader = null;
            Console.WriteLine(configFileName);
            try
            {
                reader = new StreamReader(configFileName);
                var ConfigFile = reader.ReadToEnd();
                Console.Write(ConfigFile.ToString());

                ClientLookup = JsonConvert.DeserializeObject<ClientLookupHelper>(ConfigFile);
                res = true;
            }
            catch
            {
                ClientLookup.serverNames = new List<string>();
                ClientLookup.clientLocations = new List<string>();
            }
            finally
            {
                // Make sure we close our stream so the file won't be in use when we need to save it
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return res;
        }

        private bool LoadSettings(string configFileName)
        {
            bool res = false;

            StreamReader reader = null ;
            Console.WriteLine(configFileName);
            try
            {
                reader = new StreamReader(configFileName);
                var ConfigFile = reader.ReadToEnd();
                Console.Write(ConfigFile.ToString());

                Setting = JsonConvert.DeserializeObject<Settings>(ConfigFile);
                res = true;
                launcherOpenedConfigFile = configFileName;

                if (launcherOpenedConfigFile == Application.StartupPath + "\\" + launcherDefaultConfigFile)
                {
                    lLoadedConfig.Text = "";
                } else if ((Setting.configName == null) || (Setting.configName == ""))
                {
                    lLoadedConfig.Text = Path.GetFileNameWithoutExtension(launcherOpenedConfigFile);
                }
                else
                {
                    lLoadedConfig.Text = Setting.configName;
                }
            }
            catch
            {
                lLoadedConfig.Text = "";
                // If loading fails, just put in some defaults instead
                Setting.configName = ""; // Local setting doesn't display a name
                Setting.PathToGame = "";
                Setting.ServerIpAddress = "127.0.0.1";
                Setting.Lang = settingsLangEN_US;
                Setting.LauncherLang = settingsLangEN_US;
                Setting.SaveLoginAndPassword = "True";
                Setting.SkipIntro = "False";
                Setting.HideSplashLogo = "False";
                Setting.LastLoginUser = "test";
                Setting.LastLoginPass = "test";
                Setting.UserHistory = new List<string>();
                Setting.UserHistory.Clear();
                Setting.ClientLoginType = stringMailRu_1_0;
                Setting.UpdateLocale = "False";
                Setting.AllowGameUpdates = "False";
                Setting.ServerGameUpdateURL = ""; // Not allowed without a server settings file by default
                Setting.ServerWebSiteURL = urlWebsite; // default to aaemu.info
                Setting.ServerNewsFeedURL = ""; // Not implemented yet

            }
            finally
            {
                // Make sure we close our stream so the file won't be in use when we need to save it
                if (reader != null)
                {
                    reader.Close();
                }
            }

            lGamePath.Text = Setting.PathToGame;
            eServerIP.Text = Setting.ServerIpAddress;

            cbLoginList.Items.Clear();
            if (Setting.UserHistory != null)
            foreach (string s in Setting.UserHistory)
                if (s != "")
                    cbLoginList.Items.Add(s);

            eLogin.Text = Setting.LastLoginUser;
            ePassword.Text = Setting.LastLoginPass;

            if (((Setting.ServerGameUpdateURL == null)) || (Setting.ServerGameUpdateURL == ""))
            {
                Setting.AllowGameUpdates = "False";
            }

            updateGameClientTypeLabel();

            UpdateLauncherLanguage();
            UpdateLocaleLanguage();

            SetCustomCheckBox(cbSaveUser, Setting.SaveLoginAndPassword);
            SetCustomCheckBox(cbSkipIntro, Setting.SkipIntro);
            SetCustomCheckBox(cbHideSplash, Setting.HideSplashLogo);
            SetCustomCheckBox(cbUpdateLocale, Setting.UpdateLocale);
            SetCustomCheckBox(cbAllowUpdates, Setting.AllowGameUpdates);

            return res;
        }

        public static void EncryptFileMapData(IntPtr fileMapView, string ticketString) // TODO copy enc xml data to MMF
        {
            // var int_r = fileMapView.ToInt32();
            // var long_r = fileMapView.ToInt64();

            string stringForSignature = "dGVzdA==";

            var key = Encoding.ASCII.GetBytes(stringForSignature);
            //var key = BitConverter.GetBytes(fileMapView.ToInt64());

            var rc4encoder = new RC4(key);
            var xml = ticketString;
                //"123\n<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><authTicket version=\"1.2\"><storeToken>1</storeToken><password>test</password></authTicket>";
            //var xmlBytes = Encoding.ASCII.GetBytes(xml);
            var xmlBytes = Encoding.Unicode.GetBytes(xml);
            //File.WriteAllBytes("ticket-raw.txt", xmlBytes);
            var result = rc4encoder.Encode(xmlBytes, xmlBytes.Length);
            //File.WriteAllBytes("ticket-rc4.txt",result);

            var pointer = Marshal.AllocHGlobal(result.Length);
            Marshal.Copy(result, 0, pointer, result.Length);
            // TODO Marshal.FreeHGlobal(pointer);

            Win32.MemCpy(fileMapView, pointer, (uint)result.Length);
        }

        public static IntPtr CreateFileMappingHandle(string ticketString)
        {

            // MSDN Documentation says SECURITY_ATTRIBUTES needs to be set (not null) for child processes to be able to inherit the handle from CreateEvent
            Win32.SECURITY_ATTRIBUTES sa = new Win32.SECURITY_ATTRIBUTES
            {
                nLength = Marshal.SizeOf(typeof(Win32.SECURITY_ATTRIBUTES)),
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };

            IntPtr sa_pointer = Marshal.AllocHGlobal(sa.nLength);
            Marshal.StructureToPtr(sa, sa_pointer, false);

            var credentialFileMap = Win32.CreateFileMappingW(
                Win32.INVALID_HANDLE_VALUE,
                IntPtr.Zero, //sa_pointer,
                FileMapProtection.PageReadWrite,
                0,
                4096, // TODO: 0x20000 or 0x1000
                "archeage_auth_ticket_map");

            Marshal.FreeHGlobal(sa_pointer);

            if (credentialFileMap == IntPtr.Zero)
            {
                Console.WriteLine("Failed to create credential file mapping");
                return IntPtr.Zero;
            }

            var fileMapView = Win32.MapViewOfFile(credentialFileMap, FileMapAccess.FileMapAllAccessFull, 0, 0, 4096);

            if (fileMapView == IntPtr.Zero)
            {
                Console.WriteLine("Failed to create credential file mapping view");
                Win32.CloseHandle(credentialFileMap);
                return IntPtr.Zero;
            }

            EncryptFileMapData(fileMapView,ticketString);

            Win32.UnmapViewOfFile(fileMapView);
            return credentialFileMap;
        }

        public static bool generateHandlesForTrionLogin(string ticketString, string signatureString, ref int handle1, ref int handle2)
        {
            handle1 = 0x00000000;
            handle2 = 0x00000000;

            // edited with info from nikes's code sample

            var credentialFileMap = CreateFileMappingHandle(ticketString);
            if (credentialFileMap == IntPtr.Zero)
            {
                // TODO ...
                // Win32.CloseHandle(credentialEvent);
                return false;
            }


            // MSDN Documentation says SECURITY_ATTRIBUTES needs to be set (not null) for child processes to be able to inherit the handle from CreateEvent
            Win32.SECURITY_ATTRIBUTES sa = new Win32.SECURITY_ATTRIBUTES
            {
                nLength = Marshal.SizeOf(typeof(Win32.SECURITY_ATTRIBUTES)),
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };
            IntPtr sa_pointer = Marshal.AllocHGlobal(sa.nLength);
            Marshal.StructureToPtr(sa, sa_pointer, false);
            var credentialEvent = Win32.CreateEventW(sa_pointer, true, false, "archeage_auth_ticket_event");
            Marshal.FreeHGlobal(sa_pointer);

            if (credentialEvent == IntPtr.Zero)
            {
                Console.WriteLine("Failed to create credential event");
                return false;
            }


            handle1 = credentialFileMap.ToInt32();
            handle2 = credentialEvent.ToInt32();
            // var credentials = $"{credentialFileMap.ToInt32():x8}:{credentialEvent.ToInt32():x8}";


            /*
            string fullTicketData = signatureString + "\n" + ticketString;

            string encryptedTicket = RC4.Encrypt(signatureString, fullTicketData);

            char[] file_mapping_seed = new char[4096];

            uint sillySizeValue = (((0x00000678 >> 16) + 2) << 16);
            IntPtr credentialFileMapHandle = CreateFileMapping(INVALID_HANDLE_VALUE, IntPtr.Zero, PAGE_READWRITE, 0, sillySizeValue, null);

            if (credentialFileMapHandle == null)
            {
                MessageBox.Show("Failed to create credential file mapping");
                return false;
            }

            IntPtr fileMapViewPointer = MapViewOfFile(credentialFileMapHandle, FILE_MAP_ALL_ACCESS, 0, 0, 0);

            if (fileMapViewPointer == null)
            {
                CloseHandle(credentialFileMapHandle);
                MessageBox.Show("Failed to create credential file mapping view");
                return false;
            }

            char[] chars = encryptedTicket.ToCharArray();

            for (int i = 0; i < chars.Length;i++)
            {
                char c = chars[i];
                Marshal.WriteByte(fileMapViewPointer, i, (byte)c);
            }
            
            //encrypt_file_map_data(file_map_view, file_mapping_seed);
            UnmapViewOfFile(fileMapViewPointer);

            handle1 = (uint)fileMapViewPointer;
            handle2 = (uint)credentialFileMapHandle;
            */
            return true;
        }

        private static string CreateTrinoHandleIDs(string user, string pass)
        {
            byte[] data = Encoding.Default.GetBytes(pass);
            var passHash = new SHA256Managed().ComputeHash(data);

            int handleID1FileMap = 0;
            int handleID2Event = 0;
            bool genRes = false;

            string stringForSignature = "dGVzdA==";
            //string stringForSignature = "Signature 1:";

            string stringForTicket = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"yes\"?>";
            stringForTicket += "<authTicket version = \"1.2\">";
            stringForTicket += "<storeToken>1</storeToken>";
            stringForTicket += "<username>" + user + "</username>";
            stringForTicket += "<password>" + BitConverter.ToString(passHash).Replace("-", "").ToLower() + "</password>";
            stringForTicket += "</authTicket>";

            genRes = generateHandlesForTrionLogin(stringForTicket, stringForSignature, ref handleID1FileMap, ref handleID2Event);

            /*
            // Stuff to use ToolsA.DLL
            // Basically A complex way of doing stringForSignature + LineFeed + stringForTicket in a byte array
            byte[] bufferSignatureID1 = Encoding.UTF8.GetBytes(stringForSignature);
            byte[] bufferTicketID2 = Encoding.UTF8.GetBytes(stringForTicket);
            byte[] bufferTotal = new byte[(bufferTicketID2.Length + 1) + bufferSignatureID1.Length];
            Array.Copy(bufferSignatureID1, 0, bufferTotal, 0, bufferSignatureID1.Length);
            bufferTotal[bufferSignatureID1.Length] = 10;
            Array.Copy(bufferTicketID2, 0, bufferTotal, bufferSignatureID1.Length + 1, bufferTicketID2.Length);

            File.WriteAllBytes("ticket-raw.txt", bufferTotal);
            
            try
            {
                genRes = generateHandlesWithToolsA(bufferTotal, bufferTotal.Length, bufferSignatureID1, bufferSignatureID1.Length, ref handleID1FileMap, ref handleID2Event);
            }
            catch
            {
                MessageBox.Show(L.ToolsAFailed);
            }
            // End ToolsA.DLL stuff
            */

            savedID1FileMap = handleID1FileMap;
            //DumpMemFile(handleID1FileMap, "ticket-pre-1.txt");

            if (genRes == false)
            {
                //MessageBox.Show("Error generating login handle");
                return "00000000:00000000";
            }
            else
            {
                return handleID1FileMap.ToString("X8") + ":" + handleID2Event.ToString("X8");
            }

        }

        private string CreateArgsMailRU_1_0(string user, string pass, string serverIP, ushort serverPort)
        {
            byte[] data = Encoding.Default.GetBytes(pass);
            var passHash = new SHA256Managed().ComputeHash(data);

            string gameProviderArg, languageArg;
            switch(Setting.Lang)
            {
                case settingsLangRU:
                    gameProviderArg = "-r ";
                    languageArg = "";
                    break;
                case settingsLangFR:
                    gameProviderArg = "-r ";
                    languageArg = "";
                    break;
                case settingsLangDE:
                    gameProviderArg = "-r ";
                    languageArg = "";
                    break;
                case settingsLangEN_US:
                default:
                    gameProviderArg = "-r ";
                    languageArg = "";
                    break;
            }
            return gameProviderArg + "+auth_ip " + serverIP + ":" + serverPort.ToString() + " -uid " + eLogin.Text + " -token " + BitConverter.ToString(passHash).Replace("-", "").ToLower()+languageArg;
        }

        private string CreateArgsTrino_1_2(string user, string pass, string serverIP, ushort serverPort)
        {

            string gameProviderArg, languageArg;
            switch (Setting.Lang)
            {
                // This will likely need some tweaking in the future
                case settingsLangRU:
                case settingsLangFR:
                case settingsLangDE:
                case settingsLangEN_US:
                    gameProviderArg = "-t ";
                    languageArg = " -lang "+Setting.Lang ;
                    break;
                default:
                    gameProviderArg = "-t ";
                    languageArg = "";
                    break;
            }

            // string handleArgs = "-handle " + GetHandleIDs(user, pass);
            string handleArgs = "-handle " + CreateTrinoHandleIDs(user, pass);

            // archeage.exe -t -auth_ip 127.0.0.1 -auth_port 1237 -handle 00000000:00000000 -lang en_us
            return gameProviderArg + "+auth_ip " + serverIP + " -auth_port " + serverPort.ToString() + " " + handleArgs + languageArg;
        }

        private void StartGame()
        { 
            if (Setting.PathToGame != "")
            {
                if (eLogin.Text != "" && ePassword.Text != "")
                {

                    string serverIP = Setting.ServerIpAddress ;
                    ushort serverPort = defaultAuthPort;

                    var splitPos = eServerIP.Text.IndexOf(":");
                    if (splitPos >= 0)
                    {
                        serverIP = eServerIP.Text.Substring(0, splitPos);
                        if (!ushort.TryParse(eServerIP.Text.Substring(splitPos+1), out serverPort))
                        {
                            serverPort = defaultAuthPort;
                        }
                    }

                    // Mutex mutUser = new Mutex(false, "archeage_auth_ticket_event");
                    // mutex name might be: archeage_auth_ticket_event

                    if (Setting.SaveLoginAndPassword == "true")
                        SaveSettings();

                    UpdateGameSystemConfigFile((Setting.UpdateLocale == "True"),Setting.Lang, (Setting.SkipIntro == "True"));

                    string LoginArg = "";

                    switch(Setting.ClientLoginType)
                    {
                        case stringTrino_1_2:
                            // Trion style auth ticket with handles, generated by ToolsA.dll
                            LoginArg = CreateArgsTrino_1_2(eLogin.Text, ePassword.Text, serverIP, serverPort);
                            break;
                        case stringMailRu_1_0:
                            // Original style using uid and hashed password as token
                            LoginArg = CreateArgsMailRU_1_0(eLogin.Text, ePassword.Text, serverIP, serverPort);
                            break;
                        default:
                            MessageBox.Show(L.UnknownLauncherProtocol,Setting.ClientLoginType);
                            return;
                    }

                    if (Setting.HideSplashLogo == "True")
                    {
                        LoginArg += " -nosplash";
                    }

                    string HShield = " +acpxmk"; // client safely ignores this if no HackShield is present or disabled

                    if (debugModeToolStripMenuItem.Checked)
                    {
                        DebugHelperForm dlg = new DebugHelperForm();
                        dlg.eArgs.Text = LoginArg + " -devmode";
                        dlg.eHackShieldArg.Text = HShield;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            LoginArg = dlg.eArgs.Text;
                            HShield = dlg.eHackShieldArg.Text;
                        }
                        dlg.Dispose();
                    }


                    /*
                    // required for 1.2+
                    PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
                    STARTUPINFO si = new STARTUPINFO();
                    uint lerr = 0;

                    try
                    {

                        bool isCreated = CreateProcess(
                            Setting.PathToGame, // app
                            "\"" + Setting.PathToGame + "\" " + LoginArg + HShield, //cmdline
                            IntPtr.Zero, // ProcAttrib
                            IntPtr.Zero, // ThreadAttrib
                            true, // inherit handles
                            0x00000004, // create flags: 0x00000004 = CREATE_SUSPENDED ;
                            IntPtr.Zero, // envi (null = inherit)
                            null, // current dir (null = app's dir)
                            ref si, // start info
                            out pi // proc info
                            );

                        lerr = GetLastError();

                        if (isCreated == false)
                        {
                            if (lerr == 740) // 0x2E4
                            {
                                MessageBox.Show("Elevation required ! Please run the launcher with admin rights.");
                            }
                            else if (lerr != 0)
                            {
                                MessageBox.Show("Failed to create process, error: " + lerr.ToString());
                            }
                            return;
                        }

                        if (ResumeThread(pi.hThread) == -1)
                        {
                            lerr = GetLastError();
                            if (lerr != 0)
                            {
                                MessageBox.Show("Failed to resume thread, error: " + lerr.ToString());
                                return;
                            }
                        }


                        if (Setting.SaveLoginAndPassword == "true")
                            SaveSettings();

                        // Minimize after launching AA
                        WindowState = FormWindowState.Minimized;
                    }
                    catch {
                        MessageBox.Show("Error: Failed to start the game");
                        // MessageBox.Show("Ошибка: Проверьте указанный путь до клиента игры!");
                    }
                    */

                    // Loading using Process.Start();
                    ProcessStartInfo GameClientProcessInfo;
                    GameClientProcessInfo = new ProcessStartInfo(Setting.PathToGame, LoginArg + HShield);
                    GameClientProcessInfo.UseShellExecute = true;
                    GameClientProcessInfo.Verb = "runas";
                    bool startOK = false;
                    try
                    {
                        Process.Start(GameClientProcessInfo);
                        startOK = true;
                    } catch
                    {
                        startOK = false;
                    }

                    //DumpMemFile(savedID1FileMap, "ticket-post-1.txt");
                    //DumpMemFile(savedID2, "ticket-post-2.txt");

                    // Minimize after launching AA
                    if (startOK)
                    {
                        WindowState = FormWindowState.Minimized;
                    }

                }
                else
                {
                    MessageBox.Show(L.NoUserOrPassword);
                    // MessageBox.Show("Логин и пароль должны быть заполнены!");
                }
            } else
            {
                MessageBox.Show(L.MissingGame);
                // MessageBox.Show("Не указан путь размещения клиента игры!");
            }

        }

        private void ButSettingSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            ShowPanelControls(0); // Show Login
        }

        private void ButSettingCancel_Click(object sender, EventArgs e)
        {
            ShowPanelControls(0); // Show Login
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
                //lFakePassword.Hide();
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

        private void LauncherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
        }

        private void SaveClientLookups()
        {
            string configFileName = Application.StartupPath + "\\" + clientLookupDefaultFile;

            if ((Setting.configName != null) && (Setting.configName != ""))
            {
                string findExe = TryAutoFindFromLookup();
                if ((findExe == "") && File.Exists(Setting.PathToGame))
                {
                    ClientLookup.serverNames.Add(Setting.configName);
                    ClientLookup.clientLocations.Add(Setting.PathToGame);
                }
                else
                {
                    return;
                }
                var ClientLookupJson = JsonConvert.SerializeObject(ClientLookup);
                try
                {
                    File.WriteAllText(configFileName, ClientLookupJson);
                }
                catch
                {
                    Console.Write("Unable to save client lookup data to :\n" + launcherOpenedConfigFile);
                }
            }
        }

        private void SaveSettings()
        {
            Setting.PathToGame = lGamePath.Text;
            // Try saving lookups after we set the path
            SaveClientLookups();

            Setting.ServerIpAddress = eServerIP.Text;
            // Setting.SaveLoginAndPassword = cbSaveLogin.Checked.ToString();
            // Setting.SkipIntro = cbSkipIntro.Checked.ToString();
            // Setting.HideSplashLogo = cbHideSplashLogo.Checked.ToString();

            if (Setting.SaveLoginAndPassword == "True")
            {
                Setting.LastLoginUser = eLogin.Text;
                // TODO: Save the password in a somewhat more safe way
                Setting.LastLoginPass = ePassword.Text;
                // Setting.LastLoginPass = ""; // don't save password, unsafe
                if ((eLogin.Text != "") && (!cbLoginList.Items.Contains(eLogin.Text)))
                {
                    cbLoginList.Items.Add(eLogin.Text);
                }
            }
            else
            {
                Setting.LastLoginUser = "";
                Setting.LastLoginPass = "";
                cbLoginList.Items.Clear(); // Also delete history if we put off the save option
            }
            Setting.UserHistory = new List<string>();
            Setting.UserHistory.Clear();
            foreach(Object o in cbLoginList.Items)
                Setting.UserHistory.Add(cbLoginList.GetItemText(o));

            var SettingJson = JsonConvert.SerializeObject(Setting);

            if ((launcherOpenedConfigFile == null) || (launcherOpenedConfigFile == "") || (File.Exists(launcherOpenedConfigFile) == false))
            {
                launcherOpenedConfigFile = Application.StartupPath + "\\" + launcherDefaultConfigFile;
            }
            Console.Write("Saving Settings to "+ launcherOpenedConfigFile +" :\n" + SettingJson);
            try
            {
                File.WriteAllText(launcherOpenedConfigFile, SettingJson);
            }
            catch 
            {
                Console.Write("Unable to save settings to :\n" + launcherOpenedConfigFile);
            }
        }

        private void txtLoginList_SelectedValueChanged(object sender, EventArgs e)
        {
            eLogin.Text = cbLoginList.Text;
            //lFakeUser.Text = cbLoginList.Text;
            cbLoginList.Hide();
        }

        private void txtLogin_Leave(object sender, EventArgs e)
        {
            eLogin.Hide();
            //lFakeUser.Text = eLogin.Text;
            //lFakeUser.Show();
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            ePassword.Hide();
            //lFakePassword.Text = new String('●', ePassword.TextLength);
            //lFakePassword.Show();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            StartGame();
            Application.UseWaitCursor = false;
        }

        private void btnPlay_MouseEnter(object sender, EventArgs e)
        {
            updatePlayButton(serverCheckStatus, true);
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            updatePlayButton(serverCheckStatus, false);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowPanelControls(1); // Show settings
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            if ((Setting.ServerWebSiteURL != null) && (Setting.ServerWebSiteURL != ""))
            {
                Process.Start(Setting.ServerWebSiteURL);
            }
            else
            {
                Process.Start(urlWebsite);
            }
        }

        private void LauncherForm_BackgroundImageChanged(object sender, EventArgs e)
        {
            Invalidate(true);
        }

        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSystem_Click(object sender, EventArgs e)
        {
            cmsAAEmuButton.Show(btnSystem,new Point(0, btnSystem.Height));
        }

        private void lSettingsBack_Click(object sender, EventArgs e)
        {
            SaveSettings();
            ShowPanelControls(0);
            serverCheckStatus = 2;
            nextServerCheck = 1000;
            updatePlayButton(serverCheckStatus, false);
        }

        private void lGamePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (lGamePath.Text != "")
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(lGamePath.Text);
            }
            if (openFileDialog.InitialDirectory == "")
            {
                openFileDialog.InitialDirectory = "C:\\ArcheAge\\Working\\Bin32";
            }
            openFileDialog.Filter = "ArcheAge Game|"+ archeAgeEXE +"|Executeable|*.exe|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                Setting.PathToGame = openFileDialog.FileName;
                lGamePath.Text = Setting.PathToGame;
                Console.WriteLine(openFileDialog.OpenFile());
            }
        }

        private void cbHideSplash_Click(object sender, EventArgs e)
        {
            Setting.HideSplashLogo = ToggleSettingCheckBox(cbHideSplash, Setting.HideSplashLogo);
        }

        private void cbSkipIntro_Click(object sender, EventArgs e)
        {
            Setting.SkipIntro = ToggleSettingCheckBox(cbSkipIntro, Setting.SkipIntro);
        }

        private void cbSaveUser_Click(object sender, EventArgs e)
        {
            Setting.SaveLoginAndPassword = ToggleSettingCheckBox(cbSaveUser, Setting.SaveLoginAndPassword);
        }

        private void cbLoginList_SelectedIndexChanged(object sender, EventArgs e)
        {
            eLogin.Text = cbLoginList.Text;
            ePassword.Text = "";
        }

        private void cbUpdateLocale_Click(object sender, EventArgs e)
        {
            Setting.UpdateLocale = ToggleSettingCheckBox(cbUpdateLocale, Setting.UpdateLocale);
        }

        private void updateGameClientTypeLabel()
        {
            switch (Setting.ClientLoginType)
            {
                case stringMailRu_1_0:
                    lGameClientType.Text = "Mail.ru 1.0 Auth (-r)";
                    break;
                case stringTrino_1_2:
                    lGameClientType.Text = "Trion 1.2 Auth (-t)";
                    break;
                default:
                    lGameClientType.Text = "???: " + Setting.ClientLoginType ;
                    break;
            }
        }

        private void lGameClientType_Click(object sender, EventArgs e)
        {
            // toggle client types
            switch (Setting.ClientLoginType)
            {
                case stringMailRu_1_0:
                    Setting.ClientLoginType = stringTrino_1_2;
                    break;
                case stringTrino_1_2:
                default:
                    Setting.ClientLoginType = stringMailRu_1_0;
                    break;
            }

            updateGameClientTypeLabel();
        }

        private void UpdateGameSystemConfigFile(bool enableUpdateLocale, string locale, bool enableSkipIntro)
        {
            // C:\ArcheAge\Documents => UserHomeFolder\ArcheAge
            string configFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ArcheAge\\system.cfg";
            const string localeField = "locale = ";
            const string movieField = "login_first_movie = ";
            const string optionSoundField = "option_sound = ";



            List<string> lines = new List<string>();
            List<string> newLines = new List<string>();

            bool updatedLocale = false;
            bool updatedMovie = false;
            bool hasSoundOptionSetting = false;

            if (File.Exists(configFileName) == true)
            {
                lines = File.ReadAllLines(configFileName).ToList();

                foreach (string line in lines)
                {
                    if ((enableUpdateLocale == true) && (line.IndexOf(localeField) >= 0))
                    {
                        // replace here
                        if (updatedLocale == false)
                        {
                            newLines.Add(localeField + locale);
                        }
                        updatedLocale = true;
                    }
                    else
                    if ((enableSkipIntro == true) && (line.IndexOf(movieField) >= 0))
                    {
                        // replace here
                        if (updatedMovie == false)
                        {
                            newLines.Add(movieField + " 1");
                        }
                        updatedMovie = true;
                    }
                    else
                    {
                        newLines.Add(line);
                    }
                    if (line.IndexOf(optionSoundField) >= 0)
                    {
                        hasSoundOptionSetting = true;
                    }

                }
            }

            // Hack to make sure people can get past the server select on their first run
            // Apperantly missing the option_sound in the system.cfg will make it so the charater
            // select screen crashes
            if (hasSoundOptionSetting == false)
            {
                newLines.Add(optionSoundField + " 4");
            }

            // Add our settings if needed
            if ((enableSkipIntro == true) && (updatedMovie == false))
            {
                newLines.Add(movieField + " 1");
            }
            if ((enableUpdateLocale == true) && (updatedLocale == false))
            {
                newLines.Add(localeField + locale);
            }

            try
            {
                File.WriteAllLines(configFileName, newLines);
            } catch
            {
                MessageBox.Show(L.ErrorUpdatingFile,configFileName);
            }

        }

        private void aAEmuServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(urlAAEmuGitHub);
        }

        private void aAEmuLauncherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(urlLauncherGitHub);
        }

        private void cbAllowUpdates_Click(object sender, EventArgs e)
        {
            if ((Setting.ServerGameUpdateURL != null) && (Setting.ServerGameUpdateURL != ""))
            {
                Setting.AllowGameUpdates = ToggleSettingCheckBox(cbAllowUpdates, Setting.AllowGameUpdates);
            }
            else
            {
                Setting.AllowGameUpdates = "False";
                SetCustomCheckBox(cbAllowUpdates, Setting.AllowGameUpdates);
            }
        }

        private string TryAutoFindInDir(string dirName, string fileName,int depth)
        {
            pb1.PerformStep();
            pb1.Refresh();

            // Don't search too deep, 5 deep should be enough to be in range of what we want
            if (depth >= 5)
            {
                return "";
            }

            DirectoryInfo di = new DirectoryInfo(dirName);
            FileInfo[] files = di.GetFiles(fileName);
            foreach(FileInfo fi in files)
            {
                if (fi.Name.ToLower() == fileName.ToLower() && (File.Exists(dirName+fi.Name)))
                {
                    return dirName + fi.Name;
                }
            }

            DirectoryInfo[] dirs = di.GetDirectories();
            pb1.Maximum = pb1.Maximum + dirs.Length ;
            foreach (DirectoryInfo downDir in dirs)
            {
                string dirRes = TryAutoFindInDir(dirName + downDir.Name + "\\", fileName,depth+1);
                if (dirRes != "")
                {
                    return dirRes;
                }
            }

            return "";
        }

        private string TryAutoFindFromLookup()
        {
            if (ClientLookup.serverNames.Count != ClientLookup.clientLocations.Count)
            {
                Console.WriteLine("Error in client lookup file, array size mismatch ! Clearing settings");
                ClientLookup.serverNames.Clear();
                ClientLookup.clientLocations.Clear();
            }
            for (int i = 0;i < ClientLookup.serverNames.Count();i++)
            {
                if (ClientLookup.serverNames[i] == Setting.configName)
                {
                    return ClientLookup.clientLocations[i];
                }
            }
            return "";
        }

        private void TryAutoFindGameExe()
        {
            Application.UseWaitCursor = true;

            string exeFile = "";

            if ((Setting.configName != null) && (Setting.configName != ""))
            {
                // If we loaded a named config, try to pick from a saved list first
                exeFile = TryAutoFindFromLookup();
                if (exeFile != "")
                {
                    Setting.PathToGame = exeFile;
                    lGamePath.Text = Setting.PathToGame;
                    Application.UseWaitCursor = false;
                    return;
                }
            }

            string configPath = Path.GetDirectoryName(launcherOpenedConfigFile);
            // Yes I know this trim looks silly, but it's to prevent stuff like "C:\\\\directory\\pathtogame.exe"
            if (configPath == "")
            {
                configPath = Path.GetDirectoryName(Application.ExecutablePath);
            }
            configPath = configPath.TrimEnd('\\') + "\\" ;

            pb1.Visible = true;
            pb1.Minimum = 0;
            pb1.Maximum = 2;
            pb1.Value = 0;
            pb1.Step = 1;

            // Let's put a big old try/catch around this to prevent any file system shananigans
            try
            {
                exeFile = TryAutoFindInDir(configPath, archeAgeEXE, 0);
                if ((exeFile != "") && (File.Exists(exeFile)))
                {
                    Setting.PathToGame = exeFile;
                    lGamePath.Text = Setting.PathToGame;
                }
            }
            catch
            {
                // actually do nothing with this, user doesn't need to know
            }
            pb1.Visible = false;
            Application.UseWaitCursor = false;
        }

        private void updatePlayButton(int serverState, bool isMouseOver)
        {

            if (isMouseOver == true)
            {
                switch (serverState)
                {
                    case 0: // offline
                        btnPlay.Image = Properties.Resources.btn_red;
                        btnPlay.Text = L.Offline;
                        break;
                    case 1:
                        btnPlay.Image = Properties.Resources.btn_green_a;
                        btnPlay.Text = L.Play ;
                        break;
                    case 2:
                    default:
                        btnPlay.Image = Properties.Resources.btn_green;
                        btnPlay.Text = L.Play ;
                        break;
                }
            }
            else
            {
                switch (serverState)
                {
                    case 0: // offline
                        btnPlay.Image = Properties.Resources.btn_red;
                        btnPlay.Text = L.Offline ;
                        break;
                    case 1:
                        btnPlay.Image = Properties.Resources.btn_green;
                        btnPlay.Text = L.Play ;
                        break;
                    case 2:
                    default:
                        btnPlay.Image = Properties.Resources.btn_green_d;
                        btnPlay.Text = L.Play ;
                        break;
                }

            }

        }

        private void timerGeneral_Tick(object sender, EventArgs e)
        {
            if (checkNews == true)
            {
                checkNews = false;
                lNewsFeed.Text = "Server News\n\n\n\nLoading ...";

                try
                {
                    string newsString = WebHelper.SimpleGetURIAsString(Setting.ServerNewsFeedURL);
                    CreateNewsFeedFromJSON(newsString);
                } catch
                {
                    lNewsFeed.Text = "Server News\n\n\n\nLoad Failed !";
                }
            }

            if (nextServerCheck > 0)
            {
                /*
                if (pb2.Maximum < nextServerCheck)
                {
                    pb2.Maximum = nextServerCheck;
                }
                pb2.Value = nextServerCheck;
                */

                nextServerCheck -= timerGeneral.Interval;
                if (nextServerCheck <= 0)
                {
                    nextServerCheck += 1000 * 60 * 1 ; // 1 minute
                    checkServerStatus();

                    updatePlayButton(serverCheckStatus, false);
                }
            }

            if ((bigNewsTimer > 0) && (currentPanel == 0)) // Only count timer on the main "panel"
            {
                bigNewsTimer -= timerGeneral.Interval;
                if (bigNewsTimer <= 0)
                {
                    bigNewsTimer += 1000 * 10 * 1; // 1 minute

                    bigNewsIndex++;
                    if (bigNewsIndex >= newsFeed.data.Count)
                    {
                        bigNewsIndex = 0;
                    }

                    LoadBigNews(newsFeed.data[bigNewsIndex]);
                }
            }


        }

        private void checkServerStatus()
        {
            if ((Setting.ServerIpAddress == null) || (Setting.ServerIpAddress == ""))
            {
                serverCheckStatus = 2;
                return;
            }

            TcpClient testCon = null;
            Application.UseWaitCursor = true;
            try
            {

                string serverIP = Setting.ServerIpAddress;
                ushort serverPort = defaultAuthPort;

                var splitPos = eServerIP.Text.IndexOf(":");
                if (splitPos >= 0)
                {
                    serverIP = eServerIP.Text.Substring(0, splitPos);
                    if (!ushort.TryParse(eServerIP.Text.Substring(splitPos + 1), out serverPort))
                    {
                        serverPort = defaultAuthPort;
                    }
                }

                testCon = new TcpClientWithTimeout(serverIP, serverPort, 5000).Connect();
                if (testCon.Connected)
                {
                    serverCheckStatus = 1;
                    nextServerCheck = (1000 * 60 * 2); // check every 2 minutes when connected

                }
            }
            catch
            {
                serverCheckStatus = 0;
                nextServerCheck = (1000 * 60 * 5); // check every 5 minutes when offline
            }
            finally
            {
                if (testCon != null)
                {
                    if (testCon.Connected == true)
                    {
                        testCon.Close();
                    }
                    testCon.Dispose();
                }
            }
            Application.UseWaitCursor = false;

        }

        private void btnLauncherLangChange_Click(object sender, EventArgs e)
        {
            foreach(ToolStripMenuItem mi in cmsLauncherLanguage.Items)
            {
                mi.Enabled = (mi.Tag.ToString() != Setting.LauncherLang);
            }
            cmsLauncherLanguage.Show(btnLauncherLangChange, new Point(0, btnLauncherLangChange.Height));
        }

        private void swapLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting.LauncherLang = ((ToolStripMenuItem)sender).Tag.ToString() ;
            Console.WriteLine("Updating Language: {0}", Setting.LauncherLang);
            UpdateLauncherLanguage();
            btnLauncherLangChange.Refresh();
        }

        private void btnSystem_DoubleClick(object sender, EventArgs e)
        {
            if (debugModeToolStripMenuItem.Visible == false)
            {
                debugModeToolStripMenuItem.Checked = true;
                debugModeToolStripMenuItem.Visible = true;
            }
        }

        private void debugModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            debugModeToolStripMenuItem.Checked = !debugModeToolStripMenuItem.Checked ;
        }

        private void btnLocaleLang_Click(object sender, EventArgs e)
        {
            foreach (ToolStripItem tsi in cmsLocaleLanguage.Items)
            {
                if (tsi is ToolStripMenuItem)
                {
                    ToolStripMenuItem mi = (ToolStripMenuItem)tsi;
                    {
                        mi.Enabled = (mi.Tag.ToString() != Setting.Lang);
                    }
                }
            }
            cmsLocaleLanguage.Show(btnLocaleLang, new Point(0, btnLocaleLang.Height));
        }

        private void miLocaleLanguageChange_Click(object sender, EventArgs e)
        {
            Setting.Lang = ((ToolStripMenuItem)sender).Tag.ToString();
            Console.WriteLine("Updating Locale Language: {0}", Setting.Lang);
            UpdateLocaleLanguage();
            btnLocaleLang.Refresh();
        }

        private static void DumpMemFile(int handle, string fname)
        {
            try
            {
                IntPtr testMemPtr = Win32.MapViewOfFile((IntPtr)handle, FileMapAccess.FileMapRead, 0, 0, 0);
                string s = Marshal.PtrToStringUni(testMemPtr);
                Win32.UnmapViewOfFile(testMemPtr);
                File.WriteAllText(fname, s, Encoding.UTF8);
            }
            catch
            {
                MessageBox.Show("Error creating dump for " + fname);
            }
        }

        private void CreateNewsFeedFromJSON(string JSONString)
        {

            try
            {
                newsFeed = JsonConvert.DeserializeObject<AAEmuNewsFeed>(JSONString);
            }
            catch
            {
                lNewsFeed.Text = "Load Failed\n\nFailed to load news";
                wbNews.Hide();
                wbNews.Tag = "0";
                return;
            }
            finally
            {

            }
            if (newsFeed == null)
            {
                wbNews.Hide();
                wbNews.Tag = "0";
                return;
            }

            string wns = "";
            wns += "<html>";
            wns += "<style>";
            wns += "body { ";
            wns += "  background-attachment: fixed;";
            wns += "  background-repeat: no-repeat;";
            wns += "}";
            wns += "</style>";
            wns += "<body bgcolor=\"#000000\" text=\"#FFFFFF\"  link=\"#EEEEFF\"  vlink=\"#FFEEFF\" ";
            wns += "background =\"data:image/png;base64," + Properties.Resources.bgnews_b64 + "\">";

            foreach (AAEmuNewsFeedDataItem i in newsFeed.data)
            {
                wns += "<p align=\"center\">";
                if (i.itemAttributes.itemIsNew == "1")
                {
                    wns += "*new* ";
                }
                wns += "<a href=\"" + i.itemAttributes.itemLinks.self + "\" target=\"_new\">";
                wns += i.itemAttributes.itemTitle;
                wns += "</a><br>";
                wns += "<div align=\"left\"><font size=\"1\">";
                wns += i.itemAttributes.itemBody.Replace("\\r", "").Replace("\\n", "<br>");
                wns += "</font></div></p>";
            }
            // wns += "<p align=\"center\"><a href=\"testlink\" target=\"_new\">Test</a></p>";
            wns += "</body></html>";
            wbNews.DocumentText = wns;
            wbNews.Refresh();
            // File.WriteAllText("newscache.htm",wns);

            if ((newsFeed.data != null) && (newsFeed.data[0].itemAttributes.itemPicture != null) && (newsFeed.data[0].itemAttributes.itemPicture != ""))
            {
                LoadBigNews(newsFeed.data[0]);
                bigNewsTimer = 1000 * 10 * 1; // 10 seconds
            }
        }

        private Image LoadImageForNews(AAEmuNewsFeedDataItem newsItem)
        {
            string cacheFolder = Application.LocalUserAppDataPath + "\\data";
            Directory.CreateDirectory(cacheFolder);
            string cacheFileName = cacheFolder + "\\img -" + Setting.configName.Replace("@","_").Replace("/","_").Replace("\\","_").Replace("|","_") + "-" + newsItem.itemID + ".bin";

            MemoryStream imageData;
            Image img = null;
            if (File.Exists(cacheFileName) == true)
            {
                FileStream fs = new FileStream(cacheFileName, FileMode.Open);
                imageData = new MemoryStream();
                fs.CopyTo(imageData);
                fs.Dispose();
                img = Image.FromStream(imageData);
            }
            else
            {
                try
                {
                    imageData = WebHelper.SimpleGetURIAsMemoryStream(newsItem.itemAttributes.itemPicture);
                    FileStream fs = new FileStream(cacheFileName, FileMode.Create);
                    imageData.WriteTo(fs);
                    fs.Flush();
                    fs.Dispose();
                    img = Image.FromStream(imageData);
                }
                catch
                {
                    Console.WriteLine("Error loading " + newsItem.itemAttributes.itemPicture + " into " + cacheFileName);
                }

            }
            return img;
        }

        private void LoadBigNews(AAEmuNewsFeedDataItem newsItem)
        {
            Application.UseWaitCursor = true;
            try
            {
                var img = LoadImageForNews(newsItem);
                if (img != null)
                {
                    imgBigNews.Image = img;
                    lBigNewsImage.Text = newsItem.itemAttributes.itemTitle;
                    lBigNewsImage.Tag = newsItem.itemAttributes.itemLinks.self;
                    lBigNewsImage.Visible = (currentPanel == 0);
                }
                else
                {
                    lBigNewsImage.Tag = "";
                    lBigNewsImage.Visible = false;
                    imgBigNews.Image = Properties.Resources.bignews_default ;
                    imgBigNews.Visible = (currentPanel == 0);
                }
            }
            catch
            {
                lBigNewsImage.Tag = "";
                lBigNewsImage.Visible = false;
                imgBigNews.Image = Properties.Resources.bignews_default;
                imgBigNews.Visible = (currentPanel == 0);
            }
            Application.UseWaitCursor = false;
        }

        private void wbNews_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            wbNews.Visible = (currentPanel == 0);
            wbNews.Tag = "1"; // We use this it indicate if we need to show/hide the browser when swapping panels
        }

        private void lBigNewsImage_Click(object sender, EventArgs e)
        {
            if ((lBigNewsImage.Tag != null) && (lBigNewsImage.Tag.ToString() != ""))
            {
                Process.Start(lBigNewsImage.Tag.ToString());
            }
        }
    }
}
