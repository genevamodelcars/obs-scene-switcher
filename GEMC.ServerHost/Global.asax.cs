﻿using System.IO;
using log4net.Config;

namespace GEMC.ServerHost
{
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

        protected void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            GlobalConfiguration.Configure(ServerConfiguration.Register);

            WindsorConfiguration.Register(GlobalConfiguration.Configuration);

            Logger = WindsorConfiguration.Container.Resolve<ILogger>();

            MessageContainer container = WindsorConfiguration.Container.Resolve<MessageContainer>();

            MessagesListener = new MessagesListener("ws://192.168.254.60:8787", container, Logger);
            MessagesListener.SceneChanging += OnSceneChanging;

            MessagesListener.Start();

            Logger.Info(this.GetType(),"Server started");
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
            MessageSender messageSender = new MessageSender("ws://localhost:4444", "p@ssw0rd", Logger);

            messageSender.SwitchScene(e.Name);
        }
    }
}