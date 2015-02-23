using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Tavis.Parser;

namespace RunscopeSlackApi
{
    public class ActionController : ApiController
    {
       // test update

        public async Task<IHttpActionResult> Post(FormDataCollection formData)
        {
            
            var text = formData["text"];

            // Runscope: Run HttpCheck with x=y

            var parseNodes = RunscopeCommand.Syntax.Consume(new Inputdata(text.ToLowerInvariant()));

            var verb = parseNodes.ChildNode("verb");
            switch (verb.Text)
            {
                case "run":
                    var clientState = new ClientState(HttpClientFactory.CreateHttpClient(Request.GetPrivateData()));        
                    var runCommand = new RunCommand(parseNodes);
                    try
                    {
                        await runCommand.Execute(clientState);
                        return new SlackResult(runCommand.Output);
                    }
                    catch (Exception ex)
                    {
                        return new SlackResult("Run command failed - " + (!String.IsNullOrEmpty(runCommand.Output)  ? runCommand.Output : ex.Message ));
                        
                    }
                    break;
                            
                default:
                    return new SlackResult("Command " + verb.Text + " not recognized");
            }
            

           
        }

        
    }
}
