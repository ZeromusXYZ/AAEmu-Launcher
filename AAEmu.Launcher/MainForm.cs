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
using AAPakEditor;
// using XLPakTool;


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
        private static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("Kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);

        [DllImport("Kernel32.dll")]
        private static extern uint GetLastError();
        //------------------------------------------------------------------------------------------------------
        */

        //------------------------------------------------------------------------------------------------------
        // Imports for using ToolsA.dll, currently required to be able to use Trion-style login authentication
        //------------------------------------------------------------------------------------------------------
        // [DllImport("ToolsA.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "?_g3@@YA_NPAEH0HPAPAX1@Z")] // named entrypoint is ugly
        [DllImport("ToolsA.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#15")]
        public static extern bool generateInitStr(byte[] byte_0, int int_28, byte[] byte_1, int int_29, ref uint uint_0, ref uint uint_1);
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

            [JsonProperty("update")]
            public string Update { get; set; }

            [JsonProperty("updating")]
            public string Updating { get; set; }

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

            [JsonProperty("noupdateurl")]
            public string NoUpdateURL { get; set; }

            [JsonProperty("checkversion")]
            public string CheckVersion { get; set; }

            [JsonProperty("downloadpatch")]
            public string DownloadPatch { get; set; }

            [JsonProperty("downloaderror")]
            public string DownloadError { get; set; }

            [JsonProperty("applypatch")]
            public string ApplyPatch { get; set; }

            [JsonProperty("applypatcherror")]
            public string ApplyPatchError { get; set; }

            [JsonProperty("patchcomplete")]
            public string PatchComplete { get; set; }

            [JsonProperty("failedcreatedirectory")]
            public string FailedCreateDirectory { get; set; }

            [JsonProperty("unsupportedpatchtype")]
            public string UnsupportedPatchType { get;set;}

            [JsonProperty("errorreadinglocalversioninformation")]
            public string ErrorReadingLocalVersionInformation { get; set; }

            [JsonProperty("nothingtoupdate")]
            public string NothingToUpdate { get; set; }

            [JsonProperty("failedtoopen")]
            public string FailedToOpen { get; set; }

            [JsonProperty("filenotfound")]
            public string FileNotFound { get; set; }

            [JsonProperty("patchhashmismatch")]
            public string PatchHashMismatch { get; set; }

            [JsonProperty("nofilestoupdate")]
            public string NoFilesToUpdate { get; set; }

            [JsonProperty("errorcreatingpatchcache")]
            public string ErrorCreatingPatchCache { get; set; }

            [JsonProperty("failedtoopenpatchcache")]
            public string FailedToOpenPatchCache { get; set; }

            [JsonProperty("fatalerrorfailedtoopenfileforwrite")]
            public string FatalErrorFailedToOpenFileForWrite { get; set; }

            [JsonProperty("downloadhashmismatch")]
            public string DownloadHashMismatch { get; set; }

            [JsonProperty("failedtosavecache")]
            public string FailedToSaveCache { get; set; }

            [JsonProperty("downloadfileerror")]
            public string DownloadFileError { get; set; }

            [JsonProperty("errorpatchapplyfile")]
            public string ErrorPatchApplyFile { get; set; }

            [JsonProperty("errorpatchexportfile")]
            public string ErrorPatchExportFile { get; set; }

            [JsonProperty("errorpatchexportdb")]
            public string ErrorPatchExportDB { get; set; }

            [JsonProperty("initialization")]
            public string Initialization { get; set; }

            [JsonProperty("downloadverfile")]
            public string DownloadVerFile { get; set; }

            [JsonProperty("comparingversion")]
            public string ComparingVersion { get; set; }

            [JsonProperty("checklocalfiles")]
            public string CheckLocalFiles { get; set; }

            [JsonProperty("rehashlocalfiles")]
            public string ReHashLocalFiles { get; set; }

            [JsonProperty("downloadpatchfilesinfo")]
            public string DownloadPatchFilesInfo { get; set; }

            [JsonProperty("calculatedownloads")]
            public string CalculateDownloads { get; set; }

            [JsonProperty("downloadfiles")]
            public string DownloadFiles { get; set; }

            [JsonProperty("addfiles")]
            public string AddFiles { get; set; }

            [JsonProperty("patchdone")]
            public string PatchDone { get; set; }

            [JsonProperty("gamepakneedsupdate")]
            public string Game_PakNeedsUpdate { get; set; }

            [JsonProperty("downloadsizeandfiles")]
            public string DownloadSizeAndFiles { get; set; }

            [JsonProperty("errorsavingversioninfo")]
            public string ErrorSavingVersionInfo { get; set; }

            [JsonProperty("servernewsloading")]
            public string ServerNewsLoading { get; set; }

            [JsonProperty("servernews")]
            public string ServerNews { get; set; }

            [JsonProperty("servernewsfailed")]
            public string ServerNewsFailed { get; set; }

            [JsonProperty("deletegamesettings")]
            public string DeleteGameSettings { get; set; }

            [JsonProperty("deletegamesettingstitle")]
            public string DeleteGameSettingsTitle { get; set; }

            [JsonProperty("deleteshadercache")]
            public string DeleteShaderCache { get; set; }

            [JsonProperty("deleteshadercachetitle")]
            public string DeleteShaderCacheTitle { get; set; }

            [JsonProperty("downloadsizemismatch")]
            public string DownloadSizeMismatch { get; set; }

            [JsonProperty("troubleshoot")]
            public string TroubleShoot { get; set; }

            [JsonProperty("tsdebugmode")]
            public string TSDebugMode { get; set; }

            [JsonProperty("tsdeleteshadercache")]
            public string TSDeleteShaderCache { get; set; }

            [JsonProperty("tsdeletegamesetting")]
            public string TSDeleteGameSetting { get; set; }

            [JsonProperty("tsdeleteall")]
            public string TSDeleteAll { get; set; }

            [JsonProperty("tsforcepatch")]
            public string TSForcePatch { get; set; }

            [JsonProperty("tsforcenopatch")]
            public string TSForceNoPatch { get; set; }
        }


        public Settings Setting = new Settings();
        public Settings RemoteSetting = new Settings();
        public static LanguageSettings L = new LanguageSettings();
        public ClientLookupHelper ClientLookup = new ClientLookupHelper();

        const string archeAgeEXE = "archeage.exe";
        const string archeAgeSystemConfigFileName = "system.cfg";
        const ushort defaultAuthPort = 1237 ;
        const string launcherDefaultConfigFile = "settings.aelcf"; // .aelcf = ArcheAge Emu Launcher Configuration File
        const string clientLookupDefaultFile = "clientslist.json";
        string localPatchFolderName = ".patch" + Path.DirectorySeparatorChar;
        const string remotePatchFolderURI = ".patch/" ;
        const string patchListFileName = "patchfiles.csv";
        const string patchVersionFileName = "patchfiles.ver";
        const string localPatchPakFileName = "download.patch";
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
        enum serverCheck { Offline = 0, Online, Unknown, Update, Updating };
        private serverCheck serverCheckStatus = serverCheck.Unknown;
        private bool checkNews = false;
        AAEmuNewsFeed newsFeed = null ;
        private int bigNewsIndex = -1;
        private int bigNewsTimer = -1;

        private AAPatchProgress aaPatcher = new AAPatchProgress();
        private AAPak pak = null;
        private AAPak PatchDownloadPak = null;
        private List<AAPakFileInfo> dlPakFileList = new List<AAPakFileInfo>();

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
            L.Update = "Update!";
            L.Updating = "Updating";
            L.UpdateLocale = "Update locale";
            L.AllowUpdates = "Allow Updates";
            L.ToolsAFailed = "Failed to load ToolsA.DLL, you might have a debugger open, if so, please close it and restart the launcher";
            L.UnknownLauncherProtocol = "Unknown launcher protocol: {0}";
            L.NoUserOrPassword = "Please enter a username and password";
            L.MissingGame = "No valid game path set!";
            L.ErrorUpdatingFile = "ERROR updating {0}";
            L.Minimize = "Minimize";
            L.CloseProgram = "Close";
            L.NoUpdateURL = "No update information provided, please start the launcher with a configuration file from the server you are trying to play on.";
            L.CheckVersion = "Checking version information.";
            L.DownloadPatch = "Downloading patch files";
            L.DownloadError = "Error downloading patch files!";
            L.ApplyPatch = "Applying patch";
            L.ApplyPatchError = "Error while patching game files!";
            L.PatchComplete = "Done patching files.";

            L.ServerNewsLoading = "Server News\n\n\n\nLoading ...";
            L.ServerNews = "Server News";
            L.ServerNewsFailed = "Server News\n\n\n\nLoad Failed !";

            L.FailedCreateDirectory = "Failed to create directory: {0}";
            L.UnsupportedPatchType = "Unsupported patch system: Version {0}";
            L.ErrorReadingLocalVersionInformation = "Error loading local Version Information";
            L.NothingToUpdate = "Nothing to update, same version";
            L.NoFilesToUpdate = "No files need to be updated";
            L.FailedToOpen = "Failed to open: {0}";
            L.FileNotFound = "File not found: {0}";
            L.PatchHashMismatch = "Downloaded patch file information does not seem to match the server version information, aborting update !\r\nGot: {0} \r\nExpected: {1}\r\\nPossible corrupted or incomplete download\"";
            L.ErrorCreatingPatchCache = "Error creating patch cache: {0}";
            L.FailedToOpen = "Failed to open patch cache, might be corrupted !\r\nTrying again with a clean cache.";
            L.FatalErrorFailedToOpenFileForWrite = "Fatal Error: unable to open patch cache for writing, please check permissions !";
            L.DownloadSizeMismatch = "Downloaded size does not match\r\n{0}\r\nExpected: {1}\r\nGot: {2}";
            L.DownloadHashMismatch = "Hash of downloaded file does not match\r\n{0}\r\nExpected: {1}\r\nGot: {2}";
            L.FailedToSaveCache = "Failed to save download cache:\r\n{0}";
            L.DownloadFileError = "Error downloading: {0}";
            L.ErrorPatchApplyFile = "Error applying patch file to game client:\r\n{0}";
            L.ErrorPatchExportFile = "Error exporting patch file to game client:\r\n{0}";
            L.ErrorPatchExportDB = "Error exporting database to game client:\r\n{0}";
            L.Initialization = "Initialization";
            L.DownloadVerFile = "Download version information";
            L.ComparingVersion = "Comparing version";
            L.CheckLocalFiles = "Checking local game client files";
            L.ReHashLocalFiles = "Initialize game client for update";
            L.DownloadPatchFilesInfo = "Downloading patch file information";
            L.CalculateDownloads = "Calculating download information";
            L.DownloadFiles = "Download patch files";
            L.AddFiles = "Update game client files";
            L.PatchDone = "Completed";
            L.Game_PakNeedsUpdate = "game_pak needs to be updated, this may take a long time";
            L.DownloadSizeAndFiles = "Download {0} MB in {1} file(s)";
            L.ErrorSavingVersionInfo = "Error saving version information: {0}";
            L.DeleteGameSettings = "Are you sure you want to delete the game settings file ?";
            L.DeleteGameSettingsTitle = "Delete system.cfg ?";
            L.DeleteShaderCache = "Are you sure you want to delete the shader cache ?";
            L.DeleteShaderCacheTitle = "Delete Shader Cache";

            L.TroubleShoot = "Troubleshoot";
            L.TSDebugMode = "Debug Mode";
            L.TSDeleteShaderCache = "Delete Shader Cache";
            L.TSDeleteGameSetting = "Delete Game Settings";
            L.TSDeleteAll = "Delete all ArcheAge settings";
            L.TSForcePatch = "Force Patch Download";
            L.TSForceNoPatch = "Force Skip Patch";
        }

        private void LoadLanguageFromFile(string languageID)
        {
            bool res = false;

            string lngFileName = Path.GetDirectoryName(Application.ExecutablePath) + Path.DirectorySeparatorChar + "lng" + Path.DirectorySeparatorChar + languageID + ".lng";

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

            // 0: Main login "panel"  and  2: Update/Patch "panel"
            panelLoginAndPatch.Visible = ((panelID == 0) || (panelID == 2));
            panelLoginAndPatch.Location = new Point(0, 0);
            panelLoginAndPatch.Size = this.Size;
            eLogin.Visible = (panelID == 0);
            ePassword.Visible = (panelID == 0);
            lLogin.Visible = (panelID == 0);
            lPassword.Visible = (panelID == 0);
            lNewsFeed.Visible = ((panelID == 0) || (panelID == 2));
            imgBigNews.Visible = (panelID == 0);
            cbLoginList.Visible = (cbLoginList.Items.Count > 0);
            lBigNewsImage.Visible = ((panelID == 0) && (lBigNewsImage.Tag != null) && (lBigNewsImage.Tag.ToString() != ""));
            wbNews.Visible = (((panelID == 0) || (panelID == 2)) && (wbNews.Tag != null) && (wbNews.Tag.ToString() == "1"));
            lPatchProgressBarText.Visible = (panelID == 2);
            pgbBackTotal.Visible = (panelID == 2);
            pgbFrontTotal.Visible = (panelID == 2);
            pPatchSteps.Visible = (panelID == 2);

            // 1: Settings "panel"
            panelSettings.Visible = (panelID == 1);
            panelSettings.Location = new Point(0, 0);
            panelSettings.Size = this.Size;
            eServerIP.Enabled = (serverCheckStatus != serverCheck.Updating);

            if (serverCheckStatus == serverCheck.Updating)
            {
                lGamePath.Cursor = Cursors.No;
            }
            else
            {
                lGamePath.Cursor = Cursors.Hand;
            }


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
                    panelLoginAndPatch.BackgroundImage = Properties.Resources.bg_login;
                    break;
                case 1:
                    BackgroundImage = Properties.Resources.bg_setup;
                    break;
                case 2:
                    BackgroundImage = Properties.Resources.bg_patch;
                    panelLoginAndPatch.BackgroundImage = Properties.Resources.bg_patch;
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

            rbInit.Text = L.Initialization ;
            rbDownloadVerFile.Text = L.DownloadVerFile;
            rbComparingVersion.Text = L.ComparingVersion;
            rbCheckLocalFiles.Text = L.CheckLocalFiles;
            rbReHashLocalFiles.Text = L.ReHashLocalFiles;
            rbDownloadPatchFilesInfo.Text = L.DownloadPatchFilesInfo;
            rbCalculateDownloads.Text = L.CalculateDownloads;
            rbDownloadFiles.Text = L.DownloadFiles;
            rbAddFiles.Text = L.AddFiles;
            rbDone.Text = L.PatchDone;

            troubleshootToolStripMenuItem.Text = L.TroubleShoot ;
            debugModeToolStripMenuItem.Text = L.TSDebugMode ;
            deleteShaderCacheToolStripMenuItem.Text = L.TSDeleteShaderCache;
            deleteGameConfigurationToolStripMenuItem.Text = L.TSDeleteGameSetting ;
            deleteAllArcheAgeSettingsToolStripMenuItem.Text = L.TSDeleteAll ;
            forcePatchDownloadToolStripMenuItem.Text = L.TSForcePatch ;
            skipPatchToolStripMenuItem.Text = L.TSForceNoPatch ;

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
            //Console.WriteLine("Updating Language: {0}",Setting.LauncherLang);
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
            SetDefaultSettings();
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
                if (!LoadSettings(Application.StartupPath + Path.DirectorySeparatorChar + launcherDefaultConfigFile))
                    SetDefaultSettings();
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
            string configFileName = Application.StartupPath + Path.DirectorySeparatorChar + clientLookupDefaultFile;

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

        private void SetDefaultSettings()
        {
            Setting.configName = "";
            Setting.Lang = settingsLangEN_US;
            Setting.LauncherLang = settingsLangEN_US;
            Setting.PathToGame = "";
            Setting.ServerIpAddress = "127.0.0.1";
            Setting.SaveLoginAndPassword = "False";
            Setting.SkipIntro = "False";
            Setting.HideSplashLogo = "False";
            Setting.LastLoginUser = "";
            Setting.LastLoginPass = "";
            Setting.ClientLoginType = stringTrino_1_2;
            Setting.UpdateLocale = "False";
            Setting.AllowGameUpdates = "False";
            Setting.ServerGameUpdateURL = "";
            Setting.ServerWebSiteURL = "http://aaemi.info/";
            Setting.ServerNewsFeedURL = "";
            Setting.UserHistory = new List<string>();
        }

        private bool LoadSettings(string configFileName)
        {
            bool res = false;

            if (!File.Exists(configFileName))
                return res;

            StreamReader reader = null ;
            //Console.WriteLine(configFileName);
            try
            {
                reader = new StreamReader(configFileName);
                var ConfigFile = reader.ReadToEnd();
                Console.Write(ConfigFile.ToString());

                Setting = JsonConvert.DeserializeObject<Settings>(ConfigFile);
                res = true;
                launcherOpenedConfigFile = configFileName;

                if (launcherOpenedConfigFile == Application.StartupPath + Path.DirectorySeparatorChar + launcherDefaultConfigFile)
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
                Setting.ClientLoginType = stringTrino_1_2;
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

        private static string CreateTrinoHandleIDs(string user, string pass)
        {
            byte[] data = Encoding.Default.GetBytes(pass);
            var passHash = new SHA256Managed().ComputeHash(data);

            uint handleID1 = 0;
            uint handleID2 = 0;

            string stringForSignature = "dGVzdA==";
            //string stringForSignature = "Signature 1:";

            string stringForTicket = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"yes\"?>";
            stringForTicket += "<authTicket version = \"1.2\">";
            stringForTicket += "<storeToken>1</storeToken>";
            stringForTicket += "<username>" + user + "</username>";
            stringForTicket += "<password>" + BitConverter.ToString(passHash).Replace("-", "").ToLower() + "</password>";
            stringForTicket += "</authTicket>";

            // Basically A complex way of doing stringForSignature + LineFeed + stringForTicket in a byte array
            byte[] bufferIntPtrID1 = Encoding.UTF8.GetBytes(stringForSignature);
            byte[] bufferIntPtrID2 = Encoding.UTF8.GetBytes(stringForTicket);
            byte[] bufferTotal = new byte[(bufferIntPtrID2.Length + 1) + bufferIntPtrID1.Length];
            Array.Copy(bufferIntPtrID1, 0, bufferTotal, 0, bufferIntPtrID1.Length);
            bufferTotal[bufferIntPtrID1.Length] = 10;
            Array.Copy(bufferIntPtrID2, 0, bufferTotal, bufferIntPtrID1.Length + 1, bufferIntPtrID2.Length);

            bool genRes = false ;
            try
            {
                genRes = generateInitStr(bufferTotal, bufferTotal.Length, bufferIntPtrID1, bufferIntPtrID1.Length, ref handleID1, ref handleID2);
            }
            catch
            {
                MessageBox.Show(L.ToolsAFailed);
            }
            if (genRes == false)
            {
                //MessageBox.Show("Error generating login handle");
                return "00000000:00000000";
            }
            else
            {
                return handleID1.ToString("X8") + ":" + handleID2.ToString("X8");
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

                    string HShield = " +acpxmk";

                    if (debugModeToolStripMenuItem.Checked)
                    {
                        DebugHelperForm dlg = new DebugHelperForm();
                        dlg.eArgs.Text = LoginArg;
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
            try
            {
                if (PatchDownloadPak != null)
                    PatchDownloadPak.ClosePak();
            }
            catch { }
            try
            {
                if (pak != null)
                    pak.ClosePak();
            }
            catch { }
        }

        private void SaveClientLookups()
        {
            string configFileName = Application.StartupPath + Path.DirectorySeparatorChar + clientLookupDefaultFile;

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
                var ClientLookupJson = JsonConvert.SerializeObject(ClientLookup,Formatting.Indented);
                try
                {
                    File.WriteAllText(configFileName, ClientLookupJson);
                }
                catch
                {
                    //Console.Write("Unable to save client lookup data to :\n" + launcherOpenedConfigFile);
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

            var SettingJson = JsonConvert.SerializeObject(Setting,Formatting.Indented);

            if ((launcherOpenedConfigFile == null) || (launcherOpenedConfigFile == "") || (File.Exists(launcherOpenedConfigFile) == false))
            {
                launcherOpenedConfigFile = Application.StartupPath + Path.DirectorySeparatorChar + launcherDefaultConfigFile;
            }
            //Console.Write("Saving Settings to "+ launcherOpenedConfigFile +" :\n" + SettingJson);
            try
            {
                File.WriteAllText(launcherOpenedConfigFile, SettingJson);
            }
            catch 
            {
                //Console.Write("Unable to save settings to :\n" + launcherOpenedConfigFile);
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
            switch (serverCheckStatus)
            {
                case serverCheck.Update:
                    if ((Setting.ServerGameUpdateURL != null) && (Setting.ServerGameUpdateURL != ""))
                    {
                        StartUpdate();
                    }
                    else
                    {
                        MessageBox.Show(L.NoUpdateURL,"No update URL");
                        serverCheckStatus = serverCheck.Unknown;
                        nextServerCheck = 1000;
                    }
                    break;
                case serverCheck.Updating:
                    // Do nothing with this button while we are updating
                    break;
                default:
                    // Start game if status is unknown, online or offline
                    Application.UseWaitCursor = true;
                    StartGame();
                    Application.UseWaitCursor = false;
                    break;
            }
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
            forcePatchDownloadToolStripMenuItem.Enabled = ((Setting.ServerGameUpdateURL != null) && (Setting.ServerGameUpdateURL != ""));
            skipPatchToolStripMenuItem.Enabled = ((Setting.ServerGameUpdateURL != null) && (Setting.ServerGameUpdateURL != ""));
            cmsAAEmuButton.Show(btnSystem,new Point(0, btnSystem.Height));
        }

        private void lSettingsBack_Click(object sender, EventArgs e)
        {
            SaveSettings();
            if ((serverCheckStatus == serverCheck.Online) || (serverCheckStatus == serverCheck.Offline) || (serverCheckStatus == serverCheck.Unknown))
            {
                serverCheckStatus = serverCheck.Unknown;
                nextServerCheck = 1000;
                ShowPanelControls(0);
            }
            if (serverCheckStatus == serverCheck.Update)
            {
                // Allow re-checking if settings changed (or not)
                serverCheckStatus = serverCheck.Unknown;
                nextServerCheck = 1000;
                ShowPanelControls(0);
            }
            if (serverCheckStatus == serverCheck.Updating)
            {
                ShowPanelControls(2);
            }
            updatePlayButton(serverCheckStatus, false);
        }

        private void lGamePath_Click(object sender, EventArgs e)
        {
            if (serverCheckStatus == serverCheck.Updating)
            {
                return;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (lGamePath.Text != "")
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(lGamePath.Text);
            }
            if (openFileDialog.InitialDirectory == "")
            {
                openFileDialog.InitialDirectory = "C:"+Path.DirectorySeparatorChar+"ArcheAge"+Path.DirectorySeparatorChar+"Working"+Path.DirectorySeparatorChar+"Bin32";
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

        private void ClearArcheAgeCache(bool clearShaders)
        {
            // C:\ArcheAge\Documents => UserHomeFolder\ArcheAge
            string aaDocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "ArcheAge" + Path.DirectorySeparatorChar;

            if (MessageBox.Show(L.DeleteShaderCache ,L.DeleteShaderCacheTitle , MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string[] rootDirs = Directory.GetDirectories(aaDocumentsFolder);
            foreach(string dir in rootDirs)
            {
                if (Path.GetFileName(dir).StartsWith("USER",true,null))
                {
                    var di = new DirectoryInfo(dir + Path.DirectorySeparatorChar + "shaders");
                    EmptyFolder(di,false);
                }
            }
        }

        private void UpdateGameSystemConfigFile(bool enableUpdateLocale, string locale, bool enableSkipIntro)
        {
            // C:\ArcheAge\Documents => UserHomeFolder\ArcheAge
            string configFileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "ArcheAge"+ Path.DirectorySeparatorChar + archeAgeSystemConfigFileName ;
            const string localeField = "locale = ";
            const string movieField = "login_first_movie = ";
            const string optionSoundField = "option_sound = ";
            const string DXField = "r_driver = ";

            List<string> lines = new List<string>();
            List<string> newLines = new List<string>();

            bool updatedLocale = false;
            bool updatedMovie = false;
            bool hasSoundOptionSetting = false;
            bool hasDXSetting = false;

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
                            newLines.Add(movieField + "1");
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
                    if (line.IndexOf(DXField) >= 0)
                    {
                        hasDXSetting = true;
                    }

                }
            }

            // Hack to make sure people can get past the server select on their first run
            // Apperantly missing the option_sound in the system.cfg will make it so the charater
            // select screen crashes
            if (hasSoundOptionSetting == false)
            {
                newLines.Add(optionSoundField + "4");
            }

            // Default to DX11 (DX10 in settings)
            // DX9 can sometimes give problems that makes you fail to load the character select screen
            // We'll also change to windowed 1280x768, most things should be able to handle this
            if (hasDXSetting == false)
            {
                newLines.Add(DXField + "\"DX10\"");
                var x = (Screen.PrimaryScreen.WorkingArea.Width - 1280) / 2;
                var y = (Screen.PrimaryScreen.WorkingArea.Height - 768) / 2;
                newLines.Add("r_windowx = "+x.ToString()); 
                newLines.Add("r_windowy = "+y.ToString());
                newLines.Add("r_width = 1280");
                newLines.Add("r_height = 768");
                newLines.Add("r_fullscreen = 0");
                newLines.Add("r_vsync = 0");
            }

            // Add our settings if needed
            if ((enableSkipIntro == true) && (updatedMovie == false))
            {
                newLines.Add(movieField + "1");
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
                string dirRes = TryAutoFindInDir(dirName + downDir.Name + Path.DirectorySeparatorChar, fileName,depth+1);
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
                //Console.WriteLine("Error in client lookup file, array size mismatch ! Clearing settings");
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

            string configPath = "";
            if (launcherOpenedConfigFile != "")
            {
                Path.GetDirectoryName(launcherOpenedConfigFile);
            }
               
            // Yes I know this trim looks silly, but it's to prevent stuff like "C:\\\\directory\\pathtogame.exe"
            if (configPath == "")
            {
                configPath = Path.GetDirectoryName(Application.ExecutablePath);
            }
            configPath = configPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar ;

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

        private void updatePlayButton(serverCheck serverState, bool isMouseOver)
        {

            if (isMouseOver == true)
            {
                switch (serverState)
                {
                    case serverCheck.Offline: // offline
                        btnPlay.Image = Properties.Resources.btn_red;
                        btnPlay.Text = L.Offline;
                        break;
                    case serverCheck.Online: // Play
                        btnPlay.Image = Properties.Resources.btn_green_a;
                        btnPlay.Text = L.Play ;
                        break;
                    case serverCheck.Update: // Update
                        btnPlay.Image = Properties.Resources.btn_green;
                        btnPlay.Text = L.Update ;
                        break;
                    case serverCheck.Updating: // Updating
                        btnPlay.Image = Properties.Resources.btn_red;
                        btnPlay.Text = L.Updating;
                        break;
                    case serverCheck.Unknown: // Play
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
                    case serverCheck.Offline: // offline
                        btnPlay.Image = Properties.Resources.btn_red;
                        btnPlay.Text = L.Offline ;
                        break;
                    case serverCheck.Online:
                        btnPlay.Image = Properties.Resources.btn_green;
                        btnPlay.Text = L.Play ;
                        break;
                    case serverCheck.Update: // Update
                        btnPlay.Image = Properties.Resources.btn_green_d;
                        btnPlay.Text = L.Update;
                        break;
                    case serverCheck.Updating: // Updating
                        btnPlay.Image = Properties.Resources.btn_red;
                        btnPlay.Text = L.Updating;
                        break;
                    case serverCheck.Unknown:
                    default:
                        btnPlay.Image = Properties.Resources.btn_green_d;
                        btnPlay.Text = L.Play ;
                        break;
                }

                if (serverCheckStatus == serverCheck.Updating)
                {
                    btnPlay.Cursor = Cursors.No;
                }
                else
                {
                    btnPlay.Cursor = Cursors.Hand;
                }

            }

        }


        private void timerGeneral_Tick(object sender, EventArgs e)
        {
            if (nextServerCheck > 0)
            {
                nextServerCheck -= timerGeneral.Interval;
                if (nextServerCheck <= 0)
                {
                    nextServerCheck += 1000 * 60 * 1 ; // 1 minute
                    bgwServerStatusCheck.RunWorkerAsync(); // former checkServerStatus();
                    updatePlayButton(serverCheckStatus, false);
                }
            }

            bool updateNews = false;
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

                    updateNews = true;
                }
            }

            if ((checkNews == true) || (updateNews == true))
            {
                if (!bgwNewsFeed.IsBusy)
                {
                    bgwNewsFeed.RunWorkerAsync();
                }
            }


        }

        private bool NeedToUpdateFrom(string remoteVersion)
        {
            string s = "";
            string localPatchVerFile = Path.GetDirectoryName(Path.GetDirectoryName(Setting.PathToGame)) + Path.DirectorySeparatorChar + localPatchFolderName + patchVersionFileName;
            try
            {
                if (File.Exists(localPatchVerFile))
                {
                    s = File.ReadAllText(localPatchVerFile);
                }
            }
            catch
            {
                s = "";
            }

            if ((s != "") && (aaPatcher.SetLocalVersionByString(s)))
            {
                if (aaPatcher.remoteVersion != aaPatcher.localVersion)
                {
                    // Different version, enable patching
                    return true;
                }
            }
            else if (s == "")
            {
                // No data, enable patching
                return true;
            }
            return false;
        }

        private void checkServerStatus()
        {
            if ((Setting.ServerIpAddress == null) || (Setting.ServerIpAddress == ""))
            {
                serverCheckStatus = serverCheck.Unknown;
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
                    serverCheckStatus = serverCheck.Online;
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

            if (Setting.AllowGameUpdates == "True")
            {
                if ((Setting.ServerGameUpdateURL != null) && (Setting.ServerGameUpdateURL != "") && (aaPatcher.remoteVersion == ""))
                {
                    // Download patch version file only once until it's invalidated
                    aaPatcher.remoteVersionString = WebHelper.SimpleGetURIAsString(Setting.ServerGameUpdateURL + remotePatchFolderURI + patchVersionFileName);
                    aaPatcher.SetRemoteVersionByString(aaPatcher.remoteVersionString);
                }
                if ((aaPatcher.remoteVersion != "") && (NeedToUpdateFrom(aaPatcher.remoteVersion)))
                {
                    aaPatcher.Init(Setting.PathToGame); // reset patch info
                    serverCheckStatus = serverCheck.Update;
                    nextServerCheck = -1;
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
            //Console.WriteLine("Updating Language: {0}", Setting.LauncherLang);
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
            troubleshootToolStripMenuItem.Visible = true;
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
            //Console.WriteLine("Updating Locale Language: {0}", Setting.Lang);
            UpdateLocaleLanguage();
            btnLocaleLang.Refresh();
        }

        private void CreateNewsFeedFromJSON(string JSONString)
        {

            try
            {
                newsFeed = JsonConvert.DeserializeObject<AAEmuNewsFeed>(JSONString);
            }
            catch
            {
                lNewsFeed.Text = L.ServerNewsFailed ;
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
                var bodyStr = i.itemAttributes.itemBody;
                if (bodyStr == null)
                    bodyStr = "";
                wns += bodyStr.Replace("\\r", "").Replace("\\n", "<br>");
                wns += "</font></div></p>";
            }
            // wns += "<p align=\"center\"><a href=\"testlink\" target=\"_new\">Test</a></p>";
            wns += "</body></html>";
            wbNews.DocumentText = wns;
            wbNews.Refresh();
            // File.WriteAllText("newscache.htm",wns);

            if ((newsFeed.data != null) && (newsFeed.data[0].itemAttributes.itemPicture != null) && (newsFeed.data[0].itemAttributes.itemPicture != ""))
            {
                // LoadBigNews(newsFeed.data[0]);
                // Move this to always load at the end of our newsfeed backgroundworker thread
                bigNewsIndex = 0;
                bigNewsTimer = 1000 * 10 * 1; // 10 seconds
            }
        }

        private Image LoadImageForNews(AAEmuNewsFeedDataItem newsItem)
        {
            string cacheFolder = Application.LocalUserAppDataPath + Path.DirectorySeparatorChar + "data";
            Directory.CreateDirectory(cacheFolder);
            string cacheFileName = cacheFolder + Path.DirectorySeparatorChar + "img -" + Setting.configName.Replace("@","_").Replace("/","_").Replace("\\","_").Replace("|","_") + "-" + newsItem.itemID + ".bin";

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
                    // Console.WriteLine("Error loading " + newsItem.itemAttributes.itemPicture + " into " + cacheFileName);
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

        private void forcePatchDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aaPatcher.Init(Setting.PathToGame); // reset patch info
            try
            {
                File.Delete(aaPatcher.localPatchDirectory + patchVersionFileName);
            }
            catch { }
            // force the button to update
            serverCheckStatus = serverCheck.Update; // Patch
            nextServerCheck = -1;
            updatePlayButton(serverCheckStatus, false);
            ShowPanelControls(0); // Update Panel (same as login but replaced news)
        }

        private void UpdateProgressBarTotal(int pos,int maxPos)
        {
            float posPC = (float)pos / (float)(maxPos);
            int progressSize = (int)(posPC * (float)pgbBackTotal.Width);
            pgbFrontTotal.Location = pgbBackTotal.Location;
            pgbFrontTotal.Size = new Size(progressSize, pgbBackTotal.Height);
        }

        private void StartUpdate()
        {
            serverCheckStatus = serverCheck.Updating;
            ShowPanelControls(2); // Swap to download layout
            updatePlayButton(serverCheckStatus, false);
            bgwPatcher.RunWorkerAsync(); // start patch process
        }

        private void bgwNewsFeed_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0);
            if (checkNews == true)
            {
                checkNews = false;
                worker.ReportProgress(1);
                System.Threading.Thread.Sleep(250);
                //lNewsFeed.Text = "Server News\n\n\n\nLoading ...";
                //wbNews.Visible = false;
                //wbNews.Tag = "0";
                try
                {
                    string newsString = WebHelper.SimpleGetURIAsString(Setting.ServerNewsFeedURL);
                    worker.ReportProgress(10);
                    CreateNewsFeedFromJSON(newsString);
                    worker.ReportProgress(50);
                }
                catch
                {
                    //lNewsFeed.Text = "Server News\n\n\n\nLoad Failed !";
                    worker.ReportProgress(99);
                }
            }

            if ((bigNewsIndex >= 0) && (bigNewsIndex < newsFeed.data.Count))
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    LoadBigNews(newsFeed.data[bigNewsIndex]);
                }));
            }
            worker.ReportProgress(100);
        }

        private void bgwNewsFeed_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Didn't feel like making a custom object, so just using the progress settings to change the label here
            switch(e.ProgressPercentage)
            {
                case 1:
                    lNewsFeed.Text = L.ServerNewsLoading ;
                    wbNews.Visible = false;
                    wbNews.Tag = "0";
                    break;
                case 10:
                    lNewsFeed.Text = L.ServerNews ;
                    break;
                case 99:
                    lNewsFeed.Text = L.ServerNewsFailed ;
                    break;
            }
        }

        private void bgwNewsFeed_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
        }

        private void bgwServerStatusCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            checkServerStatus();
        }

        private void bgwServerStatusCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updatePlayButton(serverCheckStatus, false);
        }

        private List<AAPakFileInfo> CreateXlFileListFromStream(Stream aStream)
        {
            List<AAPakFileInfo> res = new List<AAPakFileInfo>();

            aStream.Position = 0; // Rewind!
            List<string> rows = new List<string>();
            // Are you *sure* you want ASCII?
            using (var reader = new StreamReader(aStream, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    AAPakFileInfo xlfi = new AAPakFileInfo();
                    string[] items = line.Split(';');
                    // Path
                    xlfi.name = items[0];
                    // Size
                    long l = 0;
                    if (long.TryParse(items[1],out l))
                        xlfi.size = l;
                    else
                        xlfi.size = 0;
                    // If directory info

                    // Skip directories
                    if (xlfi.size < 0)
                        continue;

                    if (items.Length > 2)
                    {
                        xlfi.md5 = StringToByteArray(items[2]);
                    }
                    else
                    {
                        xlfi.md5 = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }
                    if (items.Length > 4)
                    {
                        xlfi.createTime = AAPatchProgress.PatchDateTimeStrToFILETIME(items[3]);
                        xlfi.modifyTime = AAPatchProgress.PatchDateTimeStrToFILETIME(items[4]);
                    }
                    else
                    {
                        xlfi.createTime = 0;
                        xlfi.modifyTime = 0;
                    }
                    res.Add(xlfi);
                }
            }
            return res;
        }

        private AAPakFileInfo FindPatchFileInList(string filename, ref List<AAPakFileInfo> list, ref int startIndex)
        {
            filename = filename.ToLower();
            int i = startIndex;
            for (int c = 0;c < list.Count; i++ , c++ )
            {
                if ((i >= list.Count) || (i <= 0))
                {
                    i = 0;
                }
                AAPakFileInfo pfi = list[i];
                if (pfi.name.ToLower() == filename)
                {
                    startIndex = i;
                    return pfi;
                }
            }
            return null;
        }

        private AAPakFileInfo FindPatchFileInSortedList(string filename, ref List<AAPakFileInfo> list)
        {
            filename = filename.ToLower();
            int searchMin = 0;
            int searchMax = list.Count - 1;
            int searchPos = 0;

            while (searchMin <= searchMax)
            {
                // Calculate range left
                // int searchSize = searchMax - searchMin;
                // Find the middle
                // searchPos = searchMin + (searchSize / 2);
                searchPos = ((searchMin + searchMax) / 2);

                var res = list[searchPos].name.CompareTo(filename);
                if (res < 0)
                {
                    // Looking for a smaller value
                    searchMin = searchPos+1;
                }
                else
                if (res > 0)
                {
                    // Looking for a higher value
                    searchMax = searchPos-1;
                }
                else
                {
                    return list[searchPos];
                }
            }

            return null;
        }

        private void bgwPatcher_DoWork(object sender, DoWorkEventArgs e)
        {
            //------
            // Init
            //------
            aaPatcher.Fase = PatchFase.Init;
            bgwPatcher.ReportProgress(0, aaPatcher);
            System.Threading.Thread.Sleep(150);
            try
            {
                DirectoryInfo patchDirInfo = Directory.CreateDirectory(aaPatcher.localPatchDirectory);
            }
            catch
            {
                // Failed to create temp patch directory, quit trying to update
                aaPatcher.Fase = PatchFase.Error;
                aaPatcher.ErrorMsg = string.Format(L.FailedCreateDirectory,aaPatcher.localPatchDirectory);
                return;
            }
            System.Threading.Thread.Sleep(150);

            aaPatcher.Fase = PatchFase.DownloadVerFile;
            bgwPatcher.ReportProgress(0, aaPatcher);

            // Download patch version file
            aaPatcher.remoteVersionString = WebHelper.SimpleGetURIAsString(Setting.ServerGameUpdateURL + remotePatchFolderURI + patchVersionFileName);
            if (!aaPatcher.SetRemoteVersionByString(aaPatcher.remoteVersionString))
            {
                aaPatcher.Fase = PatchFase.Error;
                aaPatcher.ErrorMsg = L.DownloadError +"\r\n"+Setting.ServerGameUpdateURL + remotePatchFolderURI + patchVersionFileName;
                return;
            }

            if (aaPatcher.remotePatchSystemVersion != "aaemu.patch.1")
            {
                aaPatcher.Fase = PatchFase.Error;
                aaPatcher.ErrorMsg = string.Format(L.UnsupportedPatchType,aaPatcher.remotePatchSystemVersion);
                return;
            }

            aaPatcher.Fase = PatchFase.CompareVersion;
            System.Threading.Thread.Sleep(250);

            //----------------------------------
            // Read Local version file (if any)
            //----------------------------------
            try
            {
                if (File.Exists(aaPatcher.localPatchDirectory + patchVersionFileName))
                {
                    aaPatcher.localVersion = File.ReadAllText(aaPatcher.localPatchDirectory + patchVersionFileName);
                }
                else
                {
                    aaPatcher.localVersion = L.ErrorReadingLocalVersionInformation ;
                }
            }
            catch
            {
                aaPatcher.localVersion = L.ErrorReadingLocalVersionInformation ;
            }

            System.Threading.Thread.Sleep(250);

            if (aaPatcher.localVersion == aaPatcher.remoteVersionString)
            {
                // Nothing to update, skip out
                aaPatcher.Fase = PatchFase.Done;
                aaPatcher.DoneMsg = L.NothingToUpdate ;
                return;
            }

            aaPatcher.Fase = PatchFase.CheckLocalFiles;
            bgwPatcher.ReportProgress(1, aaPatcher);
            System.Threading.Thread.Sleep(250);

            List<AAPakFileInfo> remotePakFileList ;
            List<AAPakFileInfo> rmPakFileList = new List<AAPakFileInfo>();
            dlPakFileList.Clear();

            //--------------------------------------
            // Create/Load Local Hash from game_pak
            //--------------------------------------
            if (File.Exists(aaPatcher.localGame_Pak))
            {
                aaPatcher.Fase = PatchFase.CheckLocalFiles;
                bgwPatcher.ReportProgress(2, aaPatcher);

                pak = new AAPak(aaPatcher.localGame_Pak, false, false);
                if (!pak.isOpen)
                {
                    // Failed to open pak
                    aaPatcher.Fase = PatchFase.Error;
                    aaPatcher.ErrorMsg = string.Format(L.FailedToOpen,aaPatcher.localGame_Pak);
                    return;
                }

                // Calculate local file size total
                // aaPatcher.FileDownloadSizeTotal = new System.IO.FileInfo(aaPatcher.localGame_Pak).Length;
                aaPatcher.FileDownloadSizeTotal = 0;
                var totalFilesCount = 0;
                foreach(AAPakFileInfo pfi in pak.files)
                {
                    aaPatcher.FileDownloadSizeTotal += pfi.size;
                    totalFilesCount++;
                }

                var filesCount = 0;
                foreach (AAPakFileInfo pfi in pak.files)
                {
                    if (BitConverter.ToString(pfi.md5).Replace("-","") == pak._header.nullHashString)
                    {
                        aaPatcher.Fase = PatchFase.ReHashLocalFiles;
                        pak.UpdateMD5(pfi);
                    }
                    filesCount++;
                    if ((filesCount % 50) == 0)
                    {
                        bgwPatcher.ReportProgress((filesCount * 100 / totalFilesCount), aaPatcher);
                    }
                }

                if (aaPatcher.Fase == PatchFase.ReHashLocalFiles)
                    pak.SaveHeader();
                System.Threading.Thread.Sleep(250);
            }
            else
            {
                // Failed to open pak
                aaPatcher.Fase = PatchFase.Error;
                aaPatcher.ErrorMsg = string.Format(L.FileNotFound, aaPatcher.localGame_Pak);
                return;
            }

            //------------------------
            // Download PatchFileInfo
            //------------------------
            aaPatcher.Fase = PatchFase.DownloadPatchFilesInfo;
            bgwPatcher.ReportProgress(2, aaPatcher);

            MemoryStream ms = WebHelper.SimpleGetURIAsMemoryStream(Setting.ServerGameUpdateURL + remotePatchFolderURI + patchListFileName);

            // check if downloaded patch files list has the correct hash
            var remotePatchFilesHash = WebHelper.GetMD5FromStream(ms);
            if (remotePatchFilesHash != aaPatcher.remotePatchFileHash)
            {
                aaPatcher.Fase = PatchFase.Error;
                aaPatcher.ErrorMsg = string.Format(L.PatchHashMismatch,remotePatchFilesHash,aaPatcher.remotePatchFileHash);
                return;
            }

            // Generate a files list in our pak files' format from the downloaded CSV
            remotePakFileList = CreateXlFileListFromStream(ms);

            System.Threading.Thread.Sleep(250);

            //--------------------------------------------------------------------------------------
            // Compare local game_pak with downloaded information to check what needs to be updated
            //--------------------------------------------------------------------------------------
            aaPatcher.Fase = PatchFase.CalculateDownloads ;
            bgwPatcher.ReportProgress(0, aaPatcher);

            // First sort both to speed things up
            pak.files.Sort();
            remotePakFileList.Sort();

            long totSize = 0;
            for (int i = 0 ; i < remotePakFileList.Count;i++)
            {
                AAPakFileInfo r = remotePakFileList[i];

                // Don't download empty files or entries marked as directories (-1)
                if (r.size <= 0) continue;

                AAPakFileInfo l = FindPatchFileInSortedList(r.name, ref pak.files);
                if (l == null)
                {
                    // We don't have a local copy of this file
                    // Add it to the list
                    dlPakFileList.Add(r);
                    totSize += r.size;
                }
                else
                {

                    if ( (l.size != r.size) || (l.md5.SequenceEqual(r.md5) == false) )
                    {
                        // Local Filesize or Hash is different from remote
                        // Redownload it
                        dlPakFileList.Add(r);
                        totSize += r.size;
                    }
                }

                if ((i % 100) == 0)
                {
                    int p = i * 100 / remotePakFileList.Count;
                    bgwPatcher.ReportProgress(p, aaPatcher);
                    // System.Threading.Thread.Sleep(1);
                }
            }
            aaPatcher.FileDownloadSizeTotal = totSize;

            if ((aaPatcher.FileDownloadSizeTotal <= 0) || (dlPakFileList.Count <= 0))
            {
                aaPatcher.Fase = PatchFase.Done;
                aaPatcher.DoneMsg = L.NoFilesToUpdate ;
                return;
            }

            //-------------------------------------------------
            // Initialize download.patch file (patch pak file)
            //-------------------------------------------------
            if (PatchDownloadPak == null)
                PatchDownloadPak = new AAPak("");

            if (!File.Exists(aaPatcher.localPatchDirectory + localPatchPakFileName))
            {
                try
                {
                    if (!PatchDownloadPak.NewPak(aaPatcher.localPatchDirectory + localPatchPakFileName))
                    {
                        aaPatcher.Fase = PatchFase.Error;
                        aaPatcher.ErrorMsg = string.Format(L.ErrorCreatingPatchCache, aaPatcher.localPatchDirectory + localPatchPakFileName);
                        return;
                    }
                    PatchDownloadPak.SaveHeader();
                    PatchDownloadPak.ClosePak();
                }
                catch (Exception x)
                {
                    aaPatcher.Fase = PatchFase.Error;
                    aaPatcher.ErrorMsg = string.Format(L.ErrorCreatingPatchCache,x.Message);
                    return;
                }
            }

            PatchDownloadPak.OpenPak(aaPatcher.localPatchDirectory + localPatchPakFileName, false);
            if (!PatchDownloadPak.isOpen)
            {
                // TODO: Add better support in case of fails
                //aaPatcher.Fase = PatchFase.Error;
                //aaPatcher.ErrorMsg = "Failed to open patch cache for writing, might be corrupted !";
                //return;
                MessageBox.Show(L.FailedToOpenPatchCache, "CACHE ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                try
                {
                    PatchDownloadPak.ClosePak();
                    PatchDownloadPak.NewPak(aaPatcher.localPatchDirectory + localPatchPakFileName);
                }
                catch
                {
                    PatchDownloadPak = null;
                }

                if ((PatchDownloadPak == null) || (!PatchDownloadPak.isOpen))
                {
                    aaPatcher.Fase = PatchFase.Error;
                    aaPatcher.ErrorMsg = L.FatalErrorFailedToOpenFileForWrite ;
                    return;
                }
            }

            // Generate a list of files to download
            // Skip any file that is already in our download.patch
            List<string> sl = new List<string>();

            totSize = 0; // calculate this again on the final list
            for (int i = dlPakFileList.Count-1; i >= 0;i--)
            {
                if (PatchDownloadPak.FileExists(dlPakFileList[i].name))
                {
                    dlPakFileList.Remove(dlPakFileList[i]);
                }
                else
                {
                    totSize += dlPakFileList[i].size;
                    sl.Add(dlPakFileList[i].name);
                }
            }
            aaPatcher.FileDownloadSizeTotal = totSize;

            // debug file to check what we'll download
            File.WriteAllLines(aaPatcher.localPatchDirectory + "download.txt", sl);

            // MessageBox.Show("Need to download " + dlPakFileList.Count.ToString() + " files, "+ (aaPatcher.FileDownloadSizeTotal / 1024 / 1024).ToString() + " MB total");

            System.Threading.Thread.Sleep(500);

            //-------------------
            // Do download stuff
            //-------------------
            aaPatcher.Fase = PatchFase.DownloadFiles;
            bgwPatcher.ReportProgress(0, aaPatcher);

            aaPatcher.FileDownloadSizeDownloaded = 0;
            for (int i = dlPakFileList.Count - 1; i >= 0; i--)
            {
                AAPakFileInfo pfi = dlPakFileList[i];
                var fileDLurl = Setting.ServerGameUpdateURL + pfi.name;

                try
                {
                    Stream fileDL = WebHelper.SimpleGetURIAsMemoryStream(fileDLurl);
                    if (fileDL.Length != pfi.size)
                    {
                        aaPatcher.Fase = PatchFase.Error;
                        aaPatcher.ErrorMsg = string.Format(L.DownloadSizeMismatch , fileDLurl, pfi.size.ToString(), fileDL.Length.ToString());
                        fileDL.Dispose();
                        return;
                    }
                    fileDL.Position = 0;
                    var fileHash =  WebHelper.GetMD5FromStream(fileDL);
                    var expectHash = BitConverter.ToString(pfi.md5).Replace("-", "").ToLower();
                    if (fileHash != expectHash)
                    {
                        aaPatcher.Fase = PatchFase.Error;
                        aaPatcher.ErrorMsg = string.Format(L.DownloadHashMismatch, fileDLurl,expectHash,fileHash);
                        fileDL.Dispose();
                        return;
                    }
                    var addpfi = PatchDownloadPak.nullAAPakFileInfo;
                    fileDL.Position = 0;
                    var addRes = PatchDownloadPak.AddFileFromStream(pfi.name, fileDL, DateTime.FromFileTime(pfi.createTime), DateTime.FromFileTime(pfi.modifyTime), false, out addpfi);
                    if (!addRes)
                    {
                        aaPatcher.Fase = PatchFase.Error;
                        aaPatcher.ErrorMsg = string.Format(L.FailedToSaveCache,pfi.name);
                        fileDL.Dispose();
                        return;
                    }
                    fileDL.Dispose();
                }
                catch
                {
                    aaPatcher.Fase = PatchFase.Error;
                    aaPatcher.ErrorMsg = string.Format(L.DownloadFileError,fileDLurl);
                    return;
                }

                aaPatcher.FileDownloadSizeDownloaded += pfi.size;

                var dlprogress = (aaPatcher.FileDownloadSizeDownloaded * 100) / aaPatcher.FileDownloadSizeTotal;
                bgwPatcher.ReportProgress((int)dlprogress, aaPatcher);
            }
            PatchDownloadPak.SaveHeader();

            //------------------------------------
            // Check if we need to extract the DB
            //------------------------------------
            bool exportDBAsWell = false;
            var dbNameInPak = "game/db/compact.sqlite3";
            if (PatchDownloadPak.FileExists(dbNameInPak))
            {
                Stream testStream = PatchDownloadPak.ExportFileAsStream(dbNameInPak);
                if (testStream.Length > 16)
                {
                    // var requiredHeader = "SQLite format 3";
                    byte[] buf = new byte[16];
                    testStream.Position = 0;
                    testStream.Read(buf, 0, 16);
                    if ((buf[0] == 'S') && (buf[1] == 'Q') && (buf[2] == 'L') && (buf[3] == 'i') && (buf[4] == 't') && (buf[5] == 'e'))
                    {
                        exportDBAsWell = true;
                    }
                }
                testStream.Dispose();
            }
            var bin32Dir = "bin32/";

            //--------------------------------------------------------------------------------------
            // Recalculate the total size to apply (including data to copy outside of the game_pak)
            //--------------------------------------------------------------------------------------
            aaPatcher.FileDownloadSizeTotal = 0;
            foreach(AAPakFileInfo pfi in PatchDownloadPak.files)
            {
                aaPatcher.FileDownloadSizeTotal += pfi.size;
                // Count files inside bin32 twice
                if ((pfi.name.Length > bin32Dir.Length) && (pfi.name.Substring(0, bin32Dir.Length) == bin32Dir))
                {
                    aaPatcher.FileDownloadSizeTotal += pfi.size;
                }
                // count compact.sqlite3 twice if it's not encrypted
                if ((pfi.name == dbNameInPak) && (exportDBAsWell))
                {
                    aaPatcher.FileDownloadSizeTotal += pfi.size;
                }
            }


            aaPatcher.FileDownloadSizeDownloaded = 0; // using downloadedsize as progress bar
            aaPatcher.Fase = PatchFase.AddFiles;
            //---------------------------------------------
            // Apply downloaded files, export where needed
            //---------------------------------------------
            foreach (AAPakFileInfo pfi in PatchDownloadPak.files)
            {
                Stream exportStream = PatchDownloadPak.ExportFileAsStream(pfi);
                exportStream.Position = 0;
                var respfi = pak.nullAAPakFileInfo;
                var addRes = pak.AddFileFromStream(pfi.name, exportStream, DateTime.FromFileTime(pfi.createTime), DateTime.FromFileTime(pfi.modifyTime), false, out respfi);
                if (!addRes)
                {
                    aaPatcher.Fase = PatchFase.Error;
                    aaPatcher.ErrorMsg = string.Format(L.ErrorPatchApplyFile,pfi.name);
                    exportStream.Dispose();
                    return;
                }
                aaPatcher.FileDownloadSizeDownloaded += pfi.size;

                // always export files inside bin32
                if ((pfi.name.Length > bin32Dir.Length) && (pfi.name.Substring(0, bin32Dir.Length) == bin32Dir))
                {
                    try
                    {
                        var destName = aaPatcher.localGameFolder + pfi.name.Replace('/', Path.DirectorySeparatorChar);
                        Directory.CreateDirectory(Path.GetDirectoryName(destName));
                        FileStream fs = new FileStream(destName, FileMode.Create);
                        exportStream.Position = 0;

                        exportStream.CopyTo(fs);

                        fs.Dispose();

                        // Update file details
                        File.SetCreationTime(destName, DateTime.FromFileTime(pfi.createTime));
                        File.SetLastWriteTime(destName, DateTime.FromFileTime(pfi.modifyTime));
                        aaPatcher.FileDownloadSizeDownloaded += pfi.size;
                    }
                    catch
                    {
                        aaPatcher.Fase = PatchFase.Error;
                        aaPatcher.ErrorMsg = string.Format(L.ErrorPatchExportFile,pfi.name);
                        exportStream.Dispose();
                        return;
                    }

                }

                // export compact.sqlite3 if it's not encrypted
                if ((pfi.name == dbNameInPak) && (exportDBAsWell))
                {
                    try
                    {
                        var destName = aaPatcher.localGameFolder + pfi.name.Replace('/', Path.DirectorySeparatorChar);
                        Directory.CreateDirectory(Path.GetDirectoryName(destName));
                        FileStream fs = new FileStream(destName, FileMode.Create);
                        exportStream.Position = 0;

                        exportStream.CopyTo(fs);

                        fs.Dispose();

                        // Update file details
                        File.SetCreationTime(destName, DateTime.FromFileTime(pfi.createTime));
                        File.SetLastWriteTime(destName, DateTime.FromFileTime(pfi.modifyTime));
                        aaPatcher.FileDownloadSizeDownloaded += pfi.size;
                    }
                    catch
                    {
                        aaPatcher.Fase = PatchFase.Error;
                        aaPatcher.ErrorMsg = string.Format(L.ErrorPatchExportDB, pfi.name);
                        exportStream.Dispose();
                        return;
                    }
                }
                if (exportStream != null)
                    exportStream.Dispose();

                var patchprogress = (aaPatcher.FileDownloadSizeDownloaded * 100) / aaPatcher.FileDownloadSizeTotal;
                bgwPatcher.ReportProgress((int)patchprogress, aaPatcher);

            }

            //-------------------------------------------
            // Delete all files mentioned in deleted.txt
            //-------------------------------------------
            try
            {
                var delFile = "deleted.txt";
                PatchDownloadPak.FileExists(delFile);
                Stream exportStream = PatchDownloadPak.ExportFileAsStream(delFile);
                exportStream.Position = 0;
                List<string> slDelFiles = new List<string>();
                using (StreamReader reader = new StreamReader(exportStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var s = reader.ReadLine();
                        slDelFiles.Add(s);
                    }
                }
                exportStream.Dispose();

                foreach(string s in slDelFiles)
                {
                    var delName = s.Replace('/', Path.DirectorySeparatorChar);
                    if (File.Exists(aaPatcher.localGameFolder + delName))
                        File.Delete(aaPatcher.localGameFolder + delName);
                }
            }
            catch { }

            System.Threading.Thread.Sleep(250);

            //----------------------------------
            // All done, back to the login page
            //----------------------------------
            aaPatcher.localVersion = aaPatcher.remoteVersion;
            aaPatcher.Fase = PatchFase.Done;
            aaPatcher.DoneMsg = L.PatchComplete;
            bgwPatcher.ReportProgress(100, aaPatcher);
            System.Threading.Thread.Sleep(1500);
        }

        private void bgwPatcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Patch progress
            AAPatchProgress p = e.UserState as AAPatchProgress;
            switch(p.Fase)
            {
                case PatchFase.Init:
                    lPatchProgressBarText.Text = L.CheckVersion;
                    rbInit.Checked = true;
                    break;
                case PatchFase.DownloadVerFile:
                    rbDownloadVerFile.Checked = true;
                    break;
                case PatchFase.CompareVersion:
                    rbComparingVersion.Checked = true;
                    break;
                case PatchFase.CheckLocalFiles:
                    lPatchProgressBarText.Text = L.CheckLocalFiles ;
                    rbCheckLocalFiles.Checked = true;
                    break;
                case PatchFase.ReHashLocalFiles:
                    lPatchProgressBarText.Text = L.Game_PakNeedsUpdate ;
                    rbReHashLocalFiles.Checked = true;
                    break;
                case PatchFase.DownloadPatchFilesInfo:
                    rbDownloadPatchFilesInfo.Checked = true;
                    break;
                case PatchFase.CalculateDownloads:
                    lPatchProgressBarText.Text = L.CheckVersion + " -> " + aaPatcher.remoteVersion ;
                    rbCalculateDownloads.Checked = true;
                    break;
                case PatchFase.DownloadFiles:
                    lPatchProgressBarText.Text = L.DownloadPatch;
                    rbDownloadFiles.Text = string.Format(L.DownloadSizeAndFiles, (aaPatcher.FileDownloadSizeTotal / 1024 / 1024).ToString(), dlPakFileList.Count.ToString());
                    // rbDownloadFiles.Text = "Download " + dlPakFileList.Count.ToString() + " patch files - " + (aaPatcher.FileDownloadSizeTotal / 1024 / 1024).ToString() + " MB";
                    rbDownloadFiles.Checked = true;
                    break;
                case PatchFase.AddFiles:
                    lPatchProgressBarText.Text = L.ApplyPatch;
                    rbAddFiles.Checked = true;
                    break;
                case PatchFase.Done:
                    lPatchProgressBarText.Text = L.PatchComplete;
                    rbDone.Checked = true;
                    break;
            }
            UpdateProgressBarTotal(e.ProgressPercentage, 100);
        }

        private void bgwPatcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Patch Finished
            // Revert server status to unknown, and do some cleaning

            // Close the patch cache file (if open)
            try
            {
                // Try saving out patch data if possible
                if (PatchDownloadPak != null)
                {
                    PatchDownloadPak.ClosePak();
                    PatchDownloadPak = null;
                }
            }
            catch { }

            // Close the client game_pak file (if open)
            try
            {
                // Try saving out patch data if possible
                if (pak != null)
                {
                    pak.ClosePak();
                    pak = null;
                }
            }
            catch { }

            if (aaPatcher.Fase == PatchFase.Error)
            {
                MessageBox.Show(aaPatcher.ErrorMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (aaPatcher.Fase == PatchFase.Done)
            {
                try
                {
                    // Save our .ver file again
                    File.WriteAllText(aaPatcher.localPatchDirectory + patchVersionFileName, aaPatcher.remoteVersionString);
                    // Delete Patch Pak if completed succesfully
                    // Keep patch cache file if debugging
                    if (debugModeToolStripMenuItem.Checked == false)
                      File.Delete(aaPatcher.localPatchDirectory + localPatchPakFileName);
                }
                catch (Exception x)
                {
                    MessageBox.Show(string.Format(L.ErrorSavingVersionInfo,x.Message));
                }
                if (aaPatcher.DoneMsg != "")
                {
                    MessageBox.Show(aaPatcher.DoneMsg, L.PatchComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            serverCheckStatus = serverCheck.Unknown;
            nextServerCheck = 1000;
            updatePlayButton(serverCheckStatus, false);
            ShowPanelControls(0);
        }

        public byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void skipPatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serverCheckStatus = serverCheck.Unknown; // Force to unknown mode
            nextServerCheck = -1;
            updatePlayButton(serverCheckStatus, false);
            ShowPanelControls(0); // Update Panel

        }

        private void deleteShaderCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearArcheAgeCache(true);
        }

        private void EmptyFolder(DirectoryInfo directoryInfo,bool onlyFiles)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                EmptyFolder(subfolder,onlyFiles);
                if (!onlyFiles)
                    subfolder.Delete();
            }
        }

        private void deleteGameConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(L.DeleteGameSettings, "Delete system.cfg",MessageBoxButtons.YesNo,MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            // C:\ArcheAge\Documents => UserHomeFolder\ArcheAge
            string systemConfigFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "ArcheAge" + Path.DirectorySeparatorChar + archeAgeSystemConfigFileName;
            File.Delete(systemConfigFile);
        }
    }
}
