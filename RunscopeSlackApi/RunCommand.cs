﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Headers.Parser;
using Newtonsoft.Json.Linq;
using Runscope.Links;
using Runscope.Messages;
using Tavis;

namespace RunscopeSlackApi
{
    public class RunCommand
    {
        private readonly ClientState _clientState;
        public string BucketName { get; set; }
        public string TestName { get; set; }

        public RunCommand(ParseNode commandNode, ClientState clientState)
        {
            _clientState = clientState;

            BucketName = commandNode.ChildNode("bucket").Text;
            TestName = commandNode.ChildNode("testphrase").ChildNode("test").Text;
        }

        public async Task Execute()
        {
            await _clientState.FollowLinkAsync(new BucketsLink());  // Retrieve the list of buckets

            var testsLink = _clientState.GetBucketTestsLinkByBucketName(BucketName);
        
            await _clientState.FollowLinkAsync(testsLink);

            var triggerLink = _clientState.GetTestTriggerLinkByTestName(TestName);
            
            await _clientState.FollowLinkAsync(triggerLink);


            Output = "Tests for bucket " + BucketName + " launched successfully";
            
        }


        public string Output { get; set; }
    }

    public class TriggerLink : Link
    {
        public TriggerLink()
        {
            Method = HttpMethod.Post;
        }
    }

    public class BucketLink : Link
    {
        public BucketLink()
        {
            Target = new Uri("/buckets/{bucket_key}", UriKind.Relative);
        }

        public static async Task<JObject> ParseResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return null;
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        public static Uri GetTestTriggerUrl(JObject bucketDetail)
        {
            var data = bucketDetail["data"] as JObject;
            return new Uri(data.Property("tests_url").Value.ToString());
        }
    }


    public class TestsLink : Link
    {
        public TestsLink()
        {
            Relation = "Tests";
            Target = new Uri("/buckets/{bucket_key}/radar", UriKind.Relative);
        }

        public async Task<JObject> ParseResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) return null;
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }
    } 
}