using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RunscopeSlackApi;
using RunscopeSlackApi.Commands;
using Xunit;

namespace RunscopeSlackApiTests
{
    public class ShowCommandTests
    {
        private string testRunscopeApiKey = Environment.GetEnvironmentVariable("RunscopeApiKey");

        [Fact]
        public async Task ShowBucketsCommand()
        {

            var state = new ClientState(HttpClientFactory.CreateRunscopeHttpClient(new PrivateData() { RunscopeApiKey = testRunscopeApiKey }));
            var cmd = new ShowCommand("buckets");
            await cmd.Execute(state);
            Debug.WriteLine(cmd.Output);
            Assert.NotNull(cmd.Output);
        }

        [Fact]
        public async Task ShowTestsCommand()
        {

            var state = new ClientState(HttpClientFactory.CreateRunscopeHttpClient(new PrivateData() { RunscopeApiKey = testRunscopeApiKey }));
            var cmd = new ShowCommand("tests in Design UI Testing");
            await cmd.Execute(state);
            Debug.WriteLine(cmd.Output);
            Assert.NotNull(cmd.Output);
        }


        [Fact]
        public async Task RunTestCommand()
        {

            var state = new ClientState(HttpClientFactory.CreateRunscopeHttpClient(new PrivateData() { RunscopeApiKey = testRunscopeApiKey }));
            var cmd = new RunCommand("APIExamples/Bing Api");
            await cmd.Execute(state);
            Debug.WriteLine(cmd.Output);
            Assert.NotNull(cmd.Output);
        }
    }
}
