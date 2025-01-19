using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.Win32;

namespace AAEmu.Launcher
{
    /// <summary>
    /// TcpClientWithTimeout is used to open a TcpClient connection, with a 
    /// user definable connection timeout in milliseconds (1000=1second)
    /// Use it like this:
    /// TcpClient connection = new TcpClientWithTimeout('127.0.0.1',80,1000).Connect();
    /// </summary>
    public class TcpClientWithTimeout
    {
        protected string _hostname;
        protected int _port;
        protected int _timeout_milliseconds;
        protected TcpClient connection;
        protected bool connected;
        protected Exception exception;

        public TcpClientWithTimeout(string hostname, int port, int timeout_milliseconds)
        {
            _hostname = hostname;
            _port = port;
            _timeout_milliseconds = timeout_milliseconds;
        }
        public TcpClient Connect()
        {
            // kick off the thread that tries to connect
            connected = false;
            exception = null;
            Thread thread = new Thread(new ThreadStart(BeginConnect));
            thread.IsBackground = true; // So that a failed connection attempt 
                                        // wont prevent the process from terminating while it does the long timeout
            thread.Start();

            // wait for either the timeout or the thread to finish
            thread.Join(_timeout_milliseconds);

            if (connected == true)
            {
                // it succeeded, so return the connection
                thread.Abort();
                return connection;
            }
            if (exception != null)
            {
                // it crashed, so return the exception to the caller
                thread.Abort();
                throw exception;
            }
            else
            {
                // if it gets here, it timed out, so abort the thread and throw an exception
                thread.Abort();
                string message = string.Format("TcpClient connection to {0}:{1} timed out",
                  _hostname, _port);
                throw new TimeoutException(message);
            }
        }
        protected void BeginConnect()
        {
            try
            {
                connection = new TcpClient(_hostname, _port);
                // record that it succeeded, for the main thread to return to the caller
                connected = true;
            }
            catch (Exception ex)
            {
                // record the exception for the main thread to re-throw back to the calling code
                exception = ex;
            }
        }
    }

    public static class WebHelper
    {
        public static string SimpleGetURIAsString(string uri, int timeOut = -1)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (timeOut >= 0)
                request.Timeout = timeOut;
            request.UserAgent = "AAEmu.Launcher";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return "";
            }
        }

        public static MemoryStream SimpleGetURIAsMemoryStream(string uri, out Exception returnException, int timeOut = -1)
        {
            MemoryStream ms = new MemoryStream();
            returnException = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                if (timeOut >= 0)
                    request.Timeout = timeOut;
                request.UserAgent = "AAEmu.Launcher";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    stream.CopyTo(ms);
                    return ms;
                }
            }
            catch (Exception e)
            {
                ms.SetLength(0);
                returnException = e;
                return ms;
            }
        }

        public static string GetMD5FromStream(Stream fs)
        {
            MD5 hash = MD5.Create();
            fs.Position = 0;
            var newHash = hash.ComputeHash(fs);
            hash.Dispose();
            return BitConverter.ToString(newHash).Replace("-", "").ToLower(); // Return the (updated) md5 as a string
        }

    }

    public struct AAEmuNewsFeedLinksItem
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }

    public struct AAEmuNewsFeedDataItemAttributes
    {

        [JsonProperty("picture")]
        public string ItemPicture { get; set; }
        [JsonProperty("title")]
        public string ItemTitle { get; set; }
        [JsonProperty("body")]
        public string ItemBody { get; set; }
        [JsonProperty("new")]
        public string ItemIsNew { get; set; }
        [JsonProperty("links")]
        public AAEmuNewsFeedLinksItem ItemLinks { get; set; }
    }

    public struct AAEmuNewsFeedDataItem
    {
        [JsonProperty("type")]
        public string ItemType { get; set; }

        [JsonProperty("id")]
        public int ItemID { get; set; }

        [JsonProperty("attributes")]
        public AAEmuNewsFeedDataItemAttributes ItemAttributes { get; set; }

    }

    public partial class AAEmuNewsFeed
    {
        [JsonProperty("data")]
        public List<AAEmuNewsFeedDataItem> Data { get; set; }

        [JsonProperty("links")]
        public AAEmuNewsFeedLinksItem Links { get; set; }
    }

    public class PakFileInfo
    {
        public string filePath;
        public Int64 fileSize;
        public string fileHash;
        public FileSystemInfo fileInfo;
    }

    public enum PatchFase { Error, Init, DownloadVerFile, CompareVersion, CheckLocalFiles, ReHashLocalFiles, DownloadPatchFilesInfo, CalculateDownloads, DownloadFiles, AddFiles, Done };
    public class AAPatchProgress
    {
        public PatchFase Fase = PatchFase.Init;
        public string localVersion = "";
        public string remoteVersionString = "";
        public string remoteVersion = "";
        public string remotePatchFileHash = "";
        public string remotePatchSystemVersion = "0";
        public string localGame_Pak = "";
        public string localGameFolder = "";
        public string localPatchDirectory = ".patch\\";
        public string ErrorMsg = "NO_ERROR";
        public string DoneMsg = "";
        public List<PakFileInfo> localPakFileList = new List<PakFileInfo>();
        public List<PakFileInfo> remotePakFileList = new List<PakFileInfo>();

        public Int64 FileDownloadSizeTotal = 0;
        public Int64 FileDownloadSizeDownloaded = 0;
        public Int64 FileApplySize = 0;

        public void Init(string ArcheAgeExeLocation)
        {
            Fase = PatchFase.Init;
            remotePatchSystemVersion = "0";
            localVersion = "";
            remoteVersion = "";
            localGameFolder = Path.GetDirectoryName(Path.GetDirectoryName(ArcheAgeExeLocation)) + Path.DirectorySeparatorChar;
            localGame_Pak = localGameFolder + "game_pak";
            localPatchDirectory = localGameFolder + ".patch" + Path.DirectorySeparatorChar;
            localPakFileList = new List<PakFileInfo>();
            remotePakFileList = new List<PakFileInfo>();

            FileDownloadSizeTotal = 0;
            FileDownloadSizeDownloaded = 0;
            FileApplySize = 0;
            ErrorMsg = "NO_ERROR";
            DoneMsg = "";
        }

        public void RecalculateTotalDownloadSize()
        {
            Int64 c = 0;
            for (int i = 0; i < remotePakFileList.Count; i++)
            {
                c += remotePakFileList[i].fileSize;
            }
            FileDownloadSizeTotal = c;
        }

        public bool SetRemoteVersionByString(string verStr)
        {
            string[] strItems = verStr.Split(';');
            if (strItems.Length >= 3) // Only check if 3 or more, might extend later versions
            {
                // Primitive check for size mismatch
                if ((strItems[0].Length != 15) || (strItems[1].Length != 32))
                {
                    return false;
                }
                remoteVersion = strItems[0];
                remotePatchFileHash = strItems[1];
                remotePatchSystemVersion = strItems[2];
                return true;
            }
            return false;
        }

        public bool SetLocalVersionByString(string verStr)
        {
            string[] strItems = verStr.Split(';');
            if (strItems.Length >= 3) // Only check if 3 or more, might extend later versions, only first is used for local patch info
            {
                // Primitive check for size mismatch
                if ((strItems[0].Length != 15))
                {
                    return false;
                }
                localVersion = strItems[0];
                return true;
            }
            return false;
        }

        public int GetDownloadProgressPercent()
        {
            long p = FileDownloadSizeDownloaded * 100 / FileDownloadSizeTotal;
            return (int)p;
        }

        public static string DateTimeToPatchDateTimeStr(DateTime aTime)
        {
            string res ;
            try
            {
                res = aTime.ToString("yyyyMMdd-HHmmss");
            }
            catch
            {
                res = "00000000-000000";
            }
            return res;
        }

        public static long PatchDateTimeStrToFILETIME(string encodedString)
        {
            try
            {
                if (!int.TryParse(encodedString.Substring(0, 4), out var yyyy)) yyyy = 0;
                if (!int.TryParse(encodedString.Substring(4, 2), out var mm)) mm = 0;
                if (!int.TryParse(encodedString.Substring(6, 2), out var dd)) dd = 0;
                if (!int.TryParse(encodedString.Substring(9, 2), out var hh)) hh = 0;
                if (!int.TryParse(encodedString.Substring(11, 2), out var nn)) nn = 0;
                if (!int.TryParse(encodedString.Substring(13, 2), out var ss)) ss = 0;

                return (new DateTime(yyyy, mm, dd, hh, nn, ss)).ToFileTime();
            }
            catch
            {
                return 0;
            }
        }

    }

    public class FileAssociation
    {
        public string Extension { get; set; }
        public string ProgId { get; set; }
        public string FileTypeDescription { get; set; }
        public string ExecutableFilePath { get; set; }
    }


    // source: https://stackoverflow.com/questions/2681878/associate-file-extension-with-application
    public class FileAssociations
    {
        // needed so that Explorer windows get refreshed after the registry is updated
        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        private const int SHCNE_ASSOCCHANGED = 0x8000000;
        private const int SHCNF_FLUSH = 0x1000;

        public static void EnsureAssociationsSet()
        {
            var filePath = Process.GetCurrentProcess().MainModule.FileName;
            EnsureAssociationsSet(
                new FileAssociation
                {
                    Extension = ".aelcf",
                    ProgId = "AAEmu_Launcher_Config",
                    FileTypeDescription = "ArcheAge Emu Launcher Configuration File",
                    ExecutableFilePath = filePath
                });
        }

        public static void EnsureURIAssociationsSet()
        {
            var filePath = Process.GetCurrentProcess().MainModule.FileName;
            SetURIAssociation("aelcf", "AAEmu Launcher Protocol", filePath);
        }

        public static void EnsureAssociationsSet(params FileAssociation[] associations)
        {
            bool madeChanges = false;
            foreach (var association in associations)
            {
                madeChanges |= SetAssociation(
                    association.Extension,
                    association.ProgId,
                    association.FileTypeDescription,
                    association.ExecutableFilePath);
            }

            if (madeChanges)
            {
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static bool SetAssociation(string extension, string progId, string fileTypeDescription, string applicationFilePath)
        {
            bool madeChanges = false;
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + extension, progId);
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + progId, fileTypeDescription);
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            return madeChanges;
        }

        private static bool SetKeyDefaultValue(string keyPath, string value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                if (key.GetValue(null) as string != value)
                {
                    key.SetValue(null, value);
                    return true;
                }
            }

            return false;
        }

        private static bool SetKeyValue(string keyPath, string keyName, string value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                if (key.GetValue(keyName) as string != value)
                {
                    key.SetValue(keyName, value);
                    return true;
                }
            }

            return false;
        }

        public static bool SetURIAssociation(string protocolID, string protocolName, string applicationFilePath)
        {
            bool madeChanges = false;
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + protocolID, "URL:"+protocolName);
            madeChanges |= SetKeyValue(@"Software\Classes\" + protocolID, "URL PRotocol", "");
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + protocolID + @"\DefaultIcon", "\"" + applicationFilePath+",1\"");
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + protocolID + @"\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            return madeChanges;
        }


    }

}
