using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace RunscopeSlackApi
{
    public class PrivateData
    {
        public string SlackNotifyUrl { get; set; }
        public string RunscopeApiKey { get; set; }
    }

    public static class PrivateDataHelperExtensions
    {
        public static PrivateData GetPrivateData(this HttpRequestMessage request)
        {
            var privateDataPath = request.Properties[PrivateDataMessageHandler.PrivateDataFileKey] as string;
            return JsonConvert.DeserializeObject<PrivateData>(new StreamReader(privateDataPath).ReadToEnd());
        }

    }
}
