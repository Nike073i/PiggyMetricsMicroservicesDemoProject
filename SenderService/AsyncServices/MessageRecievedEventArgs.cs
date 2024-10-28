using System;

namespace SenderService.AsyncServices
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public byte[] MessageData { get; }

        public bool Requeue { get; set; }

        public MessageRecievedEventArgs(byte[] messageData)
        {
            MessageData = messageData;
        }
    }
}
