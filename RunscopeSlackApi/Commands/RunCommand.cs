using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinkTests;
using Runscope;
using Runscope.Links;
using Tavis.Headers.Elements;

namespace RunscopeSlackApi.Commands
{
    public class RunCommand : IRunscopeCommand
    {
        private static Regex _BucketTestRegex = new Regex(@"(?<Bucket>[\w\s]+)(?:/(?<Test>.+))?");
        private static Regex _ParametersRegex = new Regex(@"(?:(?<name>[\w\s]+)=(?<value>[^,]+))");

        public string Name { get { return "run"; } }
        public static string Description { get { return "Executes a test"; } }
        public static string Syntax { get { return "run <BucketName>/[<TestName>] [with <param>=<value>[,<param>=<value>]*]"; } }

        public string BucketName { get; set; }
        public string TestName { get; set; }
        public List<Parameter> Parameters { get; set; }
        
        public RunCommand(string parameters) {

            string bucketTest = null;
            var paramstart = parameters.IndexOf(" with "); 
            bucketTest = paramstart > 0 ? parameters.Substring(0, paramstart) : parameters;
            var bucketTestMatch = _BucketTestRegex.Match(bucketTest);

            BucketName = bucketTestMatch.Groups["Bucket"].Value.Trim();
            var testMatch = bucketTestMatch.Groups["Test"];
            if (testMatch != null)
            {
                TestName = testMatch.Value.Trim();
            }

            if (paramstart > 0)
            {

                Parameters = _ParametersRegex.Matches(parameters.Substring(paramstart + 6))
                    .Cast<Match>()
                    .Select(paramPair => new Parameter
                        {
                            Name = paramPair.Groups["name"].Value.Trim(),
                            Value = paramPair.Groups["value"].Value.Trim()
                        }).ToList();
                    
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

            var notifyUrl = new Uri("https://runscope-slack.azurewebsites.net/notify"); // Currently fails if callback url has a dash .ToRunscopeUrl("t6so3gtoys0d");
            
            var triggerparams = new Dictionary<string, string> { { "runscope_notification_url", notifyUrl.OriginalString} };


            if (Parameters != null)
            {
                // Apply parameters to triggerLink
                foreach (var parameter in Parameters)
                {
                    triggerparams.Add(parameter.Name, parameter.Value);
                }
            }

            var triggerUrl = triggerLink.Target.AddToQuery(triggerparams);
            triggerLink.Target = triggerUrl.ToRunscopeUrl("t6so3gtoys0d");
            triggerLink.Method = HttpMethod.Get; // Flip to a GET for the moment as POST isn't notifying at the moment.
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
