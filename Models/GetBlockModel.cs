using System.Collections.Generic;

namespace BitCoin.Models
{

    public class GetBlockModel
    {
        public result result { get; set; }
        public string error { get; set; }
        public string id { get; set; }
    }

    public class result
    {
        public string hash { get; set; }
        public int confirmations { get; set; }
        public int height { get; set; }
        public int version { get; set; }
        public string versionHex { get; set; }
        public string merkleroot { get; set; }
        public int time { get; set; }
        public int mediantime { get; set; }
        public long nonce { get; set; }
        public string bits { get; set; }
        public float difficulty { get; set; }
        public string chainwork { get; set; }
        public int nTx { get; set; }
        public string previousblockhash { get; set; }
        public string nextblockhash { get; set; }
        public int strippedsize { get; set; }
        public int size { get; set; }
        public int weight { get; set; }
        public List<string> tx { get; set; }
    }
}
