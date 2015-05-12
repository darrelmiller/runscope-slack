using System.Threading.Tasks;

namespace RunscopeSlackApi.Commands
{
    public class MakeCommand : IRunscopeCommand
    {
        public string Name { get; private set; }

        public MakeCommand(string parameters)
        {
            Output = "Here you go, ";
            if (parameters.Contains("sandwich"))
            {
                Output += ":sandwich: ";
            }
            if (parameters.Contains("burrito"))
            {
                Output += ":burrito: ";
            }
            if (parameters.Contains("taco"))
            {
                Output += ":taco: ";
            }
            if (parameters.Contains("poutine"))
            {
                Output += ":poutine: ";
            }
            if (parameters.Contains("vegemite"))
            {
                Output += ":vegemite: ";
            }
            if (parameters.Contains("coffee"))
            {
                Output += ":coffee: ";
            }
            if (parameters.Contains("hamburger"))
            {
                Output += ":pizza: ";
            }

            if (parameters.Contains("sudo"))
            {
                Output += " We're on Windows here, consider your privileges elevated by default.";
            }
            
        }
        public Task Execute(ClientState clientState)
        {
            return Task.FromResult<object>(null);
        }

        public string Output { get; private set; }
    }
}
