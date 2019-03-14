using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using ChunkedMemStream;

namespace XLPakTool
{
    public delegate void XLPakToolProgress(long progress);

    public class PatchFileInfo: IComparable<PatchFileInfo>
    {
        public string Path { get; set; }
        public long Size { get; set; }
        public string Hash { get; set; } // TODO md5
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }

        public int CompareTo(PatchFileInfo other)
        {
            return Path.CompareTo(other.Path);
        }
}

    public class XLTreeDictionary
    {
        public class XlFile
        {
            public string Path { get; set; }
            public long Size { get; set; }
            public string Hash { get; set; } // TODO md5
            public DateTime CreateTime { get; set; }
            public DateTime ModifyTime { get; set; }

            public XlFile(string path, XLPack.pack_stat2 stat)
            {
                Path = path;
                CreateTime = DateTime.FromFileTime(stat.stat.creationTime);
                ModifyTime = DateTime.FromFileTime(stat.stat.modifiedTime);
                Hash = BitConverter.ToString(stat.digest.md5).Replace("-", "").ToLower();
                Size = stat.length;
            }
        }

        public string Path { get; set; }
        public XLTreeDictionary Parent { get; set; }
        public List<XLTreeDictionary> Directories { get; set; }
        public List<XlFile> Files { get; set; }

        public XLTreeDictionary(string path)
        {
            Path = path;
            Directories = new List<XLTreeDictionary>();
            Files = new List<XlFile>();
        }
    }

    public class XLPack
    {
        public static XLPakToolProgress XLPakToolProgressCallBackFunction;

        private static string _globalPath = "/master/";
        private static string _fsPath;
        private static IntPtr _fsHandler;
        private static IntPtr _masterHandler;
        private static IntPtr _logHandler;
        public static bool isXLGamePackMounted = false;

        [DllImport("xlpack.dll", EntryPoint = "?ApplyPatchPak@@YA_NPBD0@Z", CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ApplyPatchPak([MarshalAs(UnmanagedType.LPStr)] string s1, [MarshalAs(UnmanagedType.LPStr)] string s2);

        [DllImport("xlpack.dll", EntryPoint = "?Copy@@YA_NPBD0@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Copy([MarshalAs(UnmanagedType.LPStr)] string from, [MarshalAs(UnmanagedType.LPStr)] string to);

        [DllImport("xlpack.dll", EntryPoint = "?CopyDir@@YA_NPBD0@Z", CharSet = CharSet.Ansi)]
        public static extern bool CopyDir([MarshalAs(UnmanagedType.LPStr)] string from, [MarshalAs(UnmanagedType.LPStr)] string to);

        [DllImport("xlpack.dll", EntryPoint = "?CreateFileSystem@@YA_NXZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CreateFileSystem();

        [DllImport("xlpack.dll", EntryPoint = "?DestroyFileLogHandler@@YAXPAX@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyFileLogHandler(IntPtr lp1);

        [DllImport("xlpack.dll", EntryPoint = "?DestroyFileSystem@@YAXXZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyFileSystem();

        [DllImport("xlpack.dll", EntryPoint = "?FDelete@@YA_NPBD@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool FDelete([MarshalAs(UnmanagedType.LPStr)] string where);

        [DllImport("xlpack.dll", EntryPoint = "?FindClose@@YAHH@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FindClose(int i);

        [DllImport("xlpack.dll", EntryPoint = "?FindFirst@@YAHPBDPAUafs_finddata@@@Z", CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int FindFirst([MarshalAs(UnmanagedType.LPStr)] string file, ref afs_finddata fd);

        [DllImport("xlpack.dll", EntryPoint = "?FindNext@@YAHHPAUafs_finddata@@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FindNext(int i, ref afs_finddata fd);

        [DllImport("xlpack.dll", EntryPoint = "?GetFileName@@YAPBDPBUafs_finddata@@@Z", CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetFileName(ref afs_finddata fd);

        [DllImport("xlpack.dll", EntryPoint = "?IsDirectory@@YA_NPBUafs_finddata@@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsDirectory(ref afs_finddata fd);

        [DllImport("xlpack.dll", EntryPoint = "?IsFileExist@@YA_NPBD@Z", CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern bool IsFileExist([MarshalAs(UnmanagedType.LPStr)] string file);

        [DllImport("xlpack.dll", EntryPoint = "?Mount@@YAPAXPBD0_N@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mount([MarshalAs(UnmanagedType.LPStr)] string where, [MarshalAs(UnmanagedType.LPStr)] string which,
            [MarshalAs(UnmanagedType.Bool)] bool editable);
        
        [DllImport("xlpack.dll", EntryPoint = "?Unmount@@YA_NPBD@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Unmount([MarshalAs(UnmanagedType.LPStr)] string where);
        
        [DllImport("xlpack.dll", EntryPoint = "?Unmount@@YA_NPAX@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Unmount(IntPtr handler);

        [DllImport("xlpack.dll", EntryPoint = "?SetFileLogHandler@@YAPAXPBDP6AX0ZZ@Z", CharSet = CharSet.Ansi,
            CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SetFileLogHandler([MarshalAs(UnmanagedType.LPStr)] string s, Func f);

        [DllImport("xlpack.dll", EntryPoint = "?DeleteDir@@YA_NPBD@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool DeleteDir([MarshalAs(UnmanagedType.LPStr)] string path);
        
        [DllImport("xlpack.dll", EntryPoint = "?FOpen@@YAPAUFile@@PBD0@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FOpen([MarshalAs(UnmanagedType.LPStr)] string path, [MarshalAs(UnmanagedType.LPStr)] string mode);
        
        [DllImport("xlpack.dll", EntryPoint = "?FClose@@YAXAAPAUFile@@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FClose(ref IntPtr filePosition);
        
        [DllImport("xlpack.dll", EntryPoint = "?FGetStat@@YA_NPAUFile@@PAUpack_stat_t@@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool FGetStat(IntPtr filePosition, ref pack_stat_t stat);
        
        [DllImport("xlpack.dll", EntryPoint = "?FGetStat@@YA_NPAUFile@@PAUpack_stat2@@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool FGetStat(IntPtr filePosition, ref pack_stat2 stat);
        
        [DllImport("xlpack.dll", EntryPoint = "?FSize@@YA_JPAUFile@@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern long FSize(IntPtr filePosition);

        [DllImport("xlpack.dll", EntryPoint = "?FGetStat@@YA_NPAUFile@@PAUpack_stat2@@@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool FGetMD5(IntPtr filePosition, ref afs_md5_ctx md5);

        [DllImport("xlpack.dll", EntryPoint = "?FRead@@YA_JPAUFile@@PAD_J@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int FRead(IntPtr filePosition, IntPtr buffer, Int64 size);

        [DllImport("xlpack.dll", EntryPoint = "?FSetMD5@@YA_NPAUFile@@QBD@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool FSetMD5(IntPtr filePosition, ref afs_md5_ctx md5);

        [StructLayout(LayoutKind.Explicit)]
        public struct afs_finddata
        {
            [FieldOffset(0)] public long Offset;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct File
        {
            [FieldOffset(0)] public uint pntr;
            [FieldOffset(4)] public uint cnt;
            [FieldOffset(8)] public uint based; // base
            [FieldOffset(12)] public uint flag;
            [FieldOffset(16)] public uint file;
            [FieldOffset(20)] public uint charbuf;
            [FieldOffset(24)] public uint bufsize;
            [FieldOffset(28)] public uint tmpfname;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct XlFileInfo
        {
            [FieldOffset(0)] public uint dwFileAttributes;
            [FieldOffset(4)] public ulong ftCreationTime;
            [FieldOffset(12)] public ulong ftLastAccessTime;
            [FieldOffset(20)] public ulong ftLastWriteTime;
            [FieldOffset(28)] public uint dwVolumeSerialNumber;
            [FieldOffset(32)] public uint nFileSizeHigh;
            [FieldOffset(36)] public uint nFileSizeLow;
            [FieldOffset(40)] public uint nNumberOfLinks;
            [FieldOffset(44)] public uint nFileIndexHigh;
            [FieldOffset(48)] public uint nFileIndexLow;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct afs_md5_ctx
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)] [FieldOffset(0)]
            public byte[] md5;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct pack_stat_t
        {
            [FieldOffset(0)] public long creationTime;
            [FieldOffset(8)] public long modifiedTime;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct pack_stat2
        {
            [FieldOffset(0)] public pack_stat_t stat;
            [FieldOffset(16)] public long length;
            [FieldOffset(24)] public afs_md5_ctx digest;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void Func(params string[] values);


        public static void LogHandler(params string[] p)
        {
            foreach (var str in p)
                Console.WriteLine(str);
        }

        public static bool InitXLGamePakFileSystem()
        {
            if (!XLPack.CreateFileSystem())
            {
                return false;
            }
            _logHandler = XLPack.SetFileLogHandler(".patch/pack.log", LogHandler);

            return true;
        }

        public static void MountXLGamePakFileSystem(string game_pak_Path)
        {
            var pack = new FileInfo(game_pak_Path);

            _fsPath = pack.DirectoryName;

            _fsHandler = XLPack.Mount("/fs", _fsPath, true);
            _masterHandler = XLPack.Mount("/master", game_pak_Path, true);

            isXLGamePackMounted = true;
        }

        public static List<(string, bool)> GetFiles(string path)
        {
            var result = new List<(string, bool)>();

            var file = path + "*";
            var fd = new XLPack.afs_finddata();
            var findHandle = XLPack.FindFirst(file, ref fd);
            if (findHandle != -1)
            {
                do
                {
                    var fileName = Marshal.PtrToStringAnsi(XLPack.GetFileName(ref fd));
                    var tempFile = path + fileName;
                    var isDirectory = !XLPack.IsFileExist(tempFile);
                    result.Add((tempFile, isDirectory));
                } while (XLPack.FindNext(findHandle, ref fd) != -1);
            }

            XLPack.FindClose(findHandle);
            return result;
        }

        public static bool IsDirectory(string path)
        {
            if (XLPack.IsFileExist(path))
                return false;
            var fd = new XLPack.afs_finddata();
            var first = XLPack.FindFirst(path, ref fd);
            var flag = first != -1;
            XLPack.FindClose(first);
            return flag;
        }

        public static bool IsPathExist(string path)
        {
            if (XLPack.IsFileExist(path))
                return true;
            var fd = new XLPack.afs_finddata();
            var first = XLPack.FindFirst(path, ref fd);
            var exist = first != -1;
            XLPack.FindClose(first);
            return exist;
        }

        public static void UnmountXLPackFileSystem()
        {
            XLPack.Unmount(_masterHandler);
            XLPack.Unmount(_fsHandler);
            isXLGamePackMounted = false;
        }

        public static void Destroy()
        {
            UnmountXLPackFileSystem();
            XLPack.DestroyFileLogHandler(_logHandler);
            XLPack.DestroyFileSystem();
        }

        public static XLTreeDictionary.XlFile GetFileState(string path)
        {
            if (!XLPack.IsFileExist(path))
                return null;
            var position = XLPack.FOpen(path, "r");
            var stat = new XLPack.pack_stat2();
            var res = XLPack.FGetStat(position, ref stat);
            XLPack.FClose(ref position);
            return res ? new XLTreeDictionary.XlFile(path, stat) : null;
        }

        public static void ExportDir(XLTreeDictionary thisDir, ref List<string> stringList)
        {
            string thisPath = thisDir.Path + "/";

            var files = GetFiles(thisDir.Path + "/");
            foreach (var (file, isDirectory) in files)
            {

                // Trim the "/master/" part of the file = 8 chars, it's always the same anyway
                string thisFile = file.Remove(0, 8);

                if (isDirectory)
                {
                    var folder = new XLTreeDictionary(file) { Parent = thisDir };
                    stringList.Add(thisFile + ";-1;;;;");
                    ExportDir(folder, ref stringList);
                }
                else
                {
                    var temp = GetFileState(file);
                    if (temp != null)
                    {
                        stringList.Add(thisFile + ";" +
                            temp.Size.ToString() + ";" +
                            temp.Hash + ";" +
                            temp.CreateTime.ToString("yyyyMMdd-HHmmss") + ";" +
                            temp.ModifyTime.ToString("yyyyMMdd-HHmmss")
                            );
                    }
                }
            }

        }

        public static void ExportDirXL(XLTreeDictionary thisDir, ref List<PatchFileInfo> fileList)
        {
            string thisPath = thisDir.Path + "/";
            string nullHash = "";
            nullHash = nullHash.PadRight(32, '0');

            var files = GetFiles(thisDir.Path + "/");
            foreach (var (file, isDirectory) in files)
            {

                // Trim the "/master/" part of the file = 8 chars, it's always the same anyway
                string thisFile = file.Remove(0, 8);

                if (isDirectory)
                {
                    var folder = new XLTreeDictionary(file) { Parent = thisDir };
                    //fileList.Add(thisFile + ";-1;;;;");
                    ExportDirXL(folder, ref fileList);
                }
                else
                {
                    // Manually read the info instead of using GetFileState
                    var fi = new PatchFileInfo();
                    fi.Path = thisFile;
                    var filePos = FOpen(thisFile, "r");
                    fi.Size = FSize(filePos);
                    pack_stat_t pst = new pack_stat_t();
                    FGetStat(filePos, ref pst);
                    fi.CreateTime = DateTime.FromFileTime(pst.creationTime);
                    fi.ModifyTime = DateTime.FromFileTime(pst.modifiedTime);

                    afs_md5_ctx md5info = new afs_md5_ctx();
                    if (XLPack.FGetMD5(filePos, ref md5info))
                    {
                        fi.Hash = BitConverter.ToString(md5info.md5).Replace("-", "").ToLower();
                    }
                    FClose(ref filePos);

                    if (fi.Hash == nullHash)
                    {
                        // We encountered a missing hash, add it !
                        if (ReCalculateFileMD5(thisFile))
                        {
                            fi.Hash = GetFileMD5(thisFile);
                        }
                    }
                    fileList.Add(fi);
                    // add the new file's size to the "progress bar"
                    XLPakToolProgressCallBackFunction(fi.Size);
                }
            }

        }


        public static void ExportFileList()
        {
            Console.WriteLine("--- Init ExportFileList ---");
            var tree = new XLTreeDictionary("/master");

            List<string> sl = new List<string>();

            Console.WriteLine("--- Begin ExportFileList ---");
            ExportDir(tree, ref sl);
            Console.WriteLine("--- {0} items --- ", sl.Count);
            System.IO.File.WriteAllLines("export.csv", sl.ToArray());
            Console.WriteLine("--- End ExportFileList --- ");
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        static string GetMd5Hash(MD5 md5Hash, Stream input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(input);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, Stream input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string GetFileMD5(string path)
        {
            if (!XLPack.IsFileExist(path))
                return "";
            var position = XLPack.FOpen(path, "r");
            XLPack.afs_md5_ctx md5info = new XLPack.afs_md5_ctx();
            var res = XLPack.FGetMD5(position, ref md5info);
            XLPack.FClose(ref position);
            return res ? BitConverter.ToString(md5info.md5).Replace("-", "").ToLower() : "";
        }

        private static bool SetFileMD5(string path, string hash)
        {
            if (!XLPack.IsFileExist(path))
                return false;

            XLPack.afs_md5_ctx md5info = new XLPack.afs_md5_ctx();
            md5info.md5 = StringToByteArray(hash);
            var position = XLPack.FOpen(path, "r");
            // fsetmd5 00001111222233334444555566667777 /master/bin32/zlib1.dll
            var res = XLPack.FSetMD5(position, ref md5info);
            XLPack.FClose(ref position);
            return res;
        }

        private static bool ReCalculateFileMD5(string path)
        {
            if (!XLPack.IsFileExist(path))
                return false;

            var position = XLPack.FOpen(path, "r");
            long fileSize = XLPack.FSize(position);
            const int bufSize = 0x4000;
            byte[] buffer = new byte[bufSize];
            IntPtr bufPtr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            // TODO: Do this without reading the entire file into memory to calculate the MD5
            //       Maybe try to use the XLPack.DLL's MD5Init, MD5Update and MD5 Finalize functions ?
            // Using ChunkedMemoryStream instead of MemoryStream to hopefully avoid outofmemory errors on large files
            ChunkedMemoryStream ms = new ChunkedMemoryStream();
            long readTotalSize = 0;
            while (readTotalSize < fileSize)
            {
                long readSize = fileSize - readTotalSize;
                if (readSize > bufSize)
                {
                    readSize = bufSize;
                }
                XLPack.FRead(position, bufPtr, readSize);
                ms.Write(buffer, 0, (int)readSize); // readSize should never be out of int range, so it's safe to cast it
                readTotalSize += readSize;
            }
            XLPack.FClose(ref position);
            ms.Position = 0;
            MD5 md5Hash = MD5.Create();
            string md5String = GetMd5Hash(md5Hash, ms).Replace("-", "").ToLower();
            ms.Dispose();
            var res = SetFileMD5(path, md5String);
            return res;
        }



    }
}
