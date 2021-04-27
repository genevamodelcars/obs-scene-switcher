using GEMC.Common;

namespace GEMC.MyRcm.Client
{
    public class DataEventArgs
    {
        public Message Message { get; set; }

        public Message PreviousMessage { get; set; }

        public DataEventArgs(Message message, Message previousMessage)
        {
            Message = message;
            PreviousMessage = previousMessage;
        }
    }
}