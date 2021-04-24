namespace GEMC.OBS.Client
{
    using System;
    using OBSWebsocketDotNet;
    using GEMC.Common;

    public class MessageSender
    {
        private ILogger logger;
        private OBSWebsocket webSocket;
        private string url;
        private string password;

        public MessageSender(string url, string password, ILogger logger)
        {
            this.url = url;
            this.password = password;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.webSocket = new OBSWebsocket();

            this.webSocket.Connected += OnConnect;
            this.webSocket.Disconnected += OnDisconnect; ;
        }

        public void SwitchScene(string sceneName)
        {
            this.EnsuresConnection();

            this.webSocket.SetCurrentScene(sceneName);
        }

        private void OnDisconnect(object sender, System.EventArgs e)
        {

        }

        private void OnConnect(object sender, System.EventArgs e)
        {

        }

        private void EnsuresConnection()
        {
            if (!this.webSocket.IsConnected)
            {
                try
                {
                    webSocket.Connect(this.url, this.password);
                }
                catch (AuthFailureException ex)
                {
                    logger.Error(this.GetType(), "There was an authentication exception", ex);
                }
                catch (ErrorResponseException ex)
                {
                    logger.Error(this.GetType(), "There was a communication exception", ex);
                }
            }
        }


    }
}
