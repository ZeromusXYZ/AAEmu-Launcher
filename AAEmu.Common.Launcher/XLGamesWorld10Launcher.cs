using AAEmu.Launcher.Basic;
using System;

namespace AAEmu.Launcher.XLWorld10
{
    [AALauncher("xlworld_1_0", "World 1.0","", "1.0", "20220901")]
    public class XLWorld_1_0_Launcher: AAEmuLauncherBase
    {
        private const string TestToken = "SNsXRIVRnEqZFisW";
        // 0123456789abcdef  -  16 bytes
        // Zq5RaSSX7/3N9.5Q  -  5a71355261535358372f334e392e3551  
        // .URo1628rW7uwlRp  -  2e55526f3136323872573775776c5270
        // SNsXRIVRnEqZFisW  -  534e7358524956526e45715a46697357


        public string Token { get; set; }

        public override bool InitializeForLaunch()
        {
            if (string.IsNullOrWhiteSpace(Token) || (Token.Length != 16))
                Token = TestToken;
            {
                // Generate a random value
                Token = TestToken;
            }

            LaunchArguments = Token + " -k";
            LaunchArguments = "";

            if (Locale != "")
                LaunchArguments += " -locale " + Locale;

            // -instant_token might be related to the license.cfg file ?
            LaunchArguments += " -instant_token -region";

            return true;
        }
    }
}
