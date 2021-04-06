using GEMC.OBS.Common;

namespace GEMC.OBS.Client
{
    using System.Threading.Tasks;
    using WebSocketSharp;

    public class WebSocketClient
    {
        private readonly string address;
        private readonly ILogger logger;

        public WebSocketClient(string address, ILogger logger)
        {
            this.address = address;
            this.logger = logger;
        }

        public void Connect()
        {
            using (var socket = new WebSocket(url: this.address, onMessage: OnMessage, onError: OnError))
            {
                socket.Connect().Wait();
            }
        }

        private Task OnError(ErrorEventArgs errorEventArgs)
        {
            this.logger.Error(this.GetType(),errorEventArgs.Message,errorEventArgs.Exception);
            
            return Task.FromResult(0);
        }

        private Task OnMessage(MessageEventArgs messageEventArgs)
        {
            this.logger.Debug(this.GetType(), messageEventArgs.Text.ReadToEnd());
            return Task.FromResult(0);
        }

    }

}
