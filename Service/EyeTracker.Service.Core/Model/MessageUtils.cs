using System;
using System.Net.WebSockets;
using System.ServiceModel.Channels;

namespace EyeTracker.Service.Core.Model
{
    public static class Extensions
    {
        private static int messageTypeSize = sizeof(MessageType);

        internal static Message CreateMessage(this MessageBase message, byte[] messageDataBytes, WebSocketMessageType webSockectMsgType)
        {
            
            byte[] payload = new byte[messageTypeSize + messageDataBytes.Length];
            payload[0] = (byte)message.Type;

            Buffer.BlockCopy(messageDataBytes, 0, payload, 1, messageDataBytes.Length);
                
            Message msg = ByteStreamMessage.CreateMessage(
                new ArraySegment<byte>(payload));

            msg.Properties["WebSocketMessageProperty"] =
                new WebSocketMessageProperty
                {
                    MessageType = webSockectMsgType
                };

            return msg;
        }
    }
}
