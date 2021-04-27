namespace GEMC.OBS.Client
{
    using System;
    using GEMC.Common;

    public class MessageSender
    {
        private ILogger logger;
        private readonly ISceneManager manager;


        public MessageSender( ILogger logger, ISceneManager manager)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.manager = manager;
        }

        public void SwitchScene(string newSceneName)
        {
            this.manager.SwitchScene(newSceneName);
        }

        public string GetCurrentScene()
        {
            return this.manager.GetCurrentScene();
        }
    }
}
