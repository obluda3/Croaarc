using System.IO;
using System;

namespace Croaarc
{
    static class DAT
    {
        public static void Repack(string inputPath, string output) { }
        public static void Extract(string input, string outputDir)
        {
            var file = File.OpenRead(input);

            using(var br = new BinaryReader(file))
            {
                br.BaseStream.Seek(-13, SeekOrigin.End);

                if (br.ReadUInt32() != 0xBABEFACE) // cringe magic number
                {
                    Console.WriteLine($"Invalid file {input}");
                    return;
                }

                var offsetToFileTable = br.ReadInt32();
                var fileCount = br.ReadInt32();

                var initKey = br.ReadByte();
                Console.WriteLine($"File {Path.GetFileName(input)}: {fileCount} files");
                br.BaseStream.Position = offsetToFileTable;
                for(var i = 0; i < fileCount; i++)
                {
                    var fileName1 = br.ReadCStringXored(initKey);
                    var fileName2 = br.ReadCStringXored(initKey);

                    var offset = br.ReadInt32Xored(initKey);
                    var size = br.ReadInt64Xored(initKey);

                    var fileKey = br.ReadByteXored(initKey);
                    var bkPos = br.BaseStream.Position;

                    br.BaseStream.Position = offset;
                    var data = br.ReadBytesXored((int)size, fileKey);

                    var path = outputDir + Path.DirectorySeparatorChar + fileName2;
                    var directoryName = Path.GetDirectoryName(path);

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    using (var output = File.Open(path, FileMode.Create))
                    {
                        output.Write(data);
                    }

                    br.BaseStream.Position = bkPos;
                }

            }

            Console.WriteLine("Extacted file successfully!");
        }

    }
}
