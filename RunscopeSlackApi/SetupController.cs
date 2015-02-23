//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Results;
//using Newtonsoft.Json.Linq;

//namespace RunscopeSlackApi
//{
//    public class SetupController : ApiController
//    {

//        public IHttpActionResult Get()
//        {
//            var stream = this.GetType().Assembly.GetManifestResourceStream(this.GetType(), "Setup.html");
//            var httpResponseMessage = new HttpResponseMessage()
//            {
//                Content = new StreamContent(stream)
//            };
//            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            
//            return new ResponseMessageResult(httpResponseMessage);
//        }

//        public IHttpActionResult Post(FormDataCollection form)
//        {
//            var jObject = new JObject();
//            foreach (var pair in form)
//            {
//                jObject.Add(pair.Key, pair.Value);
//            }
//            File.WriteAllText(Request.Properties[SetupMessageHandler.PrivateDataFile] as string ,jObject.ToString());

//            return new RedirectResult(new Uri("/",UriKind.Relative),this);
//        }
//    }
//}
