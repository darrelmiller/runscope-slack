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
    public class ClientState : IResponseHandler
    {
        private readonly HttpClient _httpClient;

        public List<Bucket> BucketList { get; set; }
        public List<Test> TestsList { get; set; }

        public ClientState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        internal TestsLink GetBucketTestsLinkByBucketName(string bucketName)
        {
            var bucket = BucketList.First(b => string.Compare(b.Name,bucketName,StringComparison.CurrentCultureIgnoreCase) == 0);
            if (bucket == null) throw new Exception("Cannot find bucket named " + bucketName);
            return bucket.Tests;

        }

        internal TestTriggerLink GetTestTriggerLinkByTestName(string testName)
        {
            var data = TestsList.FirstOrDefault(t => String.Compare(t.Name, testName, StringComparison.CurrentCultureIgnoreCase) == 0);
            if (data == null) throw new Exception("Cannot find test named " + testName);
            return data.TestTrigger;
        }
    

        public async Task<HttpResponseMessage> HandleResponseAsync(string linkRelation, HttpResponseMessage responseMessage)
        {
            if ((int)responseMessage.StatusCode >= 500)
            {
                throw new Exception("Cannot connect to Runscope API");
            }
            if ((int)responseMessage.StatusCode >= 400)
            {
                throw new Exception("Runscope API didn't like our request");
            }

            switch (linkRelation)
            {
                case "https://runscope.com/rels/tests":
                    var doc = await responseMessage.Content.ReadAsRunscopeApiDocumentAsync<Test>(Test.Parse);
                    TestsList = doc.DataList;
                    break;

                case "https://runscope.com/rels/buckets":
                case "urn:runscope:buckets":
                    var bucketsDoc = await responseMessage.Content.ReadAsRunscopeApiDocumentAsync<Bucket>(Bucket.Parse);
                    BucketList = bucketsDoc.DataList;
                    break;
            }
            return responseMessage;
        }

        internal Task FollowLinkAsync(IRequestFactory link)
        {
            return _httpClient.FollowLinkAsync(link, this);
        }
    }
}