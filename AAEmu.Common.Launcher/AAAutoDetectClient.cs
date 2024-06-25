// DetectRevision function Credits: https://github.com/Ingramz/

using System;
using System.IO;
using System.Diagnostics;
using AAPacker;
using System.Reflection.PortableExecutable;

namespace AAEmu.Launcher.Basic
{
    public class AAAutoDetectClient
    {
        public static string GuessLauncher(string archeAgeExeFile)
        {
            // Check if main exe and game_pak exist
            if (!File.Exists(archeAgeExeFile))
                return string.Empty;

            var isArcheWorld = (archeAgeExeFile.ToLower().Contains("archeworld")) ;
            var pakFileName = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(archeAgeExeFile)), "game_pak");
            if (!File.Exists(pakFileName))
                return string.Empty;

            var res = string.Empty;
            try
            {
                // Check by .exe version
                var versionInfo = FileVersionInfo.GetVersionInfo(archeAgeExeFile);
                string version = string.Join(".",versionInfo.FileVersion.Replace(" ","").Split(',')); // Will typically return "1.0.0.0" in your case

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

                        if (!string.IsNullOrWhiteSpace(compareVersion) && (string.Compare(version, compareVersion, true) > 0) && (string.Compare(version, newestValidVersion, true) > 0))
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
                            if (!isArcheWorld && !string.IsNullOrWhiteSpace(aaLauncherContainer.MinimumVersion) && (worldTime > newestDateTimeFound) && (newestDateTimeFound < aaLauncherContainer.MinimumWorldDate) && (worldTime > aaLauncherContainer.MinimumWorldDate))
                            {
                                newestDateTimeFound = aaLauncherContainer.MinimumWorldDate;
                                res = aaLauncherContainer.ConfigName;
                            }

                            if (isArcheWorld && !string.IsNullOrWhiteSpace(aaLauncherContainer.MinimumVersionForWorld) && (worldTime > newestDateTimeFound) && (newestDateTimeFound < aaLauncherContainer.MinimumWorldDate) && (worldTime > aaLauncherContainer.MinimumWorldDate))
                            {
                                newestDateTimeFound = aaLauncherContainer.MinimumWorldDate;
                                res = aaLauncherContainer.ConfigName;
                            }
                        }
                    }
                    pak.ClosePak();
                }

            }
            catch { }

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
    }


}