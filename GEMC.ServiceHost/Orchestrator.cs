namespace GEMC.ServiceHost
{
    using System;
    using Common;
    using MyRcm.Client;
    using OBS.Client;
    using System.Linq;
    using System.Threading;
    using Microsoft.Owin.Hosting;

    public class Orchestrator
    {
        private readonly ILogger logger;
        private readonly MessagesListener listener;
        private readonly MessageSender sender;
        private readonly LiveDataServer server;
        private readonly Configuration configuration;

        private Timer timer;

        public Orchestrator(ILogger logger, MessagesListener listener, MessageSender sender, LiveDataServer server, Configuration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.listener = listener ?? throw new ArgumentNullException(nameof(listener));
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Start()
        {
            // Start RCM listener
            this.listener.OneMinuteBeforeStart += OneMinuteBeforeStart;
            this.listener.RaceStarted += RaceStarted;
            this.listener.RaceEnded += RaceEnded;
            this.listener.MessageReceived += MessageReceived;
            this.listener.Start();
            

            //Start LiveDataServer
            this.server.Start();

            // Start REST webservice 
            string baseAddress = $"http://localhost:{configuration.WebServicePort}/";
            WebApp.Start<Startup>(url: baseAddress);
            this.logger.Info(typeof(Program), $"Webservice started, listening on {baseAddress}api/data");
        }

        private void MessageReceived(object o, DataEventArgs e)
        {
            this.server.SendMessage(e.Message);
        }

        private void RaceEnded(object o, DataEventArgs e)
        {
            this.listener.RaceEnded -= RaceEnded;

            SceneInfo raceEndedSceneInfo = this.configuration.SceneInfos.FirstOrDefault(si => si.Case.Equals("RaceEnded", StringComparison.InvariantCultureIgnoreCase));
            SceneInfo emptySceneInfo = this.configuration.SceneInfos.FirstOrDefault(si => si.Case.Equals("Empty", StringComparison.InvariantCultureIgnoreCase));

            if (raceEndedSceneInfo != null && emptySceneInfo != null)
            {
                this.sender.SwitchScene(raceEndedSceneInfo.ObsSceneName);
                Time showDelay = new Time(this.configuration.RaceResultsTableShowTime);
                this.timer = new System.Threading.Timer(RestoreScene, emptySceneInfo.ObsSceneName, showDelay.ToMilliSeconds(), Timeout.Infinite);
            }
        }

        private void RaceStarted(object o, DataEventArgs e)
        {
            this.listener.RaceStarted -= RaceStarted;

            SceneInfo sceneInfo = this.configuration.SceneInfos.FirstOrDefault(si => si.Case.Equals("RaceStarted", StringComparison.InvariantCultureIgnoreCase));

            if (sceneInfo != null)
            {
                this.sender.SwitchScene(sceneInfo.ObsSceneName);
            }

            this.listener.RaceStarted += RaceStarted;
        }

        private void OneMinuteBeforeStart(object o, DataEventArgs e)
        {
            this.listener.OneMinuteBeforeStart -= OneMinuteBeforeStart;

            SceneInfo OneMinuteBeforeStartSceneInfo = this.configuration.SceneInfos.FirstOrDefault(si => si.Case.Equals("OneMinuteBeforeStart", StringComparison.InvariantCultureIgnoreCase));
            SceneInfo emptySceneInfo = this.configuration.SceneInfos.FirstOrDefault(si => si.Case.Equals("Empty", StringComparison.InvariantCultureIgnoreCase));

            if (OneMinuteBeforeStartSceneInfo != null && emptySceneInfo != null)
            {
                this.sender.SwitchScene(OneMinuteBeforeStartSceneInfo.ObsSceneName);
                Time showDelay = new Time(this.configuration.RaceResultsTableShowTime);
                this.timer = new System.Threading.Timer(RestoreScene,emptySceneInfo.ObsSceneName, showDelay.ToMilliSeconds(), Timeout.Infinite);
            }
        }

        private void RestoreScene(object state)
        {
            string sceneName = state.ToString();
            this.sender.SwitchScene(sceneName);
            this.listener.OneMinuteBeforeStart += OneMinuteBeforeStart;
            this.timer = null;
        }

        public void Stop()
        {
            this.listener.Stop();
            this.listener.OneMinuteBeforeStart -= OneMinuteBeforeStart;
            this.listener.RaceStarted -= RaceStarted;
            this.listener.RaceEnded -= RaceEnded;

            this.logger.Info(typeof(Program), $"Server stopped");
        }
    }
}


//private void HandlesChanges(Message newMessage)
//{
//    if (previousMessage == null)
//    {
//        return;
//    }

//    Time countDown = new Time(newMessage.Event.Metadata.Countdown);

//    if (newMessage.Status == TimingStatus.BetweenRaces && previousMessage.Status == TimingStatus.RaceEnded)
//    {
//        //TODO: result grid between eor+15 to eor+45
//        container.SaveAsPreviousRaceResult(this.previousMessage);
//        newMessage.DisplayStatus = DisplayStatus.ResultGridDisplayed;
//        logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE ENDED, using {newMessage.DisplayStatus}");
//        this.SwitchSceneIfNeeded(newMessage);
//        this.previousMessage = newMessage;
//        return;

//    }

//    if (newMessage.Status == TimingStatus.BetweenRaces && countDown < OneMinuteDelay && countDown > TwentySecondsDelay)
//    {
//        newMessage.DisplayStatus = DisplayStatus.ResultGridDisplayed;
//        logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE PREPARATION, using {newMessage.DisplayStatus}");
//        this.SwitchSceneIfNeeded(newMessage);
//        this.previousMessage = newMessage;
//        return;
//    }

//    if (newMessage.Status == TimingStatus.BetweenRaces && countDown > Time.Zero && countDown < TwentySecondsDelay)
//    {
//        newMessage.DisplayStatus = DisplayStatus.RaceGridDisplayed;
//        logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE START IMMINENT, using {newMessage.DisplayStatus}");
//        this.SwitchSceneIfNeeded(newMessage);
//        this.previousMessage = newMessage;
//        return;
//    }

//    if (newMessage.Status == TimingStatus.RaceRunning || newMessage.Status == TimingStatus.RaceEnded)
//    {
//        newMessage.DisplayStatus = DisplayStatus.RaceGridDisplayed;
//        logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE IN PROGRESS, using {newMessage.DisplayStatus}");
//        this.SwitchSceneIfNeeded(newMessage);
//        this.previousMessage = newMessage;
//        return;
//    }

//    Time divergence = new Time(newMessage.Event.Metadata.Divergence);

//    if (newMessage.Status == TimingStatus.BetweenRaces && divergence > OneMinuteDelay)
//    {
//        newMessage.DisplayStatus = DisplayStatus.NothingDisplayed;
//        logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: WAITING FOR NEXT RACE, using {newMessage.DisplayStatus}");
//        this.SwitchSceneIfNeeded(newMessage);
//        this.previousMessage = newMessage;
//        return;
//    }

//    this.previousMessage = newMessage;
//    logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: SOMETHING STRANGE HAPPENING!");
//}

//private void SwitchSceneIfNeeded(Message newMessage)
//{
//    if (string.IsNullOrEmpty(this.lastAskedScene))
//    {
//        this.OnSceneChanging(newMessage.DisplayStatus.ToString());
//        this.lastAskedScene = newMessage.DisplayStatus.ToString();
//        logger.Info(this.GetType(), $"Scene switch to {newMessage.DisplayStatus}");
//    }

//    if (!newMessage.DisplayStatus.ToString().Equals(this.lastAskedScene))
//    {
//        this.OnSceneChanging(newMessage.DisplayStatus.ToString());
//        this.lastAskedScene = newMessage.DisplayStatus.ToString();
//        logger.Info(this.GetType(), $"Scene switched to {newMessage.DisplayStatus}");
//    }
//}