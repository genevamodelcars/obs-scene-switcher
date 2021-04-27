namespace GEMC.MyRcm.Client
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using WebSocketSharp;
    using System.Threading;

    public class MessagesListener : IDisposable
    {
        public EventHandler<DataEventArgs> RaceStarted;
        public EventHandler<DataEventArgs> RaceEnded;
        public EventHandler<DataEventArgs> PresentationBeforeStart;
        public EventHandler<DataEventArgs> MessageReceived;

        private static MessageContainer container;
        private static ILogger logger;
        private static Time NextRaceTableDelay;

        private readonly Configuration configuration;

        private WebSocket webSocket;
        private Message previousMessage;

        public MessagesListener(MessageContainer container, ILogger logger, Configuration configuration)
        {
            MessagesListener.container = container;
            MessagesListener.logger = logger;
            this.configuration = configuration;
            this.webSocket = new WebSocket(this.configuration.RcmWebSocketUrl, onMessage: OnMessage, onError: OnError, onClose: OnClose, onOpen: OnOpen);
            this.webSocket.WaitTime = TimeSpan.FromMinutes(10);

            NextRaceTableDelay = new Time(this.configuration.NextRaceTableDelay);
        }

        public void Start()
        {
            this.webSocket.Connect().Wait();
            logger.Info(typeof(Program), $"RCM SocketListener connected to {this.configuration.RcmWebSocketUrl}");
        }

        public void Stop()
        {
            this.webSocket.Close(CloseStatusCode.Normal).Wait();
        }

        private Task OnClose(CloseEventArgs closeEventArgs)
        {
            logger.Info(this.GetType(), "Socket closed.");
            return Task.FromResult(0);
        }

        private Task OnOpen()
        {
            logger.Info(this.GetType(), "Socket open.");
            return Task.FromResult(0);
        }

        private Task OnError(ErrorEventArgs errorEventArgs)
        {
            logger.Error(this.GetType(), errorEventArgs.Message, errorEventArgs.Exception);
            Thread.Sleep(8000);
            this.webSocket.Connect().Wait();
            return Task.FromResult(0);
        }

        private Task OnMessage(MessageEventArgs messageEventArgs)
        {
            string json = messageEventArgs.Text.ReadToEnd();
            Message newMessage = Message.FromJson(json);
            container.UpdateMessage(newMessage);
            logger.Debug(this.GetType(), $"TIMESTAMP:{newMessage.TimeStamp.ToString("HH:mm:ss")} STATUS:{newMessage.Status}, Countdown:{newMessage.Event.Metadata.Countdown}, CurrentTime:{newMessage.Event.Metadata.CurrentTime}, RaceTime:{newMessage.Event.Metadata.RaceTime}, RemainingTime:{newMessage.Event.Metadata.RemainingTime}, Divergence:{newMessage.Event.Metadata.Divergence}");
                               
            this.HandleEvents(this.previousMessage, newMessage);

            this.previousMessage = newMessage;
            return Task.FromResult(0);
        }

        private void OnRaceStarted(Message message)
        {
            this.RaceStarted?.Invoke(this, new DataEventArgs(message, null));
        }

        private void OnRaceEnded(Message message, Message previousMessage)
        {
            this.RaceEnded?.Invoke(this, new DataEventArgs(message, previousMessage));
        }

        private void OnPresentationBeforeStart(Message message)
        {
            this.PresentationBeforeStart?.Invoke(this, new DataEventArgs(message, null));
        }

        private void OnMessageReceived(Message message)
        {
            this.MessageReceived?.Invoke(this, new DataEventArgs(message, null));
        }

        private void HandleEvents(Message message, Message newMessage)
        {
            // Handling the first message
            if (previousMessage == null)
            {
                return;
            }

            this.OnMessageReceived(newMessage);
            
            Time countDown = new Time(newMessage.Event.Metadata.Countdown);

            // Race ended case
            if (newMessage.Status == TimingStatus.BetweenRaces && previousMessage.Status == TimingStatus.RaceEnded)
            {
                container.SaveAsPreviousRaceResult(this.previousMessage);
                container.SaveAsNextRaceInfo(newMessage);

                logger.Info(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE ENDED");

                this.OnRaceEnded(newMessage, this.previousMessage);

                this.previousMessage = newMessage;
            }

            // Race started
            if (newMessage.Status == TimingStatus.RaceRunning && previousMessage.Status == TimingStatus.BetweenRaces)
            {
                logger.Info(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE STARTED");

                this.OnRaceStarted(newMessage);

                this.previousMessage = newMessage;
            }

            // One minute before start
            if (newMessage.Status == TimingStatus.BetweenRaces && countDown == NextRaceTableDelay)
            {
                logger.Info(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE START in {NextRaceTableDelay}");

                this.OnPresentationBeforeStart(newMessage);

                this.previousMessage = newMessage;
            }

        }

        public void Dispose()
        {
            this.webSocket.Close();
            this.webSocket?.Dispose();
        }
    }
}
