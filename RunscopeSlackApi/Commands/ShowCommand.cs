using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Runscope.Links;

namespace RunscopeSlackApi.Commands
{
    public class ShowCommand : IRunscopeCommand
    {
        public string Name { get { return "show"; } }
       
        public static string Description { get { return "Show information about Runscope resources"; } }
        public static string Syntax { get { return @"show ( buckets | tests in <BucketName> | teams ) "; } }

        private static Regex _ParamRegex = new Regex(@"(?<what>buckets|tests|teams)(?:\s+in\s+(?<bucket>.+))?");
        public string What { get; set; }
        public string BucketName { get; set; }

        public ShowCommand(string parameters)
        {
            var match = _ParamRegex.Match(parameters);
            What = match.Groups["what"].Value;
            var bucketMatch = (match.Groups["bucket"] as Group);
            if (bucketMatch != null)
            {
                BucketName = bucketMatch.Value;
            }

        }
        public async Task Execute(ClientState clientState)
        {
                switch (What)
                {
                    case "buckets":
                        await clientState.FollowLinkAsync(new BucketsLink());
                        // Render BucketList to Output
                        Output = RenderBucketList(clientState);
                        break;
                    case "tests":
                        await clientState.FollowLinkAsync(new BucketsLink());  // Retrieve the list of buckets
                        var testsLink = clientState.GetBucketTestsLinkByBucketName(BucketName);

                        await clientState.FollowLinkAsync(testsLink);

                        Output = RenderTestList(clientState);

                        break;

                    case "teams":
                        Output = "Not implemented yet!";
                        break;
                    default:
                        Output = "Sorry, I'm not sure how to show that";
                        break;
                }
            
        }

        private string RenderTestList(ClientState clientState)
        {
            var sbt = new StringBuilder();
            foreach (var test in clientState.TestsList)
            {
                sbt.AppendLine(test.Name);
            }
            return sbt.ToString();
        }

        private string RenderBucketList(ClientState clientState)
        {
            var sb = new StringBuilder();
            foreach (var bucket in clientState.BucketList)
            {
                sb.AppendLine(bucket.Name);
            }
            return sb.ToString();
        }

        public string Output { get; private set; }
    }
}
