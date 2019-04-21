using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using AAEmu.Launcher.Basic;
using AAEmu.Launcher.Trion12;

namespace AAEmu.Launcher.Trion35
{
    public class Trion_3_5_Launcher: AAEmu.Launcher.Trion12.Trion_1_2_Launcher
    {

        public override bool InitializeForLaunch()
        {
            var res = base.InitializeForLaunch();
            LaunchArguments += " -uid " + UserName;
            return res;
        }

    }
}
