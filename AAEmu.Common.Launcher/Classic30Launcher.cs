using System;
using AAEmu.Launcher.Basic;

namespace AAEmu.Launcher.Classic30
{
    [AALauncher("classic_3_0", "Classic 3.x+", 304031, "3.0", "", "20241001")]
    public class Classic_3_0_Launcher : AAEmuLauncherBase
    {
        public override bool InitializeForLaunch()
        {
            LaunchArguments = "-r +auth_ip " + LoginServerAdress + ":" + LoginServerPort.ToString() + " -uid " + UserName + " -token " + PasswordHash + " +cl_serveraddr " + LoginServerAdress;
            return true;
        }

        public override bool IsFlavorAllowed(string flavor)
        {
            // Only allow for AAClassic
            return flavor.Equals(@"AAClassic", StringComparison.OrdinalIgnoreCase);
        }
    }
}