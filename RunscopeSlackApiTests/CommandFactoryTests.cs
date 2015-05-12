using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunscopeSlackApi;
using RunscopeSlackApi.Commands;
using Xunit;

namespace RunscopeSlackApiTests
{
    public class CommandFactoryTests
    {
        [Fact]
        public void TestHelpCommand()
        {
            var cmd = new CommandFactory().CreateCommand("runscope: help");

            Assert.IsType(typeof(HelpCommand), cmd);

        }
    }
}
