using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace RunscopeSlackApi
{

    public class SetupMessageHandler : DelegatingHandler
    {
        public const string PrivateDataFile = "apikeys";
        private const string _SetupPath = "/setup";

        private readonly string _configFilePath;

        public SetupMessageHandler(string configFilePath)
        {
            _configFilePath = configFilePath;  // Store path to config file provided by host
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            if (!File.Exists(_configFilePath) && request.RequestUri.AbsolutePath != _SetupPath)
            {
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.SeeOther);
                httpResponseMessage.Headers.Location = new Uri(_SetupPath,UriKind.Relative);
                return Task.FromResult(httpResponseMessage);
            }
            request.Properties[PrivateDataFile] = _configFilePath;  // pass config file to request

            return base.SendAsync(request, cancellationToken);
        }
    }

    public static class SetupHttpRequestMessageExtensions
    {
        public static PrivateData GetPrivateConfig(this HttpRequestMessage request)
        {
            var privateDataPath = request.Properties[SetupMessageHandler.PrivateDataFile] as string;
            var jObject = JObject.Load(new JsonTextReader(new StreamReader(privateDataPath)));
            return new PrivateData()
            {
                RunscopeApiKey = (string)jObject["apikey"],
                SlackNotifyUrl = (string)jObject["slackwebhook"]
            };
        }
    }

    public class PrivateData
    {
        public string SlackNotifyUrl { get; set; }
        public string RunscopeApiKey { get; set; }
    }
}
