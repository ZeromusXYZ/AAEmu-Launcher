using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using AAEmu.Launcher.Basic;
using AAEmu.Launcher.Trion12;
using System.Diagnostics;

namespace AAEmu.Launcher.Trion70
{
    [AALauncher("trino_7_0", "Trion 7.0", "7.0", "", "20210318")]
    public class Trion_7_0_Launcher : AAEmu.Launcher.Trion60.Trion_6_0_Launcher
    {
        // Example args (from Trion 7.5)
        // \launch_game.exe -eac_launcher_settings settings_32.json -t +auth_ip 46.253.159.98 -auth_port 1237 -handle 0000097C:00000954 -lang en_us -time_offset 120 -glyphpid 5644 -lang en_us
        // Notable differences from 6.x :
        // launching through EAC launcher, and settings arguments for said launcher
        // 

        public string EACPath = string.Empty;
        private readonly string EACLaunchName = "launch_game.exe";

        public bool DetectEAC()
        {
            var gameRoot = Path.GetDirectoryName(Path.GetDirectoryName(GameExeFilePath));
            if (File.Exists(Path.Combine(gameRoot, EACLaunchName)))
                EACPath = Path.Combine(gameRoot, EACLaunchName);
            else
                EACPath = string.Empty;
            return !string.IsNullOrEmpty(EACPath);
        }

        public override bool InitializeForLaunch()
        {
            var res = base.InitializeForLaunch();
            if (DetectEAC())
            {
                GameExeFilePath = EACPath;
                LaunchArguments = "-eac_launcher_settings settings_32.json " + LaunchArguments ;
                // Console.WriteLine("Detected EAC at {0}, modifying launching program path", EACPath);
            }
            // LaunchArguments += " -uid " + UserName;
            if (TimeOffsetValue != string.Empty)
                LaunchArguments += " -time_offset " + TimeOffsetValue + " " ;

            LaunchArguments += " -glyphpid " + Process.GetCurrentProcess().Id.ToString() + " " ;

            return res;
        }

    }
}
