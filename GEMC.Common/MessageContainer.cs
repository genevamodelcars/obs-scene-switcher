namespace GEMC.Common
{
    public class MessageContainer
    {
        private readonly object messageLocker = new object();
        private readonly object raceMessageLocker = new object();
        private Message currentMessage;
        private Message raceResultMessage;

        public MessageContainer()
        {
            this.currentMessage = new Message();
            this.raceResultMessage = new Message();
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

        public void SaveAsRaceResultMessage(Message msg)
        {
            lock (raceMessageLocker)
            {
                this.raceResultMessage = msg;
            }
        }

        public Message GetRaceResultMessage()
        {
            lock (raceMessageLocker)
            {
                return this.raceResultMessage;
            }
        }
    }
}