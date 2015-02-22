using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Runscope;
using Runscope.Links;
using Runscope.Messages;
using Tavis;

namespace RunscopeSlackApi
{
    public class ClientState
    {
        private readonly HttpClient _httpClient;

        public List<Bucket> BucketList { get; set; }
        public List<Test> TestsList { get; set; }

        public ClientState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task FollowLinkAsync(BucketsLink bucketsLink)
        {
            var response = await _httpClient.FollowLinkAsync(bucketsLink);
            CheckResponse(response);
            var doc = await response.Content.ReadAsRunscopeApiDocument<Bucket>(Bucket.Parse);
            BucketList = doc.DataList;
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
            var bucket = BucketList.First(b => b.Name == bucketName);
            if (bucket == null) throw new Exception("Cannot find bucket named " + bucketName);
            return bucket.Tests;

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
                    var doc = await response.Content.ReadAsRunscopeApiDocument<Test>(Test.Parse);
                    TestsList = doc.DataList;

                    break;

            }
        }



        internal TestTriggerLink GetTestTriggerLinkByTestName(string testName)
        {
            var data = TestsList.FirstOrDefault(t => t.Name == "testName");
            if (data == null) throw new Exception("Cannot find test named " + testName);
            return data.TestTrigger;
        }
    }
}
