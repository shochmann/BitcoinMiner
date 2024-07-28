using BitcoinProcessor;
using System.Text;

try
{
    var client = new HttpClient();
    var processor = new Processor();
    var apiBase = "https://localhost:7181/bitcoin/";
    string x = null;
    var l = x.Length;

    for (int i = 0; i < 5; i++)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
        apiBase + "getNextHeader/" + i.ToString());
        var response = client.SendAsync(request).Result;
        response.EnsureSuccessStatusCode();
        var headerMinusN = response.Content.ReadAsStringAsync().Result;

        var header = processor.ProcessHeader(headerMinusN);
        if (!string.IsNullOrEmpty(header))
        {
            var data = new StringContent("{ \"HeaderString\": \"" + header + "\" }", Encoding.UTF8, "application/json");
            var postResponse = client.PostAsync(apiBase + "postValidHeader", data).Result;
            postResponse.EnsureSuccessStatusCode();
            var responseString = postResponse.Content.ReadAsStringAsync().Result;
        }
    }
}
catch(Exception ex)
{
    var client = new HttpClient();
    var apiBase = "https://localhost:7181/bitcoin/";
    var error = ex.Message + ex.StackTrace + ex.InnerException;

    var data = new StringContent("{ \"HeaderString\": \"" + error.Replace("\\","-") + "\" }", Encoding.UTF8, "application/json");
    var postResponse = client.PostAsync(apiBase + "postError", data).Result;
    postResponse.EnsureSuccessStatusCode();
    var responseString = postResponse.Content.ReadAsStringAsync().Result;
}