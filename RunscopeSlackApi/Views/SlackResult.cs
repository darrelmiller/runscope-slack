using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using RunscopeSlackApi.Views;

namespace RunscopeSlackApi
{
    public class SlackResult : IHttpActionResult
    {
        private readonly string _text;

        public SlackResult(string text)
        {
            _text = text;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var jObject = new JObject {new JProperty("text", _text)};
            return Task.FromResult(new HttpResponseMessage
            {
                Content = new JsonContent(jObject)
            });
        }
    }
}