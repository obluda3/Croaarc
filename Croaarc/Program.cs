using System;
using System.IO;

namespace Croaarc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("CROAARC - 2021");
            Console.WriteLine("Archive extractor/packer for the resource.dat found in Webfoot games\r\n");

            if(args.Length < 2)
            {
                PrintUsage();
            }
            else if(args[0] != "-r" && args[0] != "-e")
            {
                PrintUsage();
            }
            else if(args[0] == "-e")
            {
                if (!File.Exists(args[1]))
                {
                    Console.WriteLine($"File {args[1]} can't be found");
                }
                else
                {
                    var fullPath = Path.GetFullPath(args[1]);
                    var name = Path.GetFileNameWithoutExtension(fullPath);
                    var outputPath = Path.GetDirectoryName(fullPath) + Path.DirectorySeparatorChar + name;
                    Directory.CreateDirectory(outputPath);
                    DAT.Extract(fullPath, outputPath);
                }
            }
            else
            {
                var fullPath = Path.GetFullPath(args[1]);
                if (!Directory.Exists(fullPath))
                {
                    Console.WriteLine($"Folder {args[1]} can't be found");
                }
                else
                {
                    var parentFolder = Directory.GetParent(fullPath).FullName;
                    var output = parentFolder + Path.DirectorySeparatorChar + Path.GetDirectoryName(fullPath) + ".dat"; 
                    
                    DAT.Repack(fullPath, output);
                }
            }
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("Repack from folder:     Croaarc -r <folder>");
            Console.WriteLine("Extract file:           Croaarc -e <file>");
        }
    }
}
