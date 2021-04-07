namespace GEMC.OBS.SceneSwitcher
{
    using System;
    using System.IO;
    using Castle.Facilities.Logging;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using GEMC.Common;
    using GEMC.MyRcm.Client;
    using log4net.Config;
    using CLROBS;

    public class Plugin : AbstractPlugin
    {
        internal IWindsorContainer NetConfigurationContainer { get; private set; }
        private Common.ILogger logger;
        private PluginConfiguration configuration;

        public Plugin()
        {
            this.Name = "OBS Scene Switcher";

            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            NetConfigurationContainer = new WindsorContainer();
            NetConfigurationContainer.Install(Configuration.FromAppConfig());
            NetConfigurationContainer.Install(new CastleInstaller());
            NetConfigurationContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net());
            this.logger = NetConfigurationContainer.Resolve<ILogger>();
            this.configuration = NetConfigurationContainer.Resolve<PluginConfiguration>();

            if (configuration != null)
            {
                MessagesListener listener = new MessagesListener(configuration.Url, logger);

                listener.SceneChanging += this.SceneChanging;

                listener.Start();

                Console.ReadKey(true);

                listener.Stop();
            }
        }

        private void SceneChanging(object sender, SceneInfoEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
