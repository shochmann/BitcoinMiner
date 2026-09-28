using BitCoin.Helpers;
using BitCoin.Mining;
using System.Linq;

namespace BitCoin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gbtResult = RpcApiHelper.GetBlockTemplate();
            var gbtModel = ModelHelper.ConvertGbtResponse(gbtResult);
            var coinbaseModel = MiningHelper.MakeCoinbase(gbtModel.result.height);
            var transactions = gbtModel.result.transactions.Select(x => x.txid).ToArray();
            var allTransactions = new[] { coinbaseModel.NewTxId }.Concat(transactions).ToArray();
            var merkleRoot = MerkleFactory.CreateMerkleRoot(allTransactions);
            var header = HeaderFactory.CreateBlockHashReturnHeader(gbtModel.result.version,
                gbtModel.result.previousblockhash, merkleRoot, gbtModel.result.curtime,
                gbtModel.result.bits);
            var serializedBlock = MiningHelper.MakeSerializedBlock(header, allTransactions);
            var submitBlockResponse = RpcApiHelper.SubmitBlock(serializedBlock);
        }
    }
}
