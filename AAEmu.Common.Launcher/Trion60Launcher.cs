using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using AAEmu.Launcher.Basic;
using AAEmu.Launcher.Trion12;
using System.Diagnostics;

namespace AAEmu.Launcher.Trion60
{
    [AALauncher("trino_6_0", "Trion 6.0","6.0", "", "20190919")]
    public class Trion_6_0_Launcher: AAEmu.Launcher.Trion12.Trion_1_2_Launcher
    {
        // Example args (from Trion 6.0.7.0 PTS) launched at 17:07 CET (15:07 UTC)
        // bin64\archeage.exe -t +auth 208.94.24.110 -auth_port 1237 -handle 000006c8:00000948 -lang en_us -time_offset 300 -glyphpid 6328 -lang en_us
        // Notable differences from 3.x :
        // -uid removed
        // -glyphpid added that points to the launcher's Process ID, this is likely used to work with Easy Anti-Cheat
        // 

        public string TimeOffsetValue = "300";

        public override bool InitializeForLaunch()
        {
            var res = base.InitializeForLaunch();
            // LaunchArguments += " -uid " + UserName;
            if (TimeOffsetValue != string.Empty)
                LaunchArguments += " -time_offset " + TimeOffsetValue + " " ;

            LaunchArguments += " -glyphpid " + Process.GetCurrentProcess().Id.ToString() + " " ;

            return res;
        }

    }
}
