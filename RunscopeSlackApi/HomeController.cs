using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;

namespace RunscopeSlackApi
{
    public class HomeController : ApiController
    {
        public IHttpActionResult Get()
        {
            


            var jObject = new JObject(
                new JProperty("resources",new JObject(
                    new JProperty("https://runscope.com/rels/notify",new JObject( new JProperty("href", "/notify"))),
                    new JProperty("https://runscope.com/rels/action",new JObject( new JProperty("href", "/action")))
                    )
                )); 
            return new ResponseMessageResult(new HttpResponseMessage()
            {
                Content = new JsonContent(jObject)
            });
        }
    }


}
