using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RunscopeSlackApi.Views
{
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