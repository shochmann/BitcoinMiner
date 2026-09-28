using System;
using System.Linq;
using System.Text;

namespace BitCoin.Mining
{
    public static class HeaderFactory
    {
        public static string CreateBlockHashReturnHeader(int version, string previousHash, string merkleRoot,
            int time, string bits)
        {
            var versionFix = BytesToHex(BitConverter.GetBytes(version));
            var previousHashFixed = ReverseEndian(previousHash);
            var merkleRootFixed = ReverseEndian(merkleRoot);
            var timeFixed = BytesToHex(BitConverter.GetBytes(time));
            var bitsFixed = ReverseEndian(bits);
            var nonce = "00000000";
            var header = string.Concat(versionFix, previousHashFixed, merkleRootFixed, timeFixed, bitsFixed, nonce);
            var headerBytes = Sha256Factory.HashFile(Sha256Factory.HashFile(HexToBytes(header)));
            var hash = ReverseEndian(BitConverter.ToString(headerBytes).Replace("-", "").ToLower());
            return header;
        }

        public static string BytesToHex(byte[] myByteArray)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in myByteArray)
                sb.Append(b.ToString("X2"));

            return sb.ToString().ToLower();
        }

        public static byte[] HexToBytes(string a)
        {
            byte[] a1 = Enumerable.Range(0, a.Length / 2).Select(x => Convert.ToByte(a.Substring(x * 2, 2), 16)).ToArray();
            return a1;
        }

        public static string ReverseEndian(string a)
        {
            byte[] a1 = Enumerable.Range(0, a.Length / 2).Select(x => Convert.ToByte(a.Substring(x * 2, 2), 16)).ToArray();
            Array.Reverse(a1);
            return BitConverter.ToString(a1).Replace("-", "").ToLower();
        }
    }
}
