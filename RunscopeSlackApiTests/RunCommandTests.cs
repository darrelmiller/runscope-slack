using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using RunscopeSlackApi;
using RunscopeSlackApi.Commands;
using Tavis.Parser;
using Xunit;

namespace RunscopeSlackApiTests
{
    public class RunCommandTests
    {
       

        [Fact]
        public void Run_command_for_bucket_with_spaces()
        {
            var cmd = new RunCommand("foo fun/bar");
            Assert.Equal("foo fun", cmd.BucketName);
            Assert.Equal("bar", cmd.TestName);
        }
        [Fact]
        public void Run_command_with_space_in_test()
        {
            var cmd = new RunCommand("foo/bar baz");
            Assert.Equal("foo", cmd.BucketName);
            Assert.Equal("bar baz", cmd.TestName);
        }
        [Fact]
        public void Run_command_with_space_in_bucket_and_test()
        {
            var cmd = new RunCommand(" foo far / bar baz ");
            Assert.Equal("foo far", cmd.BucketName);
            Assert.Equal("bar baz", cmd.TestName);
        }
        [Fact]
        public void Run_command_with_no_test()
        {
            var cmd = new RunCommand("foo");
            Assert.Equal("foo", cmd.BucketName);
            Assert.Equal("", cmd.TestName);
        }

        [Fact]
        public void Run_command_with_no_test_but_with_parameters()
        {
            var cmd = new RunCommand("foo with x =y , p= q");
            Assert.Equal("foo", cmd.BucketName);
            Assert.Equal("", cmd.TestName);
            Assert.Equal(2, cmd.Parameters.Count);
            var firstParam = cmd.Parameters.First();
            Assert.Equal("x", firstParam.Name);
            Assert.Equal("y", firstParam.Value);
            var secondParam = cmd.Parameters.Skip(1).First();
            Assert.Equal("p", secondParam.Name);
            Assert.Equal("q", secondParam.Value); 
        }

        
    }
}
