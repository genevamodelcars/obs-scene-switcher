namespace GEMC.WebSocketTestClient
{
    using System;
    using System.IO;
    using GEMC.MyRcm.Client;
    using log4net.Config;

    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

            WindsorConfiguration.Register();

            MessagesListener listener = WindsorConfiguration.Container.Resolve<MessagesListener>();
            listener.MessageReceived += MessageReceived;
            listener.Start();

            Console.ReadLine();
        }

        private static void MessageReceived(object sender, DataEventArgs e)
        {
            Console.WriteLine(e.Message.Json);
        }
    }
}
