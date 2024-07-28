using BitcoinProcessor.Mining;
using System.Security.Cryptography;

namespace BitcoinProcessor
{
    public class Processor
    {
        public string ProcessHeader(string headerMinusNonce)
        {
            var dummyNonce = "00000000";
            var nonceList = GetViableNonceList(headerMinusNonce + dummyNonce);
            var finalHeader = "";
            Parallel.ForEach(nonceList, (nonce, loopState) =>
            {
                var header = headerMinusNonce + Utility.ReverseEndian(Utility.BytesToHex(BitConverter.GetBytes(nonce)));
                var sha = SHA256.Create();
                var headerBytes = sha.ComputeHash(sha.ComputeHash(Utility.HexToBytes(header)));
                var hash = Utility.ReverseEndian(BitConverter.ToString(headerBytes).Replace("-", "").ToLower());

                if (hash.StartsWith("000000000000000000"))
                {
                    finalHeader = header;
                    loopState.Stop();
                }
            });
            return finalHeader;
        }

        public List<uint> GetViableNonceList(string headerWithDummyN)
        {
            Sha256Factory.NonceList = new List<uint>();
            Sha256Factory.LoopCounter = 1;
            var headerBytes = Sha256Factory.HashFile(Sha256Factory.HashFile(Utility.HexToBytes(headerWithDummyN), headerWithDummyN), headerWithDummyN);
            var list = Sha256Factory.NonceList;
            Sha256Factory.NonceList = null;
            return list;
        }
    }
}
