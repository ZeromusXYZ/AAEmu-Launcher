using AAEmu.Launcher.Basic;
using System;

namespace AAEmu.Launcher.Kakao80
{
    [AALauncher("kakao_8_0", "Kakao 8.0","8.0", "0.1", "20211202")]
    public class Kakao_8_0_Launcher: AAEmuLauncherBase
    {
        public string AuthToken { get; set; } = System.Guid.Empty.ToString();
        public string TimeOffsetValue { get; set; } = "-120";

        public override bool InitializeForLaunch()
        {

            if (string.IsNullOrWhiteSpace(TimeOffsetValue))
                TimeOffsetValue = (DateTime.UtcNow - DateTime.Now).TotalMinutes.ToString();

            if (string.IsNullOrWhiteSpace(AuthToken))
            {
                // Generate a random value
                var guid = System.Guid.NewGuid();
                AuthToken = guid.ToString();
            }

            LaunchArguments = "-t +auth_ip " + LoginServerAdress + " -auth_port " + LoginServerPort.ToString() + " -authtoken " + AuthToken ;

            if (Locale != "")
                LaunchArguments += " -lang " + Locale;

            if (TimeOffsetValue != string.Empty)
                LaunchArguments += " -time_offset " + TimeOffsetValue ;

            return true;
        }
    }
}
