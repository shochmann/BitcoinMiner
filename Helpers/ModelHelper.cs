using BitCoin.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace BitCoin.Helpers
{
    public static class ModelHelper
    {
        public static GetBlockTemplateModel ConvertGbtResponse(string gbt)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            GetBlockTemplateModel model = JsonConvert.DeserializeObject<GetBlockTemplateModel>(gbt);
            return model;
        }

        public static GetBlockModel ConvertBlockResponse(string getBlock)
        {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            GetBlockModel model = JsonConvert.DeserializeObject<GetBlockModel>(getBlock);
            return model;
        }
    }
}
