namespace GEMC.OBS.Client
{
    using System;
    using GEMC.Common;
    using OBSWebsocketDotNet;
    using OBSWebsocketDotNet.Types;

    public class ObsSceneManager : ISceneManager
    {
        private readonly Configuration configuration;
        private readonly ILogger logger;
        private OBSWebsocket webSocket;

        public ObsSceneManager(Configuration configuration, ILogger logger)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.webSocket = new OBSWebsocket();

            this.webSocket.Connected += OnConnect;
            this.webSocket.Disconnected += OnDisconnect;
        }

        public void Connect()
        {
            try
            {
                webSocket.Connect(this.configuration.ObsWebSocketUrl, this.configuration.ObsWebSocketPassword);
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

        public void SwitchScene(string newSceneName)
        {
            this.Connect();

            this.webSocket.SetCurrentScene(newSceneName);
            this.logger.Info(this.GetType(), $"Switch scene command with argument '{newSceneName}' sent to OBS");
            this.Disconnect();
        }

        public string GetCurrentScene()
        {
            this.Connect();

            OBSScene obsScene = this.webSocket.GetCurrentScene();

            this.Disconnect();

            return obsScene.Name;
        }

        public void Disconnect()
        {
            this.webSocket.Disconnect();
        }

        private void OnDisconnect(object sender, System.EventArgs e)
        {

        }

        private void OnConnect(object sender, System.EventArgs e)
        {

        }


    }

}