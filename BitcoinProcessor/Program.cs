using BitcoinProcessor;
using System.Text;

var apiBase = "https://8nfjxs0s-7181.use.devtunnels.ms/bitcoin/";

try
{
    var client = new HttpClient();
    var processor = new Processor();
    var success = false;
    var headersToProcess = 0;
    var docPath = "C:\\ProcessorConfig\\ProcessorConfig.txt";
    var lines = File.ReadAllLines(docPath);
    headersToProcess = int.Parse(lines[0]);

    while(success == false)
    {
        var header = "";
        var headerList = new List<string>();
        for (int i = 0; i < headersToProcess; i++)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            apiBase + "getNextHeader/" + i.ToString());
            var retry = 0;
            while(retry < 3)
            {
                try
                {
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    var headerMinusN = response.Content.ReadAsStringAsync().Result;
                    headerList.Add(headerMinusN);
                    retry = 3;
                }
                catch (Exception e)
                {
                    retry++;
                    Thread.Sleep(20000);
                }
            }
        }

        header = processor.ProcessHeader(headerList);
        if (!string.IsNullOrEmpty(header))
        {
            var retryHeader = 0;
            while (retryHeader < 3)
            {
                try
                {
                    var data = new StringContent("{ \"HeaderString\": \"" + header + "\" }", Encoding.UTF8, "application/json");
                    var postResponse = client.PostAsync(apiBase + "postValidHeader", data).Result;
                    postResponse.EnsureSuccessStatusCode();
                    retryHeader = 3;
                    success = true;
                    var responseString = postResponse.Content.ReadAsStringAsync().Result;
                }
                catch (Exception exception)
                {
                    retryHeader++;
                    Thread.Sleep(20000);
                }
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