using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;
using HttpCheckApi;
using RunscopeSlackApi;

namespace RunscopeSlack
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        
        {

            var config = GlobalConfiguration.Configuration;

            var privateDataHandler = new PrivateDataMessageHandler(Server.MapPath("~/App_Data/privatedata.json"));
            config.MessageHandlers.Add(privateDataHandler);
            
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}