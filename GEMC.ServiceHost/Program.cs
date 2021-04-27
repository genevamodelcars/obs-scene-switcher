using System.Runtime.CompilerServices;

namespace GEMC.ServiceHost
{
    using System.IO;
    using GEMC.Common;
    using GEMC.MyRcm.Client;
    using GEMC.OBS.Client;
    using log4net.Config;
    using System;
    using System.Net.Http;
    using Microsoft.Owin.Hosting;

    class Program
    {
        internal static Orchestrator Orchestrator;
        internal static Configuration Configuration;

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            WindsorConfiguration.Register();

            Orchestrator = WindsorConfiguration.Container.Resolve<Orchestrator>();

            Orchestrator.Start();

            Console.WriteLine("Press Enter to quit...");
            Console.ReadLine();

        }


    }
}
