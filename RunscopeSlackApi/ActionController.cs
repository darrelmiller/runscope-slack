using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Headers;
using Headers.Parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tavis.Headers.Elements;

namespace RunscopeSlackApi
{
    public class ActionController : ApiController
    {
        public static IExpression CommandSyntax = new Expression("cmd")
            {
                new Ows(),
                new Literal("runscope:"),
                new Ows(),
                new Headers.Token("verb"),
                new Rws(),
                new Headers.Token("bucket"),
                new Ows(),
                new OptionalExpression("testphrase")
                {
                    new Literal("/"),
                    new Ows(),
                    new Headers.Token("test")
                },
                new OptionalExpression("paramlist")
                {
                    new Rws(),
                    new Literal("with"),
                    new CommaList("parameters",Parameter.Syntax)    
                }
                
            };

        public async Task<IHttpActionResult> Post(FormDataCollection formData)
        {
            
            var text = formData["text"];

            // Runscope: Run HttpCheck with x=y

            var parseNodes = CommandSyntax.Consume(new Inputdata(text.ToLowerInvariant()));

            var verb = parseNodes.ChildNode("verb");
            switch (verb.Text)
            {
                case "run":
                    var clientState = new ClientState(CreateHttpClient());        
                    var runCommand = new RunCommand(parseNodes,clientState);
                    await runCommand.Execute();
                    return new SlackResult(runCommand.Output);
                    break;
                            
                default:
                    return new SlackResult("Command " + verb.Text + " not recognized");
            }
            

           
        }

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient() {BaseAddress = new Uri("https://api.runscope.com/")};
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "26de4fd5-ee42-4fb7-b996-0c6b959f3905");
            return client;

        }
    }

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

    public class JsonContent : HttpContent
    {
        private readonly JObject _value;

        public JsonContent(JObject value)
        {
            _value = value;
            Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var writer = new JsonTextWriter(new StreamWriter(stream));
            _value.WriteTo(writer);
            writer.Flush();
            
            return Task.FromResult(0);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}
