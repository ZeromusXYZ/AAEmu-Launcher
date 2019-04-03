using AAEmu.Launcher.LauncherBase;

namespace AAEmu.Launcher.MailRu10
{
    public class MailRu_1_0_Launcher: AAEmuLauncherBase
    {
        public override bool InitializeForLaunch()
        {
            launchArguments = "-r +auth_ip " + loginServerAdress + ":" + loginServerPort.ToString() + " -uid " + userName + " -token " + _passwordHash;
            return true;
        }
    }
}
