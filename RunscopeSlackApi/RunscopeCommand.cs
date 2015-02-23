using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headers;
using Tavis.Headers.Elements;
using Tavis.Parser;

namespace RunscopeSlackApi
{
    public class RunscopeCommand
    {
        public static IExpression Syntax = new Expression("cmd")
            {
                new Ows(),
                new Literal("runscope:"),
                new Ows(),
                new Headers.Token("verb"),
                new Rws(),
                new OrExpression("bucket") {
                    new Headers.QuotedString("qbucket"),
                    new Headers.Token("tbucket")
                },
                new Ows(),
                new OptionalExpression("testphrase")
                {
                    new Literal("/"),
                    new Ows(),
                    new OrExpression("test") {
                        new Headers.QuotedString("qtest"),
                        new Headers.Token("ttest")
                    }
                },
                new OptionalExpression("paramlist")
                {
                    new Rws(),
                    new Literal("with"),
                    new CommaList("parameters",Parameter.Syntax)    
                }
                
            };
    }
}
