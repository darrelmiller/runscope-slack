using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using RunscopeSlackApi.Commands;

namespace RunscopeSlackApi
{
    public interface IRunscopeCommand
    {
        string Name { get;  }
        Task Execute(ClientState clientState);
        string Output { get; }
        
    }
    public class CommandFactory
    {
        private Regex _CommandRegex = new Regex(@"runscope:\s*(?<cmd>\w+)\s*(?<params>.*)");

        public IRunscopeCommand CreateCommand(string text)
        {
            var matches = _CommandRegex.Match(text.ToLower());
            var cmd = matches.Groups["cmd"].Value;
            var cmdParameters = matches.Groups["params"].Value;

           
            switch (cmd)
            {
                case "run":
                    return new RunCommand(cmdParameters);
                case "show":
                    return new ShowCommand(cmdParameters);

                case "help":
                    return new HelpCommand(cmdParameters);

                case "make":
                case "sudo":
                    return new MakeCommand(cmdParameters);

                default:
                    return null;
            }
        }
    }
}
