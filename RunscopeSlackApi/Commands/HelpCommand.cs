using System;
using System.Threading.Tasks;

namespace RunscopeSlackApi.Commands
{
    public class HelpCommand : IRunscopeCommand
    {

        public string Name { get { return "help"; } }
        public string Description { get { return "Display help for commands"; } }
        public string Syntax { get { return "help "; } }

        public HelpCommand(string parameters)
        {
        
        }
       

        public async Task Execute(ClientState clientState)
        {
            Output = "help - this command" + Environment.NewLine +
                     RunCommand.Syntax + " - " + RunCommand.Description + Environment.NewLine +
                     ShowCommand.Syntax + " - " + ShowCommand.Description + Environment.NewLine; 
        }


        public string Output { get; set; }
    }
}
