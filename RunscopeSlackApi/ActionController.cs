using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace RunscopeSlackApi
{
    public class ActionController : ApiController
    {

        public IHttpActionResult Post(FormDataCollection formData)
        {
            var text = formData["text"];

            return new ResponseMessageResult(new HttpResponseMessage()
            {
                Content = new StringContent("{ \"text\" : \"action completed : " + text + " \" }", Encoding.UTF8,"application/json")
            });
        }
    }
}
