using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace Croaarc
{
    static class BinaryExtensions
    {

        public static string ReadCStringXored(this BinaryReader br, byte key)
        {
            var bytes = new List<byte>();
            var data = br.ReadByte();
            byte dataXored = (byte)(data ^ key);
            while(dataXored != 0)
            {
                bytes.Add(dataXored);
                data = br.ReadByte();
                dataXored = (byte)(data ^ key);
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
        public static byte[] ReadBytesXored(this BinaryReader br, int count, byte key)
        {
            var data = br.ReadBytes(count);
            for (var i = 0; i < data.Length; i++) data[i] ^= key;
            return data;
        }

        public static int ReadInt32Xored(this BinaryReader br, byte key)
        {
            var data = br.ReadBytes(4);
            for (var i = 0; i < data.Length; i++) data[i] ^= key;

            return BitConverter.ToInt32(data);
        }

        public static long ReadInt64Xored(this BinaryReader br, byte key)
        {
            var data = br.ReadBytes(8);
            for (var i = 0; i < data.Length; i++) data[i] ^= key;

            return BitConverter.ToInt64(data);
        }

        public static byte ReadByteXored(this BinaryReader br, byte key)
        {
            return (byte)(br.ReadByte() ^ key);
        }

        public static void WriteXored(this BinaryWriter bw, byte data, byte key)
        {
            bw.Write((byte)(data ^ key));
        }

        public static void WriteXored(this BinaryWriter bw, int data, byte key)
        {
            var bytes = BitConverter.GetBytes(data);
            for (var i = 0; i < 4; i++) bytes[i] ^= key;

            bw.Write(bytes);
        }

        public static void WriteXored(this BinaryWriter bw, byte[] data, byte key)
        {
            for (var i = 0; i < data.Length; i++) data[i] ^= key;
            bw.Write(data);
        }

        public static void WriteCStringXored(this BinaryWriter bw, string data, byte key)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            bw.WriteXored(bytes, key);
            bw.Write(key); // 0 ^ key = key
        }
    }
}
