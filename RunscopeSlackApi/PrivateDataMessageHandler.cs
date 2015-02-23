using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace RunscopeSlackApi
{

    public class PrivateDataMessageHandler : DelegatingHandler
    {
        public const string PrivateDataFileKey = "privatedata";

        private readonly string _configFilePath;
        private readonly string _magicPath = "/privatedataform";  // This can be anything but must match HTML form
        private bool _enabled = false;
        private string _privateDataForm = "PrivateDataForm.html";

        public PrivateDataMessageHandler(string configFilePath)
        {
            _configFilePath = configFilePath;  // Store path to config file provided by host
            _enabled = !File.Exists(_configFilePath);
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Properties[PrivateDataFileKey] = _configFilePath;  // Make available to rest of app

            if (!_enabled) return await base.SendAsync(request,cancellationToken);
            
            // Match any inbound request to display HTML form... Maybe check Accept header for text/html
            if (request.RequestUri.AbsolutePath != _magicPath ) {
                return CreatePrivateDataForm();
            }
            // This must be the POST back from the HTML form
            else if (request.RequestUri.AbsolutePath == _magicPath)
            {
                var response = await ProcessPrivateDataForm(request);
                _enabled = false;
                return response;
            }
            return await base.SendAsync(request, cancellationToken);  // Favicon!
            
        }

        private HttpResponseMessage CreatePrivateDataForm()
        {

            var stream = this.GetType().Assembly.GetManifestResourceStream(this.GetType(), _privateDataForm);
            var httpResponseMessage = new HttpResponseMessage()
            {
                Content = new StreamContent(stream)
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return httpResponseMessage;
        }

        private async Task<HttpResponseMessage> ProcessPrivateDataForm(HttpRequestMessage request)
        {
            var form = await request.Content.ReadAsFormDataAsync();
            var jObject = new JObject();
            foreach (var key in form.AllKeys)
            {
                jObject.Add(key, form[key]);
            }
            File.WriteAllText(_configFilePath, jObject.ToString());

            var response = new HttpResponseMessage
            {
                Content = new StringContent("Private data file initialized.  You can now use the API.")
            };
            return response;
        }

      

    }

   

   
}
