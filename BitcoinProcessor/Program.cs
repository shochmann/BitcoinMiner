using BitcoinProcessor;
using System.Reflection.PortableExecutable;
using System.Text;

var apiBase = "https://8nfjxs0s-7181.use.devtunnels.ms/bitcoin/";

try
{
    var client = new HttpClient();
    var processor = new Processor();
    var success = false;

    while(success == false)
    {
        for (int i = 0; i < 15; i++)
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
                success = true;
                var responseString = postResponse.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
catch(Exception ex)
{
    using (StreamWriter writer = new StreamWriter("C:\\Testing\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_Error.txt"))
    {
        writer.WriteLine(ex.Message + ex.StackTrace + ex.InnerException);
    }

    var client = new HttpClient();
    var error = ex.Message + ex.StackTrace + ex.InnerException;

    if (error == null) { error = "null error"; }

    var data = new StringContent("{ \"HeaderString\": \"" + error.Replace("\\","-") + "\" }", Encoding.UTF8, "application/json");
    var postResponse = client.PostAsync(apiBase + "postError", data).Result;
    postResponse.EnsureSuccessStatusCode();
    var responseString = postResponse.Content.ReadAsStringAsync().Result;
}