using System;
using System.Security.Cryptography;
using System.Text;
using AAEmu.Launcher.Basic;

namespace AAEmu.Launcher.Returns10
{
    /// <summary>
    /// CN ArcheAge Returns / AAEmu 10.0.2.x client launch.
    /// Password token = Base64(SHA-256(UTF-8)) as -strUserToken — same format
    /// AAEmu Login TokenAuthFlow + AutoAccount expect.
    /// Proven args (no -lang on this client):
    ///   -t -strUserToken=... -StrUserName=... -PASSPORTSVRIP=... -sIp=... -sPort=...
    ///   -serverId=... -selectedServerId=...
    /// </summary>
    [AALauncher("returns_10", "Returns 10.x+", 575, "10.0", "", "20240601")]
    public class Returns10Launcher : AAEmuLauncherBase
    {
        public int ServerId { get; set; } = 1;

        public override bool SetPassword(string userPassword)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(userPassword ?? string.Empty);
                var hash = SHA256.Create().ComputeHash(data);
                PasswordHash = Convert.ToBase64String(hash);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override bool InitializeForLaunch()
        {
            // MainForm always sets HShieldArgs to +acpxmk; strip for this client.
            HShieldArgs = string.Empty;

            var host = string.IsNullOrWhiteSpace(LoginServerAdress) ? "127.0.0.1" : LoginServerAdress.Trim();
            var port = LoginServerPort == 0 ? (ushort)1237 : LoginServerPort;
            var sid = ServerId <= 0 ? 1 : ServerId;
            LaunchArguments = string.Format(
                "-t -strUserToken={0} -StrUserName={1} -PASSPORTSVRIP={2} -sIp={2} -sPort={3} -serverId={4} -selectedServerId={4}",
                PasswordHash,
                UserName,
                host,
                port,
                sid);

            return true;
        }

        public override bool IsFlavorAllowed(string flavor)
        {
            // Only allow for AAClassic
            return flavor.Equals(@"ArcheAge", StringComparison.OrdinalIgnoreCase);
        }
    }
}
