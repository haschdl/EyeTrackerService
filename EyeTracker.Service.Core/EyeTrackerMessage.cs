using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.Text;

namespace EyeTracker.Service.Core
{

    [ComVisible(true)]
    public class EyeTrackerMessage : IDisposable
    {
        public byte[] DataArray;

        public double X;
        public double Y;

        public void Dispose()
        {
            throw new NotImplementedException();
        }



        public Message CreateBinaryMessage()
        {
            byte[] messageDataBytes = new byte[16];
            //.net double =>> 8 bytes/64bits
            var webSockectMsgType = WebSocketMessageType.Binary;
            Buffer.BlockCopy(BitConverter.GetBytes(X), 0, messageDataBytes, 0, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(Y), 0, messageDataBytes, 8, 8);

            if (messageDataBytes == null)
                return null;

            Message msg = ByteStreamMessage.CreateMessage(
                new ArraySegment<byte>(messageDataBytes));

            msg.Properties["WebSocketMessageProperty"] =
                new WebSocketMessageProperty
                {
                    MessageType = webSockectMsgType,


                };
            return msg;
        }
    }
}