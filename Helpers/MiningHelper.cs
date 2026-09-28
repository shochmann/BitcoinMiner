using BitCoin.Mining;
using BitCoin.Models;
using System;
using System.Linq;
using System.Text;

namespace BitCoin.Helpers
{
    public static class MiningHelper
    {
        public static CoinbaseModel MakeCoinbase(int height)
        {
            var minedBy = "";
            var address = "";
            var coinbaseScript = "03" + TxEncodeCoinbaseHeight(height) + AsciiToHex(minedBy);
            string pubkeyScript = "76" + "a9" + "14" + BitcoinAddressToHash160(address) + "88" + "ac";

            string tx = "";
            tx += "01000000";
            tx += "01";
            tx += new string('0', 64);
            tx += "ffffffff";
            tx += IntToVarIntHex(coinbaseScript.Length / 2);
            tx += coinbaseScript;
            tx += "00000000";
            tx += "01";
            tx += IntToLittleEndianHex(312500000, 8);
            tx += IntToVarIntHex(pubkeyScript.Length / 2);
            tx += pubkeyScript;
            tx += "00000000";

            var model = new CoinbaseModel();
            model.NewRawTransaction = tx;
            var bytes = Sha256Factory.HashFile(Sha256Factory.HashFile(HeaderFactory.HexToBytes(tx)));
            model.NewTxId = HeaderFactory.ReverseEndian(BitConverter.ToString(bytes).Replace("-", "").ToLower());
            return model;
        }

        private static string TxEncodeCoinbaseHeight(int height)
        {
            var output = HeaderFactory.ReverseEndian(height.ToString("X8"));
            if (output.EndsWith("00"))
            {
                output = output.Substring(0, output.Length - 2);
            }
            return output;
        }

        private static string BitcoinAddressToHash160(string address)
        {
            string table = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            long hash160 = 0;
            char[] addrChars = address.ToCharArray();
            Array.Reverse(addrChars);
            address = new string(addrChars);

            for (int i = 0; i < address.Length; i++)
            {
                hash160 += (long)Math.Pow(58, i) * table.IndexOf(address[i]);
            }

            StringBuilder hash160Hex = new StringBuilder(hash160.ToString("X50"));
            return hash160Hex.ToString().Substring(2, 40);
        }

        private static string IntToVarIntHex(int value)
        {
            return value.ToString("X");
        }

        private static string IntToLittleEndianHex(long value, int byteLength)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Resize(ref bytes, byteLength);
            Array.Reverse(bytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static string AsciiToHex(string input)
        {
            var output = "";
            foreach (char ch in input)
            {
                int value = Convert.ToInt32(ch);
                output += String.Format("{0:X}", value);
            }
            return output;
        }

        public static string MakeSerializedBlock(string header, string[] transactions)
        {
            var block = header;
            var transCount = transactions.Count();
            block += transCount.ToString();

            foreach(var tran in transactions)
            {
                block += tran;
            }
            return block;
        }
    }
}
