using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RunscopeSlackApi
{
    public static class HttpClientFactory
    {
        public static HttpClient CreateHttpClient(PrivateData privateData)
        {
        
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.runscope.com/")
            };

            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("bearer", privateData.RunscopeApiKey);
            return client;

        }
    }
}
