using System.Collections.Generic;

namespace BitCoin.Models
{
    public class GetBlockTemplateModel
    {
        public Result result { get; set; }
        public string error { get; set; }
        public int? id { get; set; }
    }

    public class Result
    {
        public List<string> capabilities { get; set; }
        public int version { get; set; }
        public List<string> rules { get; set; }
        public Vbavailable vbavailable { get; set; }
        public int vbrequired { get; set; }
        public string previousblockhash { get; set; }
        public List<Transaction> transactions { get; set; }
        public Coinbaseaux coinbaseaux { get; set; }
        public int coinbasevalue { get; set; }
        public string longpollid { get; set; }
        public string target { get; set; }
        public int mintime { get; set; }
        public List<string> mutable { get; set; }
        public string noncerange { get; set; }
        public int sigoplimit { get; set; }
        public int sizelimit { get; set; }
        public int weightlimit { get; set; }
        public int curtime { get; set; }
        public string bits { get; set; }
        public int height { get; set; }
        public string default_witness_commitment { get; set; }
    }

    public class Vbavailable
    {
    }

    public class Coinbaseaux
    {
    }

    public class Transaction
    {
        public string data { get; set; }
        public string txid { get; set; }
        public string hash { get; set; }
        public List<int?> depends { get; set; }
        public int fee { get; set; }
        public int sigops { get; set; }
        public int weight { get; set; }
    }
}
