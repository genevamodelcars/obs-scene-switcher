namespace GEMC.Common
{
    public class MessageContainer
    {
        private readonly object messageLocker = new object();
        private readonly object previousRaceMessageLocker = new object();
        private readonly object nextRaceMessageLocker = new object();
        private Message currentMessage;
        private Message previousRaceResultMessage;
        private Message nextRaceResultMessage;

        public MessageContainer()
        {
            this.currentMessage = new Message();
            this.previousRaceResultMessage = new Message();
        }
        
        public Message GetMessage()
        {
            lock (messageLocker)
            {
                return this.currentMessage;
            }
        }

        public void UpdateMessage(Message msg)
        {
            lock (messageLocker)
            {
                this.currentMessage = msg;
            }
        }

        public void SaveAsPreviousRaceResult(Message msg)
        {
            lock (previousRaceMessageLocker)
            {
                this.previousRaceResultMessage = msg;
            }
        }

        public void SaveAsNextRaceInfo(Message msg)
        {
            lock (nextRaceMessageLocker)
            {
                this.nextRaceResultMessage = msg;
            }
        }

        public Message GetPreviousRaceMessage()
        {
            lock (previousRaceMessageLocker)
            {
                return this.previousRaceResultMessage;
            }
        }

        public Message GetNextRaceMessage()
        {
            lock (nextRaceMessageLocker)
            {
                return this.nextRaceResultMessage;
            }
        }
    }
}