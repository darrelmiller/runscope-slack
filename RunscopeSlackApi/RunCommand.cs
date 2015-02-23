using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LinkTests;
using Runscope.Links;
using Tavis.Headers.Elements;
using Tavis.Parser;

namespace RunscopeSlackApi
{
    public class RunCommand
    {
        
        public string BucketName { get; set; }
        public string TestName { get; set; }
        public List<Parameter> Parameters { get; set; }


        public RunCommand(ParseNode commandNode)
        {
            Parameters = new List<Parameter>();
            foreach (var childNode in commandNode.ChildNodes)
            {
                if (childNode.Present)
                {
                    switch (childNode.Expression.Identifier)
                    {
                        case "tbucket":
                        case "qbucket":
                            BucketName = childNode.Text;
                            break;
                        case "testphrase":
                            if (childNode.ChildNode("qtest") != null)
                            {
                                TestName = childNode.ChildNode("qtest").Text;
                            }
                            else
                            {
                                TestName = childNode.ChildNode("ttest").Text;
                            }
                            break;
                        case "paramlist":
                            if (childNode.ChildNodes != null)
                            {
                                Parameters = childNode.ChildNode("parameters").ChildNodes
                                    .Select(Parameter.Create).ToList();
                            }
                            break;

                    }
                }
            }
            }

        public async Task Execute(ClientState clientState)
        {
            await clientState.FollowLinkAsync(new BucketsLink());  // Retrieve the list of buckets

            var testsLink = clientState.GetBucketTestsLinkByBucketName(BucketName);
        
            await clientState.FollowLinkAsync(testsLink);

            string result = string.Empty;
            if (string.IsNullOrEmpty(TestName))
            {
                foreach (var test in clientState.TestsList)
                {
                    await RunTest(clientState, test.TestTrigger);
                    var singleresult = string.Format("Test {0} in bucket {1}  launched successfully", test.Name, BucketName);
                    result += singleresult + Environment.NewLine;
                }
            } else
            {
                await RunTest(clientState, clientState.GetTestTriggerLinkByTestName(TestName));
                result = string.Format("Test {0} in bucket {1}  launched successfully", TestName, BucketName);
            }
            
            Output = result;
        }

        private async Task RunTest(ClientState clientState, TestTriggerLink triggerLink)
        {
            
            var triggerparams = new Dictionary<string, string>
            {{"runscope_notification_url", "https://runscope-slack.azurewebsites.net/notify"}};

            // Apply parameters to triggerLink
            foreach (var parameter in Parameters)
            {
                triggerparams.Add(parameter.Name, parameter.Value);
            }
            triggerLink.Target = triggerLink.Target.AddToQuery(triggerparams);

            await clientState.FollowLinkAsync(triggerLink);

        }


        public string Output { get; set; }
    }

    //public class TriggerLink : Link
    //{
    //    public TriggerLink()
    //    {
          
    //        //runscope_notification_url
    //        AddNonTemplatedParametersToQueryString = true;
    //        SetParameter("runscope_notification_url", "https://runscope-slack.azurewebsites.net/notify");
    //    }
    //}

    //public class BucketLink : Link
    //{
    //    public BucketLink()
    //    {
    //        Target = new Uri("/buckets/{bucket_key}", UriKind.Relative);
    //    }

    //    public static async Task<JObject> ParseResponse(HttpResponseMessage response)
    //    {
    //        if (!response.IsSuccessStatusCode) return null;
    //        return JObject.Parse(await response.Content.ReadAsStringAsync());
    //    }

    //    public static Uri GetTestTriggerUrl(JObject bucketDetail)
    //    {
    //        var data = bucketDetail["data"] as JObject;
    //        return new Uri(data.Property("tests_url").Value.ToString());
    //    }
    //}


    //public class TestsLink : Link
    //{
    //    public TestsLink()
    //    {
    //        Relation = "Tests";
    //        Target = new Uri("/buckets/{bucket_key}/radar", UriKind.Relative);
            
    //    }

    //    public async Task<JObject> ParseResponse(HttpResponseMessage response)
    //    {
    //        if (!response.IsSuccessStatusCode) return null;
    //        return JObject.Parse(await response.Content.ReadAsStringAsync());
    //    }
    //} 
}
