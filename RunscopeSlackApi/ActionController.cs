using System;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;


namespace RunscopeSlackApi
{
    public class ActionController : ApiController
    {

        public async Task<IHttpActionResult> Post(FormDataCollection formData)
        {
            
            var text = formData["text"];

            var cmd = new CommandFactory().CreateCommand(text);

            if (cmd == null)
            {
                return new SlackResult("Command not recognized");
            }

            var clientState = new ClientState(HttpClientFactory.CreateHttpClient(Request.GetPrivateData()));
            try
            {
                await cmd.Execute(clientState);
                return new SlackResult(cmd.Output);
            }
            catch (Exception ex)
            {
                return new SlackResult("Command failed - " + (!String.IsNullOrEmpty(cmd.Output) ? cmd.Output : ex.Message));

            }
        }
    }
}
