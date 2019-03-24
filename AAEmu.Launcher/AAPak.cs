using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using SubStreamHelper;

namespace AAPakEditor
{
    
    // We don't use ths struct anymore, but I'm keeping it around for now
    [StructLayout(LayoutKind.Explicit)]
    public struct AAPakFileInfoStruct
    {
        /*
        getdstring NAME 0x108 MEMORY_FILE
        get OFFSET longlong MEMORY_FILE
        get SIZE longlong MEMORY_FILE
        get XSIZE longlong MEMORY_FILE     # used for encryption alignment?
        get ZSIZE long MEMORY_FILE         # ???
        getdstring DUMMY 16 MEMORY_FILE    # MD5 Hash
        get DUMMY1 long MEMORY_FILE        # ???
        get TIMESTAMP longlong MEMORY_FILE # Creation TIMESTAMP
        get TIMESTAMP longlong MEMORY_FILE # Modification TIMESTAMP
        get DUMMY2 longlong MEMORY_FILE    # ???
        */
        [FieldOffset(0x000)] [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x108)] public string name;
        [FieldOffset(0x108)] public Int64 offset;
        [FieldOffset(0x110)] public Int64 size;
        [FieldOffset(0x118)] public Int64 sizeDuplicate; // maybe compressed data size ? if used
        [FieldOffset(0x120)] public Int32 paddingSize; // ??
        [FieldOffset(0x124)] public byte[] md5;
        [FieldOffset(0x134)] public Int32 dummy1; // looks like padding, always 0 ?
        [FieldOffset(0x138)] public Int64 createTime;
        [FieldOffset(0x140)] public Int64 modifyTime;
        //[FieldOffset(0x138)] public System.Runtime.InteropServices.ComTypes.FILETIME createTime ;
        //[FieldOffset(0x140)] public System.Runtime.InteropServices.ComTypes.FILETIME modifyTime ;
        [FieldOffset(0x148)] public Int64 dummy2; // looks like padding, always 0 ?
    }

    /// <summary>
    /// File Details Block
    /// </summary>
    public class AAPakFileInfo : IComparable<AAPakFileInfo>
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x108)] public string name;
        public Int64 offset;
        public Int64 size;
        public Int64 sizeDuplicate; // maybe compressed data size ? if used
        public Int32 paddingSize; // ??
        public byte[] md5;
        public Int32 dummy1; // looks like padding, always 0 ?
        public Int64 createTime;
        public Int64 modifyTime;
        public Int64 dummy2; // looks like padding, always 0 ?

        public int CompareTo(AAPakFileInfo obj)
        {
            return this.name.CompareTo(obj.name);
        }
    }

    /// <summary>
    /// Pak Header Information
    /// </summary>
    public class AAPakFileHeader
    {
        private readonly byte[] key = new byte[] { 0x32, 0x1F, 0x2A, 0xEE, 0xAA, 0x58, 0x4A, 0xB4, 0x9A, 0x6C, 0x9E, 0x09, 0xD5, 0x9E, 0x9C, 0x6F }; // AES_128 Key
        protected static readonly int headerSize = 0x200;
        protected static readonly int fileInfoSize = 0x150;
        public MemoryStream FAT = new MemoryStream();

        public AAPak _owner;
        public int Size = headerSize;
        // public long fatHeaderOffset = 0 ;
        public long FirstFileInfoOffset = 0;
        public long AddFileOffset = 0;
        public byte[] rawData = new byte[headerSize]; // unencrypted header
        public byte[] data = new byte[headerSize]; // decrypted header data
        public bool isValid;
        public uint fileCount = 0;
        public uint extraFileCount = 0; // not sure what this is for

        public byte[] nullHash;
        public string nullHashString = "".PadRight(32, '0');

        /// <summary>
        /// Creates a new Header Block for a Pak file
        /// </summary>
        /// <param name="owner">The AAPak that this header belongs to</param>
        public AAPakFileHeader(AAPak owner)
        {
            _owner = owner;
            nullHash = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        ~AAPakFileHeader()
        {
            FAT.Dispose();
        }

        // Source:
        // https://stackoverflow.com/questions/44782910/aes128-decryption-in-c-sharp

        public static byte[] EncryptAES(byte[] message, byte[] key, bool doEncryption)
        {
            try
            {
                Aes aes = new AesManaged();
                aes.Key = key;
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cipher;

                if (doEncryption == true)
                    cipher = aes.CreateEncryptor();
                else
                    cipher = aes.CreateDecryptor();

                return cipher.TransformFinalBlock(message, 0, message.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool LoadFAT()
        {
            // Read all File Table Data into Memory
            FAT.SetLength(0);

            long TotalFileInfoSize = (fileCount + extraFileCount) * fileInfoSize;
            _owner._gpFileStream.Seek(0, SeekOrigin.End);
            FirstFileInfoOffset = _owner._gpFileStream.Position;

            // Search for the first file location, it needs to be alligned to a 0x200 size block
            FirstFileInfoOffset -= headerSize;
            FirstFileInfoOffset -= TotalFileInfoSize;
            var dif = FirstFileInfoOffset % 0x200;
            // Align to previous block of 512 bytes
            FirstFileInfoOffset -= dif;

            _owner._gpFileStream.Position = FirstFileInfoOffset;

            SubStream _fat = new SubStream(_owner._gpFileStream, FirstFileInfoOffset, _owner._gpFileStream.Length - FirstFileInfoOffset);
            _fat.CopyTo(FAT);

            return true;
        }

        public bool WriteToFAT()
        {
            // Read all File Table Data into Memory
            FAT.SetLength(0);

            int bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));
            MemoryStream ms = new MemoryStream(bufSize); // Could probably do without the intermediate memorystream, but it's easier to process
            BinaryWriter writer = new BinaryWriter(ms);

            // Normal Files
            for (int i = 0; i < _owner.files.Count; i++)
            {
                ms.Position = 0;
                AAPakFileInfo pfi = _owner.files[i];

                // Manually write the string for filename
                for (int c = 0; c < 0x108; c++)
                {
                    byte ch = 0;
                    if (c < pfi.name.Length)
                        ch = (byte)pfi.name[c];
                    writer.Write(ch);
                }
                writer.Write(pfi.offset);
                writer.Write(pfi.size);
                writer.Write(pfi.sizeDuplicate);
                writer.Write(pfi.paddingSize);
                writer.Write(pfi.md5);
                writer.Write(pfi.dummy1);
                writer.Write(pfi.createTime);
                writer.Write(pfi.modifyTime);
                writer.Write(pfi.dummy2);

                byte[] decryptedFileData = new byte[bufSize];
                ms.Position = 0;
                ms.Read(decryptedFileData, 0, bufSize);
                byte[] rawFileData = EncryptAES(decryptedFileData, key, true); // encrypt header data

                FAT.Write(rawFileData, 0, bufSize);
            }

            // "Extra" Files
            for (int i = 0; i < _owner.extraFiles.Count; i++)
            {
                ms.Position = 0;
                AAPakFileInfo pfi = _owner.extraFiles[i];

                // Manually write the string for filename
                for (int c = 0; c < 0x108; c++)
                {
                    byte ch = 0;
                    if (c < pfi.name.Length)
                        ch = (byte)pfi.name[c];
                    writer.Write(ch);
                }
                writer.Write(pfi.offset);
                writer.Write(pfi.size);
                writer.Write(pfi.sizeDuplicate);
                writer.Write(pfi.paddingSize);
                writer.Write(pfi.md5);
                writer.Write(pfi.dummy1);
                writer.Write(pfi.createTime);
                writer.Write(pfi.modifyTime);
                writer.Write(pfi.dummy2);

                byte[] decryptedFileData = new byte[bufSize];
                ms.Position = 0;
                ms.Read(decryptedFileData, 0, bufSize);
                byte[] rawFileData = EncryptAES(decryptedFileData, key, true); // encrypt header data

                FAT.Write(rawFileData, 0, bufSize);
            }
            ms.Dispose();

            var dif = (FAT.Length % 0x200);
            if (dif > 0)
            {
                var pad = (0x200 - dif);
                FAT.SetLength(FAT.Length + pad);
                FAT.Position = FAT.Length;
            }

            fileCount = (uint)_owner.files.Count;
            extraFileCount = (uint)_owner.extraFiles.Count;

            FAT.SetLength(FAT.Length + headerSize);

            EncryptHeaderData();

            FAT.Write(rawData, 0, 0x20);

            return true;
        }


        /// <summary>
        /// Read and decrypt the File Details Table
        /// </summary>
        public void ReadFileTable()
        {
            /*
            math SIZE = FILES
            math SIZE += EXTRA_FILES
            math SIZE *= 0x150
            goto 0 0 SEEK_END
            savepos INFO_OFF
            math INFO_OFF -= 0x200
            for INFO_OFF -= SIZE >= 0
                if INFO_OFF % 0x200
                    math INFO_OFF -= 0x10
                else
                    break
                endif
            next
            print "FileTable offset:   %INFO_OFF|x%"
            */
            /*
            long TotalFileInfoSize = (fileCount + extraFileCount) * fileInfoSize;
            _owner._gpFileStream.Seek(0, SeekOrigin.End);
            FirstFileInfoOffset = _owner._gpFileStream.Position ;

            // Search for the first file location, it needs to be alligned to a 0x200 size block
            FirstFileInfoOffset -= headerSize;
            FirstFileInfoOffset -= TotalFileInfoSize;
            var dif = FirstFileInfoOffset % 0x200;
            FirstFileInfoOffset -= dif;
            */

            //_owner._gpFileStream.Position = FirstFileInfoOffset;
            FAT.Position = 0;

            int bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));
            MemoryStream ms = new MemoryStream(bufSize); // Could probably do without the intermediate memorystream, but it's easier to process
            BinaryReader reader = new BinaryReader(ms);
            // Main Files
            _owner.files.Clear();
            _owner.extraFiles.Clear();
            for (uint i = 0; i < fileCount; i++)
            {
                byte[] rawFileData = new byte[bufSize]; // decrypted header data
                FAT.Read(rawFileData, 0, bufSize);
                byte[] decryptedFileData = EncryptAES(rawFileData, key, false);
                ms.SetLength(0);
                ms.Write(decryptedFileData, 0, bufSize);
                ms.Position = 0;
                //BinaryFormatter formatter = new BinaryFormatter);
                //AAPakFileInfo pfi = (AAPakFileInfo)formatter.Deserialize(_owner._gpFileStream);
                AAPakFileInfo pfi = new AAPakFileInfo();
                // Manually read the string for filename
                pfi.name = "";
                for (int c = 0; c < 0x108;c++)
                {
                    byte ch = reader.ReadByte();
                    if (ch != 0)
                        pfi.name += (char)ch;
                    else
                        break;
                }
                ms.Position = 0x108;
                pfi.offset = reader.ReadInt64();
                pfi.size = reader.ReadInt64();
                pfi.sizeDuplicate = reader.ReadInt64();
                pfi.paddingSize = reader.ReadInt32();
                pfi.md5 = reader.ReadBytes(16);
                pfi.dummy1 = reader.ReadInt32(); // ???
                pfi.createTime = reader.ReadInt64();
                pfi.modifyTime = reader.ReadInt64();
                pfi.dummy2 = reader.ReadInt64(); // ???

                _owner.files.Add(pfi);

                if ((pfi.offset + pfi.size + pfi.paddingSize) > AddFileOffset)
                {
                    AddFileOffset = pfi.offset + pfi.size + pfi.paddingSize;
                }
            }

            // "Extra" Files. It looks like these are old deleted files renamed to "__unused__"
            // There might be more to these, but can't be sure at this moment, looks like they are 512 byte blocks on my paks
            for (uint i = 0; i < extraFileCount; i++)
            {
                byte[] rawFileData = new byte[bufSize]; // decrypted header data
                FAT.Read(rawFileData, 0, bufSize);
                byte[] decryptedFileData = EncryptAES(rawFileData, key, false);
                ms.SetLength(0);
                ms.Write(decryptedFileData, 0, bufSize);
                ms.Position = 0;
                //BinaryFormatter formatter = new BinaryFormatter);
                //AAPakFileInfo pfi = (AAPakFileInfo)formatter.Deserialize(_owner._gpFileStream);
                AAPakFileInfo pfi = new AAPakFileInfo();
                // Manually read the string for filename
                pfi.name = "";
                for (int c = 0; c < 0x108; c++)
                {
                    byte ch = reader.ReadByte();
                    if (ch != 0)
                        pfi.name += (char)ch;
                    else
                        break;
                }
                ms.Position = 0x108;
                pfi.offset = reader.ReadInt64();
                pfi.size = reader.ReadInt64();
                pfi.sizeDuplicate = reader.ReadInt64();
                pfi.paddingSize = reader.ReadInt32();
                pfi.md5 = reader.ReadBytes(16);
                pfi.dummy1 = reader.ReadInt32(); // ???
                pfi.createTime = reader.ReadInt64();
                pfi.modifyTime = reader.ReadInt64();
                pfi.dummy2 = reader.ReadInt64(); // ???

                _owner.extraFiles.Add(pfi);

                if ((pfi.offset + pfi.size + pfi.paddingSize) > AddFileOffset)
                {
                    AddFileOffset = pfi.offset + pfi.size + pfi.paddingSize;
                }
            }
            ms.Dispose();
        }

        /// <summary>
        /// Encrypt and Write the File Details Table
        /// </summary>
        public void WriteFileTable()
        {
            long TotalFileInfoSize = (fileCount + extraFileCount) * fileInfoSize;
            
            // Check if we are alligned to a block, this should normally already be done when adding files, but let's make sure we are correct
            var dif = FirstFileInfoOffset % 0x200;
            if (dif > 0)
            {
                // If not, align to the next block offset
                FirstFileInfoOffset += (0x200 - dif);
            }

            /*
            fatHeaderOffset = FirstFileInfoOffset + TotalFileInfoSize ;
            dif = (fatHeaderOffset % 0x200);
            if (dif > 0)
            {
                fatHeaderOffset = (0x200 - dif);
            }
            */

            _owner._gpFileStream.Position = FirstFileInfoOffset; // Move stream to location

            int bufSize = 0x150; // Marshal.SizeOf(typeof(AAPakFileInfo));
            MemoryStream ms = new MemoryStream(bufSize); // Could probably do without the intermediate memorystream, but it's easier to process
            BinaryWriter writer = new BinaryWriter(ms);

            // Normal Files
            for (int i = 0; i < _owner.files.Count; i++)
            {
                ms.Position = 0;
                AAPakFileInfo pfi = _owner.files[i];

                // Manually write the string for filename
                for (int c = 0; c < 0x108; c++)
                {
                    byte ch = 0;
                    if (c < pfi.name.Length)
                        ch = (byte)pfi.name[c];
                    writer.Write(ch);
                }
                writer.Write(pfi.offset);
                writer.Write(pfi.size);
                writer.Write(pfi.sizeDuplicate);
                writer.Write(pfi.paddingSize);
                writer.Write(pfi.md5);
                writer.Write(pfi.dummy1);
                writer.Write(pfi.createTime);
                writer.Write(pfi.modifyTime);
                writer.Write(pfi.dummy2);

                byte[] decryptedFileData = new byte[bufSize]; 
                ms.Position = 0;
                ms.Read(decryptedFileData, 0, bufSize);
                byte[] rawFileData = EncryptAES(decryptedFileData, key, true); // encrypt header data

                _owner._gpFileStream.Write(rawFileData, 0, bufSize);
            }

            // "Extra" Files
            for (int i = 0; i < _owner.extraFiles.Count; i++)
            {
                ms.Position = 0;
                AAPakFileInfo pfi = _owner.extraFiles[i];

                // Manually write the string for filename
                for (int c = 0; c < 0x108; c++)
                {
                    byte ch = 0;
                    if (c < pfi.name.Length)
                        ch = (byte)pfi.name[c];
                    writer.Write(ch);
                }
                writer.Write(pfi.offset);
                writer.Write(pfi.size);
                writer.Write(pfi.sizeDuplicate);
                writer.Write(pfi.paddingSize);
                writer.Write(pfi.md5);
                writer.Write(pfi.dummy1);
                writer.Write(pfi.createTime);
                writer.Write(pfi.modifyTime);
                writer.Write(pfi.dummy2);

                byte[] decryptedFileData = new byte[bufSize];
                ms.Position = 0;
                ms.Read(decryptedFileData, 0, bufSize);
                byte[] rawFileData = EncryptAES(decryptedFileData, key, true); // encrypt header data

                _owner._gpFileStream.Write(rawFileData, 0, bufSize);
            }

            ms.Dispose();

            fileCount = (uint)_owner.files.Count;
            extraFileCount = (uint)_owner.extraFiles.Count;

            _owner._gpFileStream.Position = FirstFileInfoOffset;
        }


        /// <summary>
        /// Decrypt the current header data
        /// </summary>
        public void DecryptHeaderData()
        {
            data = EncryptAES(rawData, key, false);
            fileCount = BitConverter.ToUInt32(data, 8);
            extraFileCount = BitConverter.ToUInt32(data, 12);
        }

        /// <summary>
        /// Encrypt the current header data
        /// </summary>
        public void EncryptHeaderData()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, headerSize);
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write((byte)'W');
            writer.Write((byte)'I');
            writer.Write((byte)'B');
            writer.Write((byte)'O');
            writer.Seek(8, SeekOrigin.Begin);
            writer.Write(fileCount);
            writer.Seek(12, SeekOrigin.Begin);
            writer.Write(extraFileCount);

            ms.Position = 0;
            ms.Read(data, 0, headerSize);

            ms.Dispose();

            rawData = EncryptAES(data, key, true);
        }

    }

    /// <summary>
    /// AAPak Class used to handle game_pak from ArcheAge
    /// </summary>
    public class AAPak
    {
        /// <summary>
        /// Virtual data to return as a null value for file details
        /// </summary>
        public AAPakFileInfo nullAAPakFileInfo = new AAPakFileInfo();
        public string _gpFilePath { get; private set; }
        public FileStream _gpFileStream { get; private set; }
        public AAPakFileHeader _header;
        public bool isOpen = false;
        public bool isDirty = false;
        public List<AAPakFileInfo> files = new List<AAPakFileInfo>();
        public List<AAPakFileInfo> extraFiles = new List<AAPakFileInfo>();
        public List<string> folders = new List<string>();
        public bool readOnly { get; private set; }
        /// <summary>
        /// If set to true, adds the freed space from a delete to the previous file's padding. If false (default), it "moves" the file to extrefiles for freeing up space, allowing it to be reused.
        /// </summary>
        public bool paddingDeleteMode = false;

        /// <summary>
        /// Creates and/or opens a game_pak file
        /// </summary>
        /// <param name="filePath">Filename of the pak</param>
        /// <param name="openAsReadOnly">Open pak in readOnly Mode if true. Ignored it createAsNewPak is set</param>
        /// <param name="createAsNewPak">If true, will create a new pak at filePath location. Warning: This will overwrite any existing pak at that location !</param>
        public AAPak(string filePath, bool openAsReadOnly = true, bool createAsNewPak = false)
        {
            _header = new AAPakFileHeader(this);
            if (filePath != "")
            {
                bool isLoaded = false;
                if (createAsNewPak)
                {
                    isLoaded = NewPak(filePath);
                }
                else
                {
                    isLoaded = OpenPak(filePath, openAsReadOnly);
                }
                if (isLoaded)
                {
                    isOpen = ReadHeader();
                }
                else
                {
                    isOpen = false;
                }
            }
            else
            {
                isOpen = false;
            }
        }

        ~AAPak()
        {
            if (isOpen)
                ClosePak();
        }

        public bool OpenPak(string filePath, bool openAsReadOnly)
        {
            // Fail if already open
            if (isOpen)
                return false;

            // Check if it exists
            if (!File.Exists(filePath))
            {
                return false;
            }

            try
            {
                // Open stream
                if (openAsReadOnly)
                {
                    _gpFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                }
                else
                {
                    _gpFileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                }
                _gpFilePath = filePath;
                isDirty = false;
                isOpen = true;
                readOnly = openAsReadOnly;
                return ReadHeader();
            }
            catch
            {
                _gpFilePath = null ;
                isOpen = false;
                readOnly = true;
                return false;
            }
        }

        public bool NewPak(string filePath)
        {
            // Fail if already open
            if (isOpen)
                return false;

            try
            {
                // Create new file stream
                _gpFileStream = new FileStream(filePath, FileMode.Create,FileAccess.ReadWrite);
                _gpFilePath = filePath;
                readOnly = false;
                isOpen = true;
                isDirty = true;
                SaveHeader(); // Save blank data
                return ReadHeader(); // read blank data to confirm
            }
            catch
            {
                _gpFilePath = null;
                isOpen = false;
                readOnly = true;
                return false;
            }
        }

        public void ClosePak()
        {
            if (!isOpen)
                return;
            if ((isDirty) && (readOnly == false))
                SaveHeader();
            _gpFileStream.Close();
            _gpFileStream = null;
            _gpFilePath = null;
            isOpen = false;
        }

        /// <summary>
        /// Encrypts and saves Header and File Information Table back to the pak. 
        /// This is also automatically called one ClosePak() if changes where made.
        /// Warning: Failing to save might corrupt your pak if files were added or deleted !
        /// </summary>
        public void SaveHeader()
        {
            _header.WriteToFAT();
            _gpFileStream.Position = _header.FirstFileInfoOffset;
            _header.FAT.Position = 0;
            _header.FAT.CopyTo(_gpFileStream);
            _gpFileStream.SetLength(_gpFileStream.Position);

            isDirty = false;
        }

        /// <summary>
        /// Read Pak Header
        /// </summary>
        /// <returns></returns>
        protected bool ReadHeader()
        {
            files.Clear();
            extraFiles.Clear();
            folders.Clear();

            // Read the last 512 bytes as raw header data
            _gpFileStream.Seek(-_header.Size, SeekOrigin.End);
            
            // Mark correct location as header offset location
            _gpFileStream.Read(_header.rawData, 0, 0x20); // We don't need to read the entire thing, just the first 32 bytes contain data
            // _gpFileStream.Read(_header.rawData, 0, _header.Size);

            _header.DecryptHeaderData();

            // A valid header/footer starts with "WIBO" after decryption
            _header.isValid = (_header.data[0] == 'W') && (_header.data[1] == 'I') && (_header.data[2] == 'B') && (_header.data[3] == 'O');
            if (_header.isValid)
            {
                _header.LoadFAT();
                _header.ReadFileTable();
            }
            else
            {
                _header.FAT.SetLength(0);
            }

            return _header.isValid ;
        }

        /// <summary>
        /// Populate the folders string list with virual folder names derived from the files found inside the pak
        /// </summary>
        public void GenerateFolderList()
        {
            // There is no actual directory info stored in the pak file, so we just generate it based on filenames
            folders.Clear();
            if (!isOpen || !_header.isValid) return;
            foreach(AAPakFileInfo pfi in files)
            {
                // Horror function, I know :p
                string n = Path.GetDirectoryName(pfi.name.ToLower().Replace('/', Path.DirectorySeparatorChar)).Replace(Path.DirectorySeparatorChar, '/');
                var pos = folders.IndexOf(n);
                if (pos >= 0) continue;
                folders.Add(n);
            }
            folders.Sort();
        }

        public List<AAPakFileInfo> GetFilesInDirectory(string dirname)
        {
            var res = new List<AAPakFileInfo>();
            dirname = dirname.ToLower();
            foreach (AAPakFileInfo pfi in files)
            {
                // extract dir name
                string n = Path.GetDirectoryName(pfi.name.ToLower().Replace('/', Path.DirectorySeparatorChar)).Replace(Path.DirectorySeparatorChar, '/');
                if (n == dirname)
                    res.Add(pfi);
            }
            return res;
        }

        /// <summary>
        /// Find a file information inside the pak by it's filename
        /// </summary>
        /// <param name="filename">filename inside the pak of the requested file</param>
        /// <param name="fileInfo">Returns the AAPakFile info of the requested file or nullAAPakFileInfo if it does not exist</param>
        /// <returns>Returns true if the file was found</returns>
        public bool GetFileByName(string filename, ref AAPakFileInfo fileInfo)
        {
            foreach (AAPakFileInfo pfi in files)
            {
                if (pfi.name == filename)
                {
                    fileInfo = pfi;
                    return true;
                }
            }
            fileInfo = nullAAPakFileInfo; // return null file if it fails
            return false;
        }

        /// <summary>
        /// Exports a given file as stream
        /// </summary>
        /// <param name="file">AAPakFileInfo of the file to be exported</param>
        /// <returns>Returns a SubStream of file within the pak</returns>
        public Stream ExportFileAsStream(AAPakFileInfo file)
        {
            return new SubStream(_gpFileStream, file.offset, file.size);
        }

        /// <summary>
        /// Exports a given file as stream
        /// </summary>
        /// <param name="fileName">filename inside the pak of the file to be exported</param>
        /// <returns>Returns a SubStream of file within the pak</returns>
        public Stream ExportFileAsStream(string fileName)
        {
            AAPakFileInfo file = nullAAPakFileInfo ;
            if (GetFileByName(fileName, ref file) == true)
            {
                return new SubStream(_gpFileStream, file.offset, file.size);
            }
            else
            {
                return new MemoryStream();
            }
        }

        /// <summary>
        /// Calculates and set the MD5 Hash of a given file
        /// </summary>
        /// <param name="file">AAPakFileInfo of the file to be updated</param>
        /// <returns>Returns the new hash as a string (with removed dashes)</returns>
        public string UpdateMD5(AAPakFileInfo file)
        {
            MD5 hash = MD5.Create();
            var fs = ExportFileAsStream(file);
            var newHash = hash.ComputeHash(fs);
            hash.Dispose();
            if (!file.md5.SequenceEqual(newHash))
            {
                // Only update if different
                newHash.CopyTo(file.md5,0);
                isDirty = true;
            }
            return BitConverter.ToString(file.md5).Replace("-", ""); // Return the (updated) md5 as a string
        }

        public bool FindFileByOffset(long offset, ref AAPakFileInfo fileInfo)
        {
            foreach(AAPakFileInfo pfi in files)
            {
                if ((offset >= pfi.offset) && (offset <= (pfi.offset + pfi.size + pfi.paddingSize)))
                {
                    fileInfo = pfi;
                    return true;
                }
            }
            fileInfo = nullAAPakFileInfo;
            return false;
        }

        public bool ReplaceFile(ref AAPakFileInfo pfi, Stream sourceStream, DateTime ModifyTime)
        {
            // Overwrite a existing file in the pak

            if (readOnly)
                return false;

            // Fail if the new file is too big
            if (sourceStream.Length > (pfi.size + pfi.paddingSize))
                return false;

            // Save endpos for easy calculation later
            long endPos = pfi.offset + pfi.size + pfi.paddingSize;

            try
            {
                // Copy new data over the old data
                _gpFileStream.Position = pfi.offset;
                sourceStream.Position = 0;
                sourceStream.CopyTo(_gpFileStream);
            }
            catch
            {
                return false;
            }

            // Update File Size in File Table
            pfi.size = sourceStream.Length;
            pfi.sizeDuplicate = pfi.size ;
            // Calculate new Padding size
            pfi.paddingSize = (int)(endPos - pfi.size - pfi.offset);
            // Recalculate the MD5 hash
            UpdateMD5(pfi); // TODO: optimize this to calculate WHILE we are copying the stream

            // Mark File Table as dirty
            isDirty = true;

            return true;
        }

        /// <summary>
        /// Delete a file from pak. Behaves differenly depending on the paddingDeleteMode setting
        /// </summary>
        /// <param name="pfi">AAPakFileInfo of the file that is to be deleted</param>
        /// <returns></returns>
        public bool DeleteFile(AAPakFileInfo pfi)
        {
            // When we detele a file from the pak, we remove the entry from the FileTable and expand the previous file's padding to take up the space
            if (readOnly)
                return false;

            if (paddingDeleteMode)
            {
                AAPakFileInfo prevPfi = nullAAPakFileInfo;
                if (FindFileByOffset(pfi.offset - 1, ref prevPfi))
                {
                    // If we have a previous file, expand it's padding are with the free space from this file
                    prevPfi.paddingSize += (int)pfi.size + pfi.paddingSize;
                }
                files.Remove(pfi);
            }
            else
            {
                // "move" offset and size data to extrafiles
                AAPakFileInfo eFile = new AAPakFileInfo();
                eFile.name = "__unused__";
                eFile.offset = pfi.offset;
                eFile.size = pfi.size + pfi.paddingSize;
                eFile.sizeDuplicate = eFile.size;
                eFile.paddingSize = 0 ;
                eFile.md5 = new byte[16];

                extraFiles.Add(eFile);

                files.Remove(pfi);
            }
            isDirty = true;
            return true;
        }

        public bool AddAsNewFile(string filename, Stream sourceStream, DateTime CreateTime, DateTime ModifyTime, bool autoSpareSpace, out AAPakFileInfo pfi)
        {
            // When we have a new file, or previous space wasn't enough, we will add it where the file table starts, and move the file table
            if (readOnly)
            {
                pfi = nullAAPakFileInfo;
                return false;
            }
            bool addedAtTheEnd = true;

            AAPakFileInfo newFile = new AAPakFileInfo();
            newFile.name = filename;
            newFile.offset = _header.FirstFileInfoOffset;
            newFile.size = sourceStream.Length;
            newFile.sizeDuplicate = newFile.size;
            newFile.createTime = CreateTime.ToFileTime();
            newFile.modifyTime = ModifyTime.ToFileTime();
            newFile.paddingSize = 0;
            newFile.md5 = new byte[16];

            // check if we have "unused" space in extraFiles that we can use
            for(int i = 0; i < extraFiles.Count;i++)
            {
                if (newFile.size <= extraFiles[i].size)
                {
                    // Copy the spare file's properties and remove it from extraFiles
                    newFile.offset = extraFiles[i].offset;
                    newFile.paddingSize = (int)(extraFiles[i].size - newFile.size); // This should already be aligned
                    addedAtTheEnd = false;
                    extraFiles.Remove(extraFiles[i]);
                    break;
                }
            }

            if (addedAtTheEnd)
            {
                // Only need to calculate padding if we are adding at the end
                var dif = (newFile.size % 0x200);
                if (dif > 0)
                {
                    newFile.paddingSize = (int)(0x200 - dif);
                }
                if (autoSpareSpace)
                {
                    // If autSpareSpace is used to add files, we will reserve some extra space as padding
                    // Add 25% by default
                    var spareSpace = (newFile.size / 4);
                    spareSpace -= (spareSpace % 0x200); // Align the spare space
                    newFile.paddingSize += (int)spareSpace;
                }
            }

            // Add to files list
            files.Add(newFile);

            isDirty = true;

            // Add File Data
            _gpFileStream.Position = newFile.offset;
            sourceStream.Position = 0;
            sourceStream.CopyTo(_gpFileStream);

            if (addedAtTheEnd)
            {
                _header.FirstFileInfoOffset = newFile.offset + newFile.size + newFile.paddingSize;
            }

            UpdateMD5(newFile); // TODO: optimize this to calculate WHILE we are copying the stream

            // Set output
            pfi = newFile;
            return true;
        }

        /// <summary>
        /// Adds or replace a given file with name filename with data from sourceStream
        /// </summary>
        /// <param name="filename">The filename used inside the pak</param>
        /// <param name="sourceStream">Source Stream of file to be added</param>
        /// <param name="CreateTime">Time to use as original file creation time</param>
        /// <param name="ModifyTime">Time to use as last modified time</param>
        /// <param name="autoSpareSpace">Enable adding 25% of the sourceStream size as padding when not replacing a file</param>
        /// <param name="pfi">AAPakFileInfo of the newly added or modified file</param>
        /// <returns></returns>
        public bool AddFileFromStream(string filename, Stream sourceStream, DateTime CreateTime, DateTime ModifyTime, bool autoSpareSpace, out AAPakFileInfo pfi)
        {
            pfi = nullAAPakFileInfo;
            if (readOnly)
            {
                return false;
            }

            bool addAsNew = true;
            // Try to find the existing file
            if (GetFileByName(filename, ref pfi))
            {
                var reservedSizeMax = pfi.size + pfi.paddingSize;
                addAsNew = (sourceStream.Length > reservedSizeMax);
            }

            if (addAsNew)
            {
                return AddAsNewFile(filename, sourceStream, CreateTime, ModifyTime, autoSpareSpace, out pfi);
            }
            else
            {
                return ReplaceFile(ref pfi, sourceStream, ModifyTime);
            }
        }

    }
}
