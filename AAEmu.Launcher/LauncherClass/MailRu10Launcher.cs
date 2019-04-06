using AAEmu.Launcher.Basic;

namespace AAEmu.Launcher.MailRu10
{
    public class MailRu_1_0_Launcher: AAEmuLauncherBase
    {
        public override bool InitializeForLaunch()
        {
            LaunchArguments = "-r +auth_ip " + LoginServerAdress + ":" + LoginServerPort.ToString() + " -uid " + UserName + " -token " + _passwordHash;
            return true;
        }
    }
}
