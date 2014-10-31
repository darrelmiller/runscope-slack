using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RunscopeSlackApi
{
    public class EchoController : ApiController
    {
        public async Task<HttpResponseMessage> Post()
        {
            await Request.Content.LoadIntoBufferAsync();

            return new HttpResponseMessage() {Content = Request.Content};
        }
    }
}
