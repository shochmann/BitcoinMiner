using BitcoinProcessor.GPU;
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
            finalHeader = GpuProcessor.ProcessHeaderOnGpu(headerMinusNonce, nonceList);
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
