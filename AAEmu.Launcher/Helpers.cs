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


    public static class RC4
    {
        public static string Encrypt(string key, string data)
        {
            Encoding unicode = Encoding.Unicode;

            return Convert.ToBase64String(Encrypt(unicode.GetBytes(key), unicode.GetBytes(data)));
        }

        public static string Decrypt(string key, string data)
        {
            Encoding unicode = Encoding.Unicode;

            return unicode.GetString(Encrypt(unicode.GetBytes(key), Convert.FromBase64String(data)));
        }

        public static byte[] Encrypt(byte[] key, byte[] data)
        {
            return EncryptOutput(key, data).ToArray();
        }

        public static byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptOutput(key, data).ToArray();
        }

        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256)
              .Select(i => (byte)i)
              .ToArray();

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;

                Swap(s, i, j);
            }

            return s;
        }

        private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = EncryptInitalize(key);

            int i = 0;
            int j = 0;

            return data.Select((b) =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;

                Swap(s, i, j);

                return (byte)(b ^ s[(s[i] + s[j]) & 255]);
            });
        }

        private static void Swap(byte[] s, int i, int j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }
    }



    public static class WebHelper
    {
        public static string SimpleGetURIAsString(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
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

        public static MemoryStream SimpleGetURIAsMemoryStream(string uri)
        {
            MemoryStream ms = new MemoryStream();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.UserAgent = "AAEmu.Launcher";
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    stream.CopyTo(ms);
                    return ms;
                }
            }
            catch
            {
                ms.SetLength(0);
                return ms;
            }
        }

        public static string GetMD5FromStream(Stream fs)
        {
            MD5 hash = MD5.Create();
            fs.Position = 0;
            var newHash = hash.ComputeHash(fs);
            hash.Dispose();
            return BitConverter.ToString(newHash).Replace("-", "").ToUpper(); // Return the (updated) md5 as a string
        }

    }

public struct AAEmuNewsFeedLinksItem
    {
        [JsonProperty("self")]
        public string self { get; set; }
    }

    public struct AAEmuNewsFeedDataItemAttributes
    {

        [JsonProperty("picture")]
        public string itemPicture { get; set; }
        [JsonProperty("title")]
        public string itemTitle { get; set; }
        [JsonProperty("body")]
        public string itemBody { get; set; }
        [JsonProperty("new")]
        public string itemIsNew { get; set; }
        [JsonProperty("links")]
        public AAEmuNewsFeedLinksItem itemLinks { get; set; }
    }

    public struct AAEmuNewsFeedDataItem
    {
        [JsonProperty("type")]
        public string itemType { get; set; }

        [JsonProperty("id")]
        public int itemID { get; set; }

        [JsonProperty("attributes")]
        public AAEmuNewsFeedDataItemAttributes itemAttributes { get; set; }

    }

    public partial class AAEmuNewsFeed
    {
        [JsonProperty("data")]
        public List<AAEmuNewsFeedDataItem> data { get; set; }

        [JsonProperty("links")]
        public AAEmuNewsFeedLinksItem links { get; set; }
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
        public string localPatchDirectory = ".patch\\";
        public string ErrorMsg = "NO_ERROR";
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
            localGame_Pak = Path.GetDirectoryName(Path.GetDirectoryName(ArcheAgeExeLocation)) + "\\game_pak";
            localPatchDirectory = Path.GetDirectoryName(Path.GetDirectoryName(ArcheAgeExeLocation)) + "\\.patch\\";
            localPakFileList = new List<PakFileInfo>();
            remotePakFileList = new List<PakFileInfo>();

            FileDownloadSizeTotal = 0;
            FileDownloadSizeDownloaded = 0;
            FileApplySize = 0;
            ErrorMsg = "NO_ERROR";
        }

        public void RecalculateTotalDownloadSize()
        {
            Int64 c = 0;
            for (int i = 0; i < remotePakFileList.Count;i++)
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
            long p = FileDownloadSizeDownloaded * 100 / FileDownloadSizeTotal ;
            return (int)p;
        }

        public static string DateTimeToPAtchDateTimeStr(DateTime aTime)
        {
            string res = "";
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
            long res = 0 ;

            int yyyy = 0;
            int mm = 0;
            int dd = 0;
            int hh = 0;
            int nn = 0;
            int ss = 0;

            try
            {
                if (!int.TryParse(encodedString.Substring(0, 4), out yyyy)) yyyy = 0;
                if (!int.TryParse(encodedString.Substring(4, 2), out mm)) mm = 0;
                if (!int.TryParse(encodedString.Substring(6, 2), out dd)) dd = 0;
                if (!int.TryParse(encodedString.Substring(9, 2), out hh)) hh = 0;
                if (!int.TryParse(encodedString.Substring(11, 2), out nn)) nn = 0;
                if (!int.TryParse(encodedString.Substring(13, 2), out ss)) ss = 0;

                res = (new DateTime(yyyy, mm, dd, hh, nn, ss)).ToFileTime();
            }
            catch
            {
                res = 0;
            }
            return res;
        }

    }



}
