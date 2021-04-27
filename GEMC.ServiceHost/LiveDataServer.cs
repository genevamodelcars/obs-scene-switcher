using System.Linq;
using System.Net;
using System.Text;

namespace GEMC.ServiceHost
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fleck;
    using GEMC.Common;
    using Newtonsoft.Json;

    public class LiveDataServer
    {
        private readonly ILogger logger;
        private readonly Configuration configuration;
        private readonly WebSocketServer server;
        readonly List<IWebSocketConnection> sockets = new List<IWebSocketConnection>();

        public LiveDataServer(ILogger logger, Configuration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.server = new WebSocketServer($"ws://0.0.0.0:{this.configuration.LiveDataServerPort}");
            this.server.RestartAfterListenError = true;

            this.logger.Info(this.GetType(), $"WebSocket ready, listening on ws://{this.GetIpAddress()}:{this.configuration.LiveDataServerPort}");
        }

        public void Start()
        {
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    this.logger.Info(this.GetType(), "Connected to " + socket.ConnectionInfo.ClientIpAddress);
                    sockets.Add(socket);
                };

                socket.OnClose = () =>
                {
                    this.logger.Info(this.GetType(), "Disconnected from " + socket.ConnectionInfo.ClientIpAddress);
                    sockets.Remove(socket);
                };

                socket.OnMessage = message =>
                {
                    socket.Send(message);
                };
            });
        }

        public void SendMessage(Message message)
        {
            string json = JsonConvert.SerializeObject(message);

            Parallel.ForEach(this.sockets, socket => socket.Send(json));
        }
        private string GetIpAddress()
        {
            String hostName = string.Empty;
            hostName = Dns.GetHostName();
            IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
            IPAddress[] address = ipHostEntry.AddressList;
            return address.Last().ToString();
        }
    }
}