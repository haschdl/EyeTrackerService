using System;
using System.Runtime.InteropServices;
using System.Net.WebSockets;
using System.ServiceModel.Channels;

namespace EyeTracker.Service.Core.Model
{


    [ComVisible(true)]
    public class UserPresenceMessage : MessageBase, IDisposable
    {

        public byte Presence { get; }


        public UserPresenceMessage(byte presence) : base(MessageType.USER_PRESENCE)
        {
            this.Presence = presence;
        }



        public override Message CreateBinaryMessage()
        {
            const int LEN = 1; //for byte Presence
            byte[] messageDataBytes = new byte[LEN];
            messageDataBytes[0] = Presence;
            if (messageDataBytes == null)
                return null;

            return this.CreateMessage(messageDataBytes, WebSocketMessageType.Binary);
        }


    }
}