using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Headers;
using Headers.Parser;
using Tavis.Headers.Elements;

namespace RunscopeSlackApi
{
    public class ActionController : ApiController
    {
        public static IExpression CommandSyntax = new Expression("cmd")
            {
                new Ows(),
                new Literal("runscope:"),
                new Ows(),
                new Headers.Token("verb"),
                new Rws(),
                new Headers.QuotedString("bucket"),
                new Ows(),
                new OptionalExpression("testphrase")
                {
                    new Literal("/"),
                    new Ows(),
                    new Headers.QuotedString("test")
                },
                new OptionalExpression("paramlist")
                {
                    new Rws(),
                    new Literal("with"),
                    new CommaList("parameters",Parameter.Syntax)    
                }
                
            };

        public async Task<IHttpActionResult> Post(FormDataCollection formData)
        {
            
            var text = formData["text"];

            // Runscope: Run HttpCheck with x=y

            var parseNodes = CommandSyntax.Consume(new Inputdata(text.ToLowerInvariant()));

            var verb = parseNodes.ChildNode("verb");
            switch (verb.Text)
            {
                case "run":
                    var clientState = new ClientState(HttpClientFactory.CreateHttpClient(Request.GetPrivateConfig()));        
                    var runCommand = new RunCommand(parseNodes,clientState);
                    try
                    {
                        await runCommand.Execute();
                        return new SlackResult(runCommand.Output);
                    }
                    catch (Exception ex)
                    {
                        return new SlackResult("Run command failed - " + runCommand.Output);
                        
                    }
                    break;
                            
                default:
                    return new SlackResult("Command " + verb.Text + " not recognized");
            }
            

           
        }

        
    }
}
