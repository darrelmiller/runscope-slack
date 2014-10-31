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

            // Runscope: Run HttpCheck with x=y
            // Split off Runscope,
            //var commandText = text.ToLowerInvariant().Tr.Remove("")
            // get Run verb
            

            // Convert bucket name into bucket key
            // Convert get bucket radar info from API to get trigger URL
            // Add parameters to the end of the trigger URL

            // Make the request

            return new ResponseMessageResult(new HttpResponseMessage()
            {
                Content = new StringContent("{ \"text\" : \"action completed : " + text + " \" }", Encoding.UTF8,"application/json")
            });
        }
    }
}
