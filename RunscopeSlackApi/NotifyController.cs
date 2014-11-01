using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace RunscopeSlackApi
{
    public class NotifyController : ApiController
    {
        private Uri SlackNotify = new Uri("https://hooks.slack.com/services/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");

        public async Task<IHttpActionResult> Post(JObject notification)
        {
            var httpClient = new HttpClient();

            var message = "Test : " + (string) notification.Property("test_name").Value + " Result : " +
                          (string) notification.Property("result").Value;

            var jText = new JObject(new JProperty("text",message),
                new JProperty("username","Runscope" ),
                new JProperty("icon_url", "https://www.runscope.com/static/img/favicon.png"));
            
            var data = new[] { new KeyValuePair<string, string>("payload", jText.ToString()) };
            
            await httpClient.PostAsync(SlackNotify,new FormUrlEncodedContent(data));

            return Ok();
        }
    }
}
