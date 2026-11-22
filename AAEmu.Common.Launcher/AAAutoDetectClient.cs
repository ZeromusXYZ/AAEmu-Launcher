using AAPacker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;

// DetectRevision function Credits: https://github.com/Ingramz/

namespace AAEmu.Launcher.Basic
{
    public class AaAutoDetectClient
    {
        public static string FlavorArcheAge = @"ArcheAge";
        public static string FlavorArcheWorld = @"ArcheWorld";
        public static string FlavorArcheRage = @"ArcheRage";
        public static string FlavorAAFree = @"AAFree";
        public static string FlavorAAClassic = @"AAClassic";

        public static string GuessLauncher(string archeAgeExeFile, string launcherExe)
        {
            // Check if main exe and game_pak exist
            if (!File.Exists(archeAgeExeFile))
                return string.Empty;
            
            var isArcheWorld = (archeAgeExeFile.ToLowerInvariant().Contains(FlavorArcheWorld.ToLowerInvariant())) ;
            var pakFileName = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(archeAgeExeFile)) ?? "", "game_pak");
            if (!File.Exists(pakFileName))
                return string.Empty;

            var flavor = GuessDocumentsFolder(archeAgeExeFile, launcherExe);

            var res = string.Empty;
            try
            {
                // Fast check with revisions
                var revision = DetectRevision(archeAgeExeFile);
                ulong newestValidRevision = 0;
                if (revision > 0)
                {
                    foreach (var aaLauncherContainer in AAEmuLauncherBase.AllLaunchers)
                    {
                        var tempLauncher =
                            Activator.CreateInstance(aaLauncherContainer.LauncherClass) as AAEmuLauncherBase;
                        if (tempLauncher == null)
                            continue;
                        if (!tempLauncher.IsFlavorAllowed(flavor))
                            continue;

                        if (revision >= aaLauncherContainer.MinimumRevision)
                        {
                            if (aaLauncherContainer.MinimumRevision > newestValidRevision)
                            {
                                newestValidRevision = aaLauncherContainer.MinimumRevision;
                                res = aaLauncherContainer.ConfigName;
                            }
                        }
                    }
                }
                else
                {
                    // Revision detection failed, so it's likely an encrypted and we'll need to rely on the flavor detection to try and find the correct launcher
                    foreach (var aaLauncherContainer in AAEmuLauncherBase.AllLaunchers)
                    {
                        var tempLauncher =
                            Activator.CreateInstance(aaLauncherContainer.LauncherClass) as AAEmuLauncherBase;
                        if (tempLauncher == null)
                            continue;
                        if (!tempLauncher.IsFlavorAllowed(flavor))
                            continue;

                        res = aaLauncherContainer.ConfigName;
                    }
                }

                if (res != string.Empty)
                {
                    return res;
                }

                // Next Check by .exe version
                var versionInfo = FileVersionInfo.GetVersionInfo(archeAgeExeFile);
                string version =
                    string.Join(".",
                        versionInfo.FileVersion.Replace(" ", "")
                            .Split(',')); // Will typically return "1.0.0.0" in your case

                // Try detecting with version first
                if (isArcheWorld || (string.Compare(version, "2.9") > 0))
                {
                    // For more recent versions, checking the .exe should be enough to be accurate
                    var newestValidVersion = "";
                    foreach (var aaLauncherContainer in AAEmuLauncherBase.AllLaunchers)
                    {
                        var compareVersion = isArcheWorld
                            ? aaLauncherContainer.MinimumVersionForWorld
                            : aaLauncherContainer.MinimumVersion;

                        if (!string.IsNullOrWhiteSpace(compareVersion) &&
                            (string.Compare(version, compareVersion, true) > 0) &&
                            (string.Compare(version, newestValidVersion, true) > 0))
                        {
                            newestValidVersion = aaLauncherContainer.MinimumVersion;
                            res = aaLauncherContainer.ConfigName;
                        }
                    }

                }

                if (res == string.Empty)
                {
                    // For older versions, it's best to check inside the game_pak

                    // Check by game_pak/game/worlds/main_world/world.xml 's create date
                    var pak = new AAPak(pakFileName, true);
                    var newestDateTimeFound = DateTime.MinValue;
                    if (pak.GetFileByName("game/worlds/main_world/world.xml", out var worldInfo))
                    {
                        var worldTime = DateTime.FromFileTime(worldInfo.CreateTime);
                        foreach (var aaLauncherContainer in AAEmuLauncherBase.AllLaunchers)
                        {
                            if (!isArcheWorld && !string.IsNullOrWhiteSpace(aaLauncherContainer.MinimumVersion) &&
                                (worldTime > newestDateTimeFound) &&
                                (newestDateTimeFound < aaLauncherContainer.MinimumWorldDate) &&
                                (worldTime > aaLauncherContainer.MinimumWorldDate))
                            {
                                newestDateTimeFound = aaLauncherContainer.MinimumWorldDate;
                                res = aaLauncherContainer.ConfigName;
                            }

                            if (isArcheWorld &&
                                !string.IsNullOrWhiteSpace(aaLauncherContainer.MinimumVersionForWorld) &&
                                (worldTime > newestDateTimeFound) &&
                                (newestDateTimeFound < aaLauncherContainer.MinimumWorldDate) &&
                                (worldTime > aaLauncherContainer.MinimumWorldDate))
                            {
                                newestDateTimeFound = aaLauncherContainer.MinimumWorldDate;
                                res = aaLauncherContainer.ConfigName;
                            }
                        }
                    }

                    pak.ClosePak();
                }

            }
            catch
            {
                //
            }

            return res;
        }

        /// <summary>
        /// Tries to detect the revision number of a ArcheAge.exe file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Revision number, or 0 if there were errors</returns>
        public static ulong DetectRevision(string fileName)
        {
            if (!File.Exists(fileName))
                return 0;

            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    return DetectRevision(stream);
            }
            catch
            {
                //
            }
            return 0;
        }

        /// <summary>
        /// Tries to detect the revision number of a stream of ArcheAge.exe file
        /// Credits: https://github.com/Ingramz/archeage/tree/master/archeage.exe-revision-finder
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>Revision number, or 0 if there were errors</returns>
        public static ulong DetectRevision(Stream stream)
        {
            try
            {
                using (PEReader peReader = new PEReader(stream))
                {

                    foreach (var h in peReader.PEHeaders.SectionHeaders)
                    {
                        // Normally it's in the .xlgames section
                        if (h.Name != ".xlgames")
                            continue;

                        // The block is normally 512 bytes
                        if (h.SizeOfRawData != 512)
                            continue;

                        // Try to grab Revision
                        try
                        {
                            stream.Seek(h.PointerToRawData, SeekOrigin.Begin);
                            byte[] buffer = new byte[8];
                            stream.Read(buffer, 0, 8);

                            var tryRevision = BitConverter.ToUInt64(buffer, 0);
                            return tryRevision;
                        }
                        catch
                        {
                            return 0;
                        }
                    }
                }
            }
            catch
            {
                // Probably invalid executable
            }
            return 0;
        }

        public static string GuessDocumentsFolder(string archeAgeExe, string launcherExe)
        {
            // Changes made base on suggestion by Giawriav using ChatGPT
            // Also now just check for a trailing null char for valid strings
            var defaultFolderName = "ArcheAge";
            var allowedFolders = new Dictionary<string, List<string>>();
            // Known/supported documents folders and their aliases
            allowedFolders.Add("archeage", new List<string> { "archeage" });
            allowedFolders.Add("archeworld", new List<string> { "archeworld" });
            allowedFolders.Add("aaemu", new List<string> { "aaemu" });
            allowedFolders.Add("aaclassic", new List<string> { "aaclassic", "archeage classic" });
            allowedFolders.Add("aafree", new List<string> { "aafree" });
            allowedFolders.Add("archerage", new List<string> { "archerage" });
            allowedFolders.Add("aagenesis", new List<string> { "aagenesis" });

            /*
            var documentConfigFile = Path.Combine(Path.GetDirectoryName(launcherExe) ?? "", "documents.folders");
            if (File.Exists(documentConfigFile))
                allowedFolders = File.ReadAllLines(documentConfigFile).Select(f => f.ToLowerInvariant().Trim()).ToList();
            */
            var maxFolderSize = allowedFolders.Keys.Max(f => f.Length) + 4;
            try
            {
                if (File.Exists(archeAgeExe))
                {
                    // First check for a known documents folder in the executable bytes
                    byte[] exeBytes = File.ReadAllBytes(archeAgeExe);
                    for (int i = 0; i < exeBytes.Length - maxFolderSize; i++)
                    {
                        string chunk = Encoding.ASCII.GetString(exeBytes, i, maxFolderSize).ToLowerInvariant();
                        foreach (var entry in allowedFolders)
                        {
                            var folder = entry.Key;
                            var aliasList = entry.Value;
                            if (folder == "archeage")
                                continue;
                            foreach (var alias in aliasList)
                            {
                                if (chunk.StartsWith(alias + '\0'))
                                {
                                    return folder;
                                }
                            }
                        }
                    }

                    // If it's not found in the executable, check for a known documents folder from the rest of the path
                    var pathSplit = archeAgeExe.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                    if (pathSplit.Length > 1)
                    {
                        // Start at -2 to exclude the exe itself
                        for (var i = pathSplit.Length - 2; i >= 0; i--)
                        {
                            var s = pathSplit[i].ToLowerInvariant();
                            foreach (var entry in allowedFolders)
                            {
                                var folder = entry.Key;
                                var aliasList = entry.Value;
                                if (folder == "archeage")
                                    continue;
                                foreach (var alias in aliasList)
                                {

                                    if (s.Contains(alias.ToLowerInvariant()))
                                    {
                                        return folder;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return defaultFolderName;
            }

            // Fallback: use the exe file name
            return Path.GetFileNameWithoutExtension(archeAgeExe) ?? defaultFolderName;
        }
    }


}