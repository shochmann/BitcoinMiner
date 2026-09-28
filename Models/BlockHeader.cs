namespace BitCoin.Models
{
    public class BlockHeader
    {
        public int version { get; set; }
        public string previousblockhash { get; set; }
        public string merkleroot { get; set; }
        public int time { get; set; }
        public float difficulty { get; set; }
        public long nonce { get; set; }
    }
}
