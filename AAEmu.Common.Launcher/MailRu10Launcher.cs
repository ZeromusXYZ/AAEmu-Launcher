using AAEmu.Launcher.Basic;
using System;

namespace AAEmu.Launcher.MailRu10
{
    [AALauncher("mailru_1_0", "Mail.ru", 0, "0.5", "", "20130423")]
    public class MailRu_1_0_Launcher : AAEmuLauncherBase
    {
        public override bool InitializeForLaunch()
        {
            LaunchArguments = "-r +auth_ip " + LoginServerAdress + ":" + LoginServerPort.ToString() + " -uid " + UserName + " -token " + PasswordHash;
            return true;
        }

        public override bool IsFlavorAllowed(string flavor)
        {
            return flavor.Equals(@"ArcheAge", StringComparison.OrdinalIgnoreCase) ||
                   flavor.Equals(@"ArcheRage", StringComparison.OrdinalIgnoreCase) ||
                   flavor.Equals(@"AAFree", StringComparison.OrdinalIgnoreCase);
        }
    }
}