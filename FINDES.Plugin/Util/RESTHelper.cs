using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FINDES.Plugin
{
    public class RESTHelper
    {
        public static string PostRawJson(string url, string json)
        {
            string urlContents = "";
            HttpResponseMessage result = null;

            Task.Run(async () => {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                result = await client.PostAsync(new Uri(url), content);
                urlContents = await result.Content.ReadAsStringAsync();
                client.Dispose();
            }).Wait();

            return urlContents;
        }
    }
}
