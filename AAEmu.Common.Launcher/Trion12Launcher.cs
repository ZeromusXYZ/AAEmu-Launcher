using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using AAEmu.Launcher.Basic;

namespace AAEmu.Launcher.Trion12
{
    [AALauncher("trino_1_2","Trion 1.2","1.2", "", "20140708")]
    public class Trion_1_2_Launcher: AAEmuLauncherBase
    {

        public bool UseCustomTicketData { get; set; }
        public string CustomTicketData { get; set; }
        public int HandleID1FileMap { get; protected set; }
        public int HandleID2Event { get; protected set; }
        public byte[] encryptionKey ;

        public Trion_1_2_Launcher() : base()
        {
            UseCustomTicketData = false;
            CustomTicketData = "";
            HandleID1FileMap = 0;
            HandleID2Event = 0;

            // This fixed value was used by ToolsA when testing
            // var key = new byte[8] { 0x29, 0x6B, 0xD6, 0xEB, 0x2C, 0xA9, 0x03, 0x21 };
            // We use random values instead
            encryptionKey = new byte[8];
            Random r = new Random();
            r.NextBytes(encryptionKey);
        }

        public override bool InitializeForLaunch()
        {
            var res = base.InitializeForLaunch();
            var customTicketName = "customticket.xml";

            if (File.Exists(customTicketName))
            {
                CustomTicketData = File.ReadAllText(customTicketName);
                UseCustomTicketData = !string.IsNullOrWhiteSpace(CustomTicketData);
            }

            string languageArgs = "";
            if (Locale != "")
                languageArgs += " -lang " + Locale;
               
            if (CreateTrinoHandleIDs() == false)
                res = false;
            string handleArgs = "-handle " + HandleID1FileMap.ToString("X8") + ":" + HandleID2Event.ToString("X8");
            // archeage.exe -t -auth_ip 127.0.0.1 -auth_port 1237 -handle 00000000:00000000 -lang en_us
            LaunchArguments = "-t +auth_ip " + LoginServerAdress + " -auth_port " + LoginServerPort.ToString() + " " + handleArgs + languageArgs;

            return res;
        }

        public override bool FinalizeLaunch()
        {
            if ((HandleID1FileMap == 0) || (HandleID2Event == 0))
                return base.FinalizeLaunch();

            if (RunningProcess != null)
            {
                // Wait up to 30 seconds before continuing
                for(int i = 0;i < 30;i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    if (RunningProcess.HasExited)
                        break;
                    if (RunningProcess.Responding)
                        break;
                }
            }

            // This will likely toss a access denied error for whatever reason, but let's keep it here for now
            var waitRes = Win32.WaitForSingleObject((IntPtr)HandleID1FileMap, 15000);
            if (waitRes != 0)
            {
                // MessageBox.Show("WaitForSingleObject\r\nResult: " + waitRes.ToString("X8")+"\r\nGetLastWin32Error:"+ Marshal.GetLastWin32Error().ToString("X8"));
            }

            return base.FinalizeLaunch();
        }

        public override void Dispose()
        {
            CleanupHandles();

            base.Dispose();
        }

        public bool CreateTrinoHandleIDs()
        {
            HandleID1FileMap = 0;
            HandleID2Event = 0;

            // Not sure if we actually need this signature part or not
            string stringForSignature = "dGVzdA==";

            string ticketDataString ;
            if (UseCustomTicketData)
            {
                ticketDataString = CustomTicketData;
            }
            else
            {
                ticketDataString = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"yes\"?>";
                ticketDataString += "<authTicket version = \"1.2\">";
                ticketDataString += "<storeToken>1</storeToken>";
                ticketDataString += "<username>" + UserName + "</username>";
                ticketDataString += "<password>" + _passwordHash + "</password>";
                ticketDataString += "</authTicket>";
            }

            // TFIR is the header for this ?
            var ticket = "TFIR" + stringForSignature + '\n' + ticketDataString;
            var ticketBytes = Encoding.UTF8.GetBytes(ticket);
            var ticketEncrypted = AAEmu.Launcher.Basic.RC4.Encrypt(encryptionKey, ticketBytes);

            // File.WriteAllText("this_ticket.txt", ticketDataString);

            //------ IntPtr CreateFileMappingHandle(string ticketString,string signatureString)

            // MSDN Documentation says SECURITY_ATTRIBUTES needs to be set (not null) for child processes to be able to inherit the handle from CreateEvent
            Win32.SECURITY_ATTRIBUTES sa = new Win32.SECURITY_ATTRIBUTES
            {
                nLength = Marshal.SizeOf(typeof(Win32.SECURITY_ATTRIBUTES)),
                lpSecurityDescriptor = IntPtr.Zero,
                bInheritHandle = true
            };

            IntPtr sa_pointer = Marshal.AllocHGlobal(sa.nLength);
            Marshal.StructureToPtr(sa, sa_pointer, false);

            uint maxMapSize = 4096; // TODO: 0x20000 or 0x1000
            maxMapSize = (uint)ticketEncrypted.Length + 0xC;
            //maxMapSize = (uint)ticketDataString.Length + 0xc;

            var credentialFileMapHandle = Win32.CreateFileMappingW(
                Win32.INVALID_HANDLE_VALUE,
                sa_pointer, // IntPtr.Zero, //sa_pointer,
                FileMapProtection.PageReadWrite,
                0,
                maxMapSize,
                "archeage_auth_ticket_map");

            //Marshal.FreeHGlobal(sa_pointer);

            if (credentialFileMapHandle == IntPtr.Zero)
            {
                //Console.WriteLine("Failed to create credential file mapping");
                Marshal.FreeHGlobal(sa_pointer);
                return false;
            }

            var fileMapViewPointer = Win32.MapViewOfFile(credentialFileMapHandle, FileMapAccess.FileMapAllAccessFull, 0, 0, maxMapSize);

            if (fileMapViewPointer == IntPtr.Zero)
            {
                //Console.WriteLine("Failed to create credential file mapping view");
                Win32.CloseHandle(credentialFileMapHandle);
                Marshal.FreeHGlobal(sa_pointer);
                return false;
            }

            //--- EncryptFileMapData(fileMapViewHandle, ticketString, signatureString);

            // Use a temporary memorystream for ease
            MemoryStream ms = new MemoryStream();
            var writer = new BinaryWriter(ms);
            writer.Write(encryptionKey, 0, encryptionKey.Length);
            Int32 ticketSize = ticketEncrypted.Length;
            writer.Write((int)ticketSize);
            writer.Write(ticketEncrypted);
            ms.Position = 0;
            var result = new byte[ms.Length];
            ms.Read(result, 0, (int)ms.Length);

            // Copy data to MemoryMappedFile
            // Structure
            // 8 bytes: RC4 Key
            // 4 bytes: Actual Data Size
            // ? bytes: Encrypted data
            var pointer = Marshal.AllocHGlobal(result.Length);
            Marshal.Copy(result, 0, pointer, result.Length);
            Win32.MemCpy(fileMapViewPointer, pointer, (uint)result.Length);
            Marshal.FreeHGlobal(pointer);

            writer.Dispose();
            ms.Dispose();

            //-----------
            if (credentialFileMapHandle == IntPtr.Zero)
            {
                // TODO ...
                // Win32.CloseHandle(credentialEvent);
                Marshal.FreeHGlobal(sa_pointer);
                return false;
            }

            var credentialEvent = Win32.CreateEventW(sa_pointer, true, false, "archeage_auth_ticket_event");

            Marshal.FreeHGlobal(sa_pointer);

            if (credentialEvent == IntPtr.Zero)
            {
                // Console.WriteLine("Failed to create credential event");
                return false;
            }

            HandleID1FileMap = credentialFileMapHandle.ToInt32();
            HandleID2Event = credentialEvent.ToInt32();

            return ((HandleID1FileMap != 0) && (HandleID2Event != 0));
        }

        protected void CleanupHandles()
        {
            try
            {
                if (HandleID1FileMap != 0)
                {
                    Win32.UnmapViewOfFile((IntPtr)HandleID1FileMap);
                    Win32.CloseHandle((IntPtr)HandleID1FileMap);
                }
                if (HandleID2Event != 0)
                {
                    // Remove it ? 
                    Win32.CloseHandle((IntPtr)HandleID2Event);
                }
            }
            catch
            {
            }
            HandleID1FileMap = 0;
            HandleID2Event = 0;
        }
    }
}
