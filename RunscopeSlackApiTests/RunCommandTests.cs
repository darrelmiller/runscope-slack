using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using RunscopeSlackApi;
using Tavis.Parser;
using Xunit;

namespace RunscopeSlackApiTests
{
    public class RunCommandTests
    {
        [Fact]
        public void SimpleRunCommand()
        {
           
            var parseNodes = RunscopeCommand.Syntax.Consume(new Inputdata("runscope: run \"foo fun\"/\"bar\""));

            var cmd = new RunCommand(parseNodes);
            Assert.Equal("foo fun",cmd.BucketName);
            Assert.Equal("bar",cmd.TestName);
        }

        [Fact]
        public void SimpleRunCommandNotQuoted()
        {

            var parseNodes = RunscopeCommand.Syntax.Consume(new Inputdata("runscope: run foo/bar"));

            var cmd = new RunCommand(parseNodes);
            Assert.Equal("foo", cmd.BucketName);
            Assert.Equal("bar", cmd.TestName);
        }

        [Fact]
        public void RunCommandMixedQuoted()
        {

            var parseNodes = RunscopeCommand.Syntax.Consume(new Inputdata("runscope: run \"foo\"/bar"));

            var cmd = new RunCommand(parseNodes);
            Assert.Equal("foo", cmd.BucketName);
            Assert.Equal("bar", cmd.TestName);
        }


        [Fact]
        public void SimpleRunCommandWithParameters()
        {

            var parseNodes = RunscopeCommand.Syntax.Consume(new Inputdata("runscope: run \"foo fun\"/\"bar\" with x=7,y=hello"));

            var cmd = new RunCommand(parseNodes);
            Assert.Equal(cmd.BucketName, "foo fun");
            Assert.Equal(cmd.TestName, "bar");
            Assert.Equal("x", cmd.Parameters[0].Name);
            Assert.Equal("7", cmd.Parameters[0].Value);
            Assert.Equal("y", cmd.Parameters[1].Name );
            Assert.Equal("hello", cmd.Parameters[1].Value);
        }
    }
}
