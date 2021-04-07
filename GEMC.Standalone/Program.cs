#pragma warning disable 618

namespace GEMC.Standalone
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Castle.Facilities.Logging;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using GEMC.Common;
    using GEMC.MyRcm.Client;
    using log4net.Config;

    public static class Program
    {
        internal static IWindsorContainer NetConfigurationContainer { get; private set; }
        private static ILogger logger;
        private static List<Message> messages = new List<Message>();

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            NetConfigurationContainer = new WindsorContainer();
            NetConfigurationContainer.Install(Configuration.FromAppConfig());
            NetConfigurationContainer.Install(new CastleInstaller());
            NetConfigurationContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net());
            logger = NetConfigurationContainer.Resolve<Common.ILogger>();

            MessagesListener messagesListener = new MessagesListener("ws://192.168.254.60:8787", logger);
            messagesListener.SceneChanging += OnSceneChanging;
            
            messagesListener.Start();

            Console.ReadKey(true);
            messagesListener.Stop();

            if (messagesListener.Messages != null)
            {
                foreach (Message message in messagesListener.Messages)
                {
                    logger.Info(typeof(Program), $"Orphan message: {message.Json}");
                }
            }
        }

        private static void OnSceneChanging(object sender, SceneInfoEventArgs e)
        {
            GEMC.OBS.Client.MessageSender messageSender = new MessageSender();

            sender.SwitchScene(e.Name);
        }
    }
}
