using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;


namespace AAEmu.Patch
{
    public class Settings
    {
        public static string appVer = "V0.2.1";
        public static string patchSystemVersion = "aaemu.patch.1";
        public static List<string> ignoreList = new List<string>();
        public static string patchListFileName = "patchfiles.csv";
        public static string patchVersionFileName = "patchfiles.ver";
        public static string masterRoot = "..\\.\\"; // By default place the patcher in a .patch folder together with the .git, bin32 and game folder

        public static void AddIgnoreFile(string aFileName)
        {
            if (CanAddFile(aFileName))
            {
                ignoreList.Add(aFileName.ToLower());
            }
        }

        public static bool CanAddFile(string aFileName)
        {
            return (ignoreList.IndexOf(aFileName.ToLower()) < 0);
        }
    }

    class PakFileInfo
    {
        public string filePath;
        public Int64 fileSize;
        public string fileHash;
        public FileSystemInfo fileInfo;
    }

    class Program
    {

        public static List<PakFileInfo> pakFiles = new List<PakFileInfo>();

        private static void Log(string heading, string msg)
        {
            if (heading == "")
            {
                Console.WriteLine("{0}", msg);
            }
            else
            {
                
                Console.WriteLine("[{0} {2}]: {1}", heading, msg, DateTime.Now.ToString());
            }
        }

        private static void LoadSettings()
        {
            Settings.masterRoot = new DirectoryInfo(".\\").Parent.FullName + "\\" ;
            Log("Config", "PAK Root: " + Settings.masterRoot);
            string[] iList;
            try
            {
                iList = File.ReadAllLines("ignore.txt", Encoding.UTF8);
                Settings.ignoreList.AddRange(iList);
            }
            catch
            {
                // load defaults if file doesn't exist
                Log("Warning", "No ignore file found, loading defaults");
                Settings.AddIgnoreFile(".");
                Settings.AddIgnoreFile("..");
                Settings.AddIgnoreFile(".git");
                Settings.AddIgnoreFile(".patch");
            }

        }

        private static string GetMd5Hash(MD5 md5Hash, Stream input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(input);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private static string GetMD5ForFile(string aFile)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                try
                {
                    FileStream fs = new FileStream(aFile, FileMode.Open, FileAccess.Read);
                    string hash = GetMd5Hash(md5Hash, fs);
                    fs.Close();
                    return hash;
                }
                catch
                {
                    Log("Hash", "Error creating md5 hash for " + aFile);
                    return "ERROR";
                }
            }

        }

        private static void AddDirectory(string path)
        {
            Console.Title = "AAEmu.Patch "+Settings.appVer+" @ " + path;

            PakFileInfo pfi;
            DirectoryInfo thisDir = new DirectoryInfo(path);
            FileInfo[] files = thisDir.GetFiles();
            foreach (FileInfo fi in files)
            {
                if (Settings.CanAddFile(fi.Name))
                {
                    pfi = new PakFileInfo();

                    pfi.filePath = path.Substring(Settings.masterRoot.Length) + fi.Name;
                    pfi.fileSize = fi.Length;
                    pfi.fileInfo = fi;
                    pfi.fileHash = GetMD5ForFile(path+ fi.Name);
                    pfi.filePath = pfi.filePath.Replace("\\", "/").ToLower(); // make all forward slashes, and lowercase it while we're at it


                    pakFiles.Add(pfi);

                    if ((pakFiles.Count % 250) == 0)
                    {
                        Console.Write(".");
                    }
                }
            }

            DirectoryInfo[] dirs = thisDir.GetDirectories();
            foreach (DirectoryInfo di in dirs)
            {
                if (Settings.CanAddFile(di.Name))
                {
                    // We no longer store the directories, as the pak itself doesn't either
                    // This will save roughly 30k lines to compare
                    /*
                    pfi = new PakFileInfo();

                    pfi.filePath = path.Substring(Settings.masterRoot.Length) + di.Name;
                    pfi.fileSize = -1 ;
                    pfi.fileInfo = di ;
                    pfi.fileHash = "";
                    pfi.filePath = pfi.filePath.Replace("\\", "/"); // make all forward slashes

                    pakFiles.Add(pfi);
                    */
                    AddDirectory(path + di.Name+"\\");
                }
            }


            Console.Title = "AAEmu.Patch " + Settings.appVer;
        }

        private static void SavePatchFiles()
        {
            try
            {
                Log("Save", "Saving patch files data");
                List<string> patchCSV = new List<string>();

                foreach (PakFileInfo pfi in pakFiles)
                {
                    /*
                    if (pfi.fileSize < 0)

                    {
                        patchCSV.Add(pfi.filePath + ";-1;;;;");
                    }
                    else
                    {
                        patchCSV.Add(pfi.filePath + ";" + pfi.fileSize + ";"+pfi.fileHash+";" + pfi.fileInfo.CreationTime.ToString("yyyyMMdd-HHmmss") + ";" + pfi.fileInfo.LastWriteTime.ToString("yyyyMMdd-HHmmss") + ";");
                    }
                    */
                    patchCSV.Add(pfi.filePath + ";" + pfi.fileSize + ";" + pfi.fileHash + ";" + pfi.fileInfo.CreationTime.ToString("yyyyMMdd-HHmmss") + ";" + pfi.fileInfo.LastWriteTime.ToString("yyyyMMdd-HHmmss") + ";");
                }
                patchCSV.Sort();

                File.WriteAllLines(Settings.patchListFileName, patchCSV);
                Log("Save", "Saved data for " + patchCSV.Count.ToString() + " files");
            }
            catch
            {
                Log("ERROR", "Failed to write data to " + Settings.patchListFileName);
            }
        }

        private static void SavePatchVersion()
        {
            string listHash = GetMD5ForFile(Settings.patchListFileName);
            string verFile = DateTime.Now.ToString("yyyyMMdd-HHmmss") + ";" + listHash + ";"+ Settings.patchSystemVersion ;
            File.WriteAllText(Settings.patchVersionFileName, verFile);
            Log("Save", "Saved patch version");
        }

        private static void MakePatch()
        {
            Log("Info", "Creating patch ...");
            pakFiles.Clear();
            AddDirectory(Settings.masterRoot);
            Console.WriteLine();
            SavePatchFiles();
            SavePatchVersion();
        }

        static void Main(string[] args)
        {
            Console.Title = "AAEmu.Patch " + Settings.appVer;

            Log("", "AAEmu server patch creator "+Settings.appVer);
            Log("", "-----------------------------------");
            Log("Info", "Loading settings");

            LoadSettings();

            Console.WriteLine("");
            Console.Write("Are you sure you want to create a new patch (y/N): ");
            string rl = Console.ReadLine();
            Console.WriteLine("");

            if (rl.ToLower().StartsWith("y"))
            {
                MakePatch();
                Log("Info", "Completed");
            }
            else
            {
                Log("Info", "Nothing to do");
            }

            Console.WriteLine("");
            Console.Write("Press ENTER to continue ... ");
            Console.ReadLine();
        }
    }
}
