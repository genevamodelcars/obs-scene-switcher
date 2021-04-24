using System.ComponentModel;
using System.Threading;

namespace GEMC.MyRcm.Client
{
    using System;
    using System.Threading.Tasks;
    using GEMC.Common;
    using WebSocketSharp;
    using Timer = System.Timers.Timer;

    public class MessagesListener : IDisposable
    {
        public EventHandler<SceneInfoEventArgs> SceneChanging;

        private static MessageContainer container;
        private static ILogger logger;
        private WebSocket webSocket;

        private static Time RaceDuration;
        private static Time TenSecondsDelay;
        private static Time TwentySecondsDelay;
        private static Time ThreeMinutesDelay;

        private Message previousMessage;
        private string lastAskedScene;

        private Time endRaceTime;

        public MessagesListener(string address, MessageContainer ctn, ILogger log)
        {
            container = ctn;
            logger = log;
            this.webSocket = new WebSocket(address, onMessage: OnMessage, onError: OnError, onClose: OnClose, onOpen: OnOpen);
            this.webSocket.WaitTime = TimeSpan.FromMinutes(5);

            RaceDuration = new Time("00:05:00");
            TenSecondsDelay = new Time("00:00:10");
            TwentySecondsDelay = new Time("00:00:20");
            ThreeMinutesDelay = new Time("00:01:15");

        }

        public void Start()
        {
            this.webSocket.Connect().Wait();
        }

        public void Stop()
        {
            this.webSocket.Close(CloseStatusCode.Normal).Wait();
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
            Message message = Message.FromJson(json);
            container.UpdateMessage(message);
            logger.Debug(this.GetType(), $"TIMESTAMP:{message.TimeStamp.ToString("HH:mm:ss")} STATUS:{message.Status}, CT:{message.Event.Metadata.Countdown}, CU:{message.Event.Metadata.CurrentTime}, RC:{message.Event.Metadata.RaceTime}, RM:{message.Event.Metadata.RemainingTime}, DV:{message.Event.Metadata.Divergence}");
            this.HandlesChanges(message);
            this.previousMessage = message;

            
            return Task.FromResult(0);
        }

        public void OnSceneChanging(string text)
        {
            this.SceneChanging?.Invoke(this, new SceneInfoEventArgs(text));
        }

        private void HandlesChanges(Message newMessage)
        {
            if (previousMessage == null)
            {
                return;
            }

            Time countDown = new Time(newMessage.Event.Metadata.Countdown);

            if (newMessage.Status == TimingStatus.BetweenRaces && previousMessage.Status == TimingStatus.RaceEnding)
            {
                //TODO: result grid between eor+15 to eor+45
                container.SaveAsRaceResultMessage(this.previousMessage);
                newMessage.DisplayStatus = DisplayStatus.ResultGridDisplayed;
                logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE ENDED, using {newMessage.DisplayStatus}");
                this.SwitchSceneIfNeeded(newMessage);
                this.previousMessage = newMessage;
                return;

            }

            if (newMessage.Status == TimingStatus.BetweenRaces && countDown < ThreeMinutesDelay && countDown > TwentySecondsDelay)
            {
                newMessage.DisplayStatus = DisplayStatus.ResultGridDisplayed;
                logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE PREPARATION, using {newMessage.DisplayStatus}");
                this.SwitchSceneIfNeeded(newMessage);
                this.previousMessage = newMessage;
                return;
            }

            if (newMessage.Status == TimingStatus.BetweenRaces && countDown > Time.Zero && countDown < TwentySecondsDelay)
            {
                newMessage.DisplayStatus = DisplayStatus.RaceGridDisplayed;
                logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE START IMMINENT, using {newMessage.DisplayStatus}");
                this.SwitchSceneIfNeeded(newMessage);
                this.previousMessage = newMessage;
                return;
            }
            
            if (newMessage.Status == TimingStatus.RaceRunning || newMessage.Status == TimingStatus.RaceEnding)
            {
                newMessage.DisplayStatus = DisplayStatus.RaceGridDisplayed;
                logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: RACE IN PROGRESS, using {newMessage.DisplayStatus}");
                this.SwitchSceneIfNeeded(newMessage);
                this.previousMessage = newMessage;
                return;
            }

            Time divergence = new Time(newMessage.Event.Metadata.Divergence);

            if (newMessage.Status == TimingStatus.BetweenRaces && divergence > ThreeMinutesDelay)
            {
                newMessage.DisplayStatus = DisplayStatus.NothingDisplayed;
                logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: WAITING FOR NEXT RACE, using {newMessage.DisplayStatus}");
                this.SwitchSceneIfNeeded(newMessage);
                this.previousMessage = newMessage;
                return;
            }
            
            this.previousMessage = newMessage;
            logger.Debug(this.GetType(), $"{newMessage.TimeStamp.ToString("HH:mm:ss")}: SOMETHING STRANGE HAPPENING!");
        }

        private void SwitchSceneIfNeeded(Message newMessage)
        {
            if (string.IsNullOrEmpty(this.lastAskedScene))
            {
                this.OnSceneChanging(newMessage.DisplayStatus.ToString());
                this.lastAskedScene = newMessage.DisplayStatus.ToString();
                logger.Info(this.GetType(), $"Scene switch to {newMessage.DisplayStatus}");
            }

            if (!newMessage.DisplayStatus.ToString().Equals(this.lastAskedScene))
            {
                this.OnSceneChanging(newMessage.DisplayStatus.ToString());
                this.lastAskedScene = newMessage.DisplayStatus.ToString();
                logger.Info(this.GetType(), $"Scene switched to {newMessage.DisplayStatus}");
            }
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

        public void Dispose()
        {
            this.webSocket.Close();
            this.webSocket?.Dispose();
        }
    }
}
