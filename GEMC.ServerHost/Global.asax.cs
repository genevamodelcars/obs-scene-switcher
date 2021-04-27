namespace GEMC.ServerHost
{
    using System.IO;
    using log4net.Config;
    using System;
    using System.Web.Http;
    using System.Reflection;
    using GEMC.Common;
    using GEMC.MyRcm.Client;
    using GEMC.OBS.Client;
    using GEMC.ServerHost.Configuration;

    public class Global : System.Web.HttpApplication
    {
        internal static ILogger Logger;
        internal static MessagesListener MessagesListener;
        internal static MessageSender MessageSender;
        internal static MessageContainer Container;

        protected void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            GlobalConfiguration.Configure(ServerConfiguration.Register);

            WindsorConfiguration.Register(GlobalConfiguration.Configuration);

            MessageSender = WindsorConfiguration.Container.Resolve<MessageSender>();
            
            MessagesListener = WindsorConfiguration.Container.Resolve<MessagesListener>();
            MessagesListener.SceneChanging += OnSceneChanging;


            MessagesListener.Start();

            Logger.Info(this.GetType(), "Server started");
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
            MessagesListener.Stop();
        }

        private static void OnSceneChanging(object sender, SceneInfoEventArgs e)
        {
            MessageSender.SwitchScene(e.Name);
        }
    }
}