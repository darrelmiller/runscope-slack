using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;


namespace RunscopeSlackApi
{
    public static class HttpClientFactory
    {
        public static HttpClient CreateRunscopeHttpClient(PrivateData privateData)
        {
        
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.runscope.com/")
            };

            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("bearer", privateData.RunscopeApiKey);
            return client;

        }

        public static HttpClient CreateSlackHttpClient()
        {

            var client = new HttpClient()
            {
            };

            //client.DefaultRequestHeaders.Authorization
            //    = new AuthenticationHeaderValue("bearer", privateData.RunscopeApiKey);
            return client;

        }
    }
}
