using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunscopeSlackApi;
using RunscopeSlackApi.Commands;
using Tavis.Parser;
using Xunit;

namespace RunscopeSlackApiTests
{
    public class HelpCommandTests
    {
        [Fact]
        public void SimpleRunCommand()
        {

         
            var cmd = new HelpCommand(null);
            cmd.Execute(null);
            Debug.WriteLine(cmd.Output);
            Assert.NotNull(cmd.Output);
        }
    }
}
