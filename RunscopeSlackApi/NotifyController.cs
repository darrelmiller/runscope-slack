using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Runscope;

namespace RunscopeSlackApi
{
    public class NotifyController : ApiController
    {
        public async Task<IHttpActionResult> Post(JObject notification)
        {
            
            var httpClient = new HttpClient();

            var message = "Test : " + (string) notification.Property("test_name").Value + " Result : " +
                          (string) notification.Property("result").Value;

            var jText = new JObject(new JProperty("text",message),
                new JProperty("username","Runscope" ),
                new JProperty("icon_url", "https://www.runscope.com/static/img/favicon.png"));
            
            var data = new[] { new KeyValuePair<string, string>("payload", jText.ToString()) };

            var privateData = Request.GetPrivateData();


            await httpClient.PostAsync(new Uri(privateData.SlackNotifyUrl).ToRunscopeUrl("t6so3gtoys0d"), new FormUrlEncodedContent(data));

            return Ok();
        }
    }
}
