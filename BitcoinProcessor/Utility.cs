using System.Text;

namespace BitcoinProcessor
{
    public static class Utility
    {
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
