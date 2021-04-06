using System;
using System.IO;
using Castle.Facilities.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;
using log4net.Config;
using log4net.Core;

namespace GEMC.OBS.SceneSwitcher
{
    using CLROBS;

    public class Plugin : AbstractPlugin
    {
        internal static IWindsorContainer NetConfigurationContainer { get; private set; }
        private static ILogger logger;

        public Plugin()
        {
            this.Name = "OBS Scene Switcher";

            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            NetConfigurationContainer = new WindsorContainer();
            NetConfigurationContainer.Install(Configuration.FromAppConfig());
            NetConfigurationContainer.Install(FromAssembly.This());
            NetConfigurationContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net());
            logger = NetConfigurationContainer.Resolve<ILogger>();
        }
    }
}
