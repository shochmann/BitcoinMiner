using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace BitCoin.Mining
{
    public static class MerkleFactory
    {
        public static string CreateMerkleRoot(string[] txs)
        {
            while (true)
            {
                if (txs.Length == 1) return txs[0];
                List<string> newHashList = new List<string>();
                int len = (txs.Length % 2 != 0) ? txs.Length - 1 : txs.Length;
                for (int i = 0; i < len; i += 2) newHashList.Add(Hash2(txs[i], txs[i + 1]));
                if (len < txs.Length) newHashList.Add(Hash2(txs[txs.Length - 1], txs[txs.Length - 1]));
                txs = newHashList.ToArray();
            }
        }

        public static string Hash2(string a, string b)
        {
            byte[] a1 = Enumerable.Range(0, a.Length / 2).Select(x => Convert.ToByte(a.Substring(x * 2, 2), 16)).ToArray();
            Array.Reverse(a1);
            byte[] b1 = Enumerable.Range(0, b.Length / 2).Select(x => Convert.ToByte(b.Substring(x * 2, 2), 16)).ToArray();
            Array.Reverse(b1);
            var c = a1.Concat(b1).ToArray();
            SHA256 sha256 = SHA256.Create();
            byte[] firstHash = sha256.ComputeHash(c);
            byte[] hashOfHash = sha256.ComputeHash(firstHash);
            Array.Reverse(hashOfHash);
            return BitConverter.ToString(hashOfHash).Replace("-", "").ToLower();
        }
    }
}
