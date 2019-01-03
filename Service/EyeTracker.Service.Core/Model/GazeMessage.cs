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
        public bool HasLeftEyePosition { get; internal set; }
        public bool HasRightEyePosition { get; internal set; }

        //public Tuple<double, double> HeadPosition = new Tuple<double, double>(0, 0);

        public GazeMessage(double x, double y) : base(MessageType.GAZE)
        {            
            this.GazePoint = new Tuple<double, double>(x, y);
        }

        public override string ToString() => $"GazeMessage [{GazePoint.Item1:F1},{GazePoint.Item2:F1}]";       

        public override Message CreateBinaryMessage()
        {
            const int DOUBLE_LEN = 8;//.net double =>> 8 bytes/64bits
            const int BOOL_LEN = 1;//.net GetBytes from boolean =>> 1 bytes

            //2 bytes for  HasLeftEye and HasRightEye (1 byte each) +
            //8 bytes for x, 8 byte for y
            byte[] messageDataBytes = new byte[2+DOUBLE_LEN * 2]; 
       
            int n = 0;
            Buffer.BlockCopy(BitConverter.GetBytes(this.HasLeftEyePosition), 0, messageDataBytes, n++, BOOL_LEN);
            Buffer.BlockCopy(BitConverter.GetBytes(this.HasRightEyePosition), 0, messageDataBytes, n++, BOOL_LEN);

            Buffer.BlockCopy(BitConverter.GetBytes(GazePoint.Item1), 0, messageDataBytes, n, DOUBLE_LEN);
            Buffer.BlockCopy(BitConverter.GetBytes(GazePoint.Item2), 0, messageDataBytes, n + DOUBLE_LEN, DOUBLE_LEN);

            if (messageDataBytes == null)
                return null;


            return this.CreateMessage(messageDataBytes, WebSocketMessageType.Binary);
        }


    }
}