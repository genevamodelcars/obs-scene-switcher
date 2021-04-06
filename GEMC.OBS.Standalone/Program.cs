#pragma warning disable 618

namespace GEMC.OBS.Standalone
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Castle.Facilities.Logging;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using GEMC.OBS.Common;
    using log4net.Config;
    using WebSocketSharp;
    using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;

    class Program
    {
        internal static IWindsorContainer NetConfigurationContainer { get; private set; }
        private static Common.ILogger logger;

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            NetConfigurationContainer = new WindsorContainer();
            NetConfigurationContainer.Install(Configuration.FromAppConfig());
            NetConfigurationContainer.Install(new CastleInstaller());
            NetConfigurationContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net());
            logger = NetConfigurationContainer.Resolve<Common.ILogger>();

            using (WebSocket socket =  new WebSocket(url: "ws://192.168.254.60:8787", onMessage: OnMessage, onError: OnError))
            {
                socket.Connect().Wait();
            }

            Console.ReadLine();
        }

        private static Task OnError(ErrorEventArgs errorEventArgs)
        {
            //this.logger.Error(this.GetType(), errorEventArgs.Message, errorEventArgs.Exception);
            Console.Write("Error: {0}, Exception: {1}", errorEventArgs.Message, errorEventArgs.Exception);
            return Task.FromResult(0);
        }

        private static Task OnMessage(MessageEventArgs messageEventArgs)
        {
            //this.logger.Debug(this.GetType(), messageEventArgs.Text.ReadToEnd());
            Console.Write("Message received: {0}", messageEventArgs.Text.ReadToEnd());
            return Task.FromResult(0);
        }
    }
}
