using System;
using System.IO;
using Castle.Facilities.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;
using GEMC.OBS.Client;
using GEMC.OBS.Common;
using log4net.Config;
using log4net.Core;

namespace GEMC.OBS.SceneSwitcher
{
    using CLROBS;

    public class Plugin : AbstractPlugin
    {
        internal static IWindsorContainer NetConfigurationContainer { get; private set; }
        private static Common.ILogger logger;

        public Plugin()
        {
            this.Name = "OBS Scene Switcher";

            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            NetConfigurationContainer = new WindsorContainer();
            NetConfigurationContainer.Install(Configuration.FromAppConfig());
            NetConfigurationContainer.Install(new CastleInstaller());
            NetConfigurationContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net());
            logger = NetConfigurationContainer.Resolve<Common.ILogger>();

            WebSocketClient client = new WebSocketClient("ws://192.168.254.60:8787", logger);

            while (true)
            {
                client.Connect();
            }
        }
    }
}
