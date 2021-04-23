namespace GEMC.Common
{
    public class MessageContainer
    {
        private readonly object messageLocker = new object();
        private Message message;

        public MessageContainer()
        {
            this.message = new Message();
        }
        
        public Message GetMessage()
        {
            lock (messageLocker)
            {
                return this.message;
            }
        }

        public void UpdateMessage(Message msg)
        {
            lock (messageLocker)
            {
                this.message = msg;
            }
        }
    }
}