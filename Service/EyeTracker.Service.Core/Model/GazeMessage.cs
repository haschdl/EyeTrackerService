using System;
using System.Runtime.InteropServices;
using System.Net.WebSockets;
using System.ServiceModel.Channels;

namespace EyeTracker.Service.Core.Model
{


    [ComVisible(true)]
    public class GazeMessage : MessageBase, IDisposable
    {

        public Tuple<double, double> GazePoint = new Tuple<double, double>(0, 0);
        public Tuple<double, double> HeadPosition = new Tuple<double, double>(0, 0);


        public GazeMessage(double x, double y) : base(MessageType.GAZE)
        {            
            this.GazePoint = new Tuple<double, double>(x, y);
        }

        public override string ToString() => $"GazeMessage [{GazePoint.Item1:F1},{GazePoint.Item2:F1}]";

        public override Message CreateBinaryMessage()
        {
            const int LEN = 8;//.net double =>> 8 bytes/64bits
            byte[] messageDataBytes = new byte[LEN * 2];
            int n = 0;

            Buffer.BlockCopy(BitConverter.GetBytes(GazePoint.Item1), 0, messageDataBytes, LEN * n++, LEN);
            Buffer.BlockCopy(BitConverter.GetBytes(GazePoint.Item2), 0, messageDataBytes, LEN * n++, LEN);

            if (messageDataBytes == null)
                return null;


            return this.CreateMessage(messageDataBytes, WebSocketMessageType.Binary);
        }


    }
}