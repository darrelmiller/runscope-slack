using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Runscope.Links;
using Runscope.Messages;
using Tavis;

namespace RunscopeSlackApi
{
    public class ClientState
    {
        private readonly HttpClient _httpClient;

        public JObject BucketList { get; set; }
        public JObject TestsList { get; set; }

        public ClientState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task FollowLinkAsync(BucketsLink bucketsLink)
        {
            var response = await _httpClient.SendAsync(bucketsLink.BuildGetRequest());
            CheckResponse(response);
            BucketList = await bucketsLink.ParseResponse(response);
        }


        private void CheckResponse(HttpResponseMessage response)
        {
            if ((int)response.StatusCode >= 500)
            {
                throw new Exception("Cannot connect to Runscope API");
            }
            if ((int)response.StatusCode >= 400)
            {
                throw new Exception("Runscope API didn't like our request");
            }
        }

        internal TestsLink GetBucketTestsLinkByBucketName(string bucketName)
        {
            var data = BucketList.Property("data").Value as JArray;
            var testsUrl =
                data.Cast<JObject>()
                    .Where(b => ((String) b.Property("name").Value).ToLowerInvariant() == bucketName)
                    .Select(b => (string)b.Property("tests_url").Value).FirstOrDefault();

            if (testsUrl == null) throw new Exception("Cannot find bucket named " + bucketName);
            return new TestsLink() {Target = new Uri(testsUrl)};

        }

        internal async Task FollowLinkAsync(Link link)
        {
            var request = link.CreateRequest();
            var response = await _httpClient.SendAsync(request);
            CheckResponse(response);

            switch (link.Relation)
            {
                case "Tests":
                    var testslink = link as TestsLink;
                    TestsList = await testslink.ParseResponse(response);

                    break;

            }
        }



        internal TriggerLink GetTestTriggerLinkByTestName(string testName)
        {
            var data = TestsList.Property("data").Value as JArray;
            var triggerUrl =
                data.Cast<JObject>()
                    .Where(b => ((String)b.Property("name").Value).ToLowerInvariant() == testName)
                    .Select(b => (string)b.Property("trigger_url").Value).FirstOrDefault();

            if (triggerUrl == null) throw new Exception("Cannot find test named " + testName);

            return new TriggerLink() { Target = new Uri(triggerUrl) }; 
        }
    }
}
