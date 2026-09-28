using System.Net.Http;

namespace BitCoin.Helpers
{
    public static class RpcApiHelper
    {
        public static string GetBlockTemplate()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8332/");
            request.Content = new StringContent("{\"method\": \"getblocktemplate\", \"params\": [{\"rules\": [\"segwit\"]}]}", null, "text/plain");
            request.Headers.Add("Authorization", "Basic ");
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        public static string GetBlock(string hash)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8332/");
            request.Content = new StringContent("{\"method\": \"getblock\", \"params\": [\"" + hash + "\"]}", null, "text/plain");
            request.Headers.Add("Authorization", "Basic ");
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        public static string SubmitBlock(string serializedBlock)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8332/");
            request.Content = new StringContent("{\"method\": \"submitblock\", \"params\": [\"" + serializedBlock + "\"]}", null, "text/plain");
            request.Headers.Add("Authorization", "Basic ");
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }
    }
}
