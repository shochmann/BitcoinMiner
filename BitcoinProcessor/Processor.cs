using BitcoinProcessor.GPU;
using BitcoinProcessor.Mining;

namespace BitcoinProcessor
{
    public class Processor
    {
        public string ProcessHeader(List<string> headersWithoutNonce)
        {
            var dummyNonce = "00000000";
            var headers = new List<string>();

            Parallel.ForEach(headersWithoutNonce, (h, loopState) =>
            {
                var nonceList = GetViableNonceList(h + dummyNonce);
                foreach (var nonce in nonceList)
                {
                    headers.Add(h + Utility.ReverseEndian(Utility.BytesToHex(BitConverter.GetBytes(nonce))));
                }
            });
            
            var finalHeader = GpuProcessor.ProcessHeaderOnGpu(headers);
            return finalHeader;
        }

        public List<uint> GetViableNonceList(string headerWithDummyN)
        {
            var factory = new Sha256Factory();
            factory.NonceList = new List<uint>();
            factory.LoopCounter = 1;
            var headerBytes = factory.HashFile(factory.HashFile(Utility.HexToBytes(headerWithDummyN), headerWithDummyN), headerWithDummyN);
            return factory.NonceList;
        }
    }
}
