using EyeXFramework;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace EyeTracker.Service.Core
{
    [ServiceBehavior(AutomaticSessionShutdown = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IEyeTrackerService, IDisposable
    {
        private EyeXHost host;
        public IEyeTrackerCallback Callback;
        private bool _keepAliveFaulted = false;
        private EyeTrackerMessage NextEyeTrackerMessage;

        public async Task ClientConnectRequest(Message msg)
        {
            if (OperationContext.Current != null)
            {
                OperationContext context = OperationContext.Current;
                MessageProperties prop = context.IncomingMessageProperties;

                //Logging origin, just for fun
                var endpoint = prop[WebSocketMessageProperty.Name] as WebSocketMessageProperty;
                string origin = endpoint?.WebSocketContext.Origin ?? "<null>";
                string requestUri = endpoint?.WebSocketContext.RequestUri.ToString() ?? "<null>";
                Console.WriteLine($"Client connected. Origin: {origin} RequestUri {requestUri} ");
            }

            Initialize();
            //_keepAliveFaulted = false;
            Callback = OperationContext.Current.GetCallbackChannel<IEyeTrackerCallback>();

            //Reply to client
            //KinectMessage kinectMessage = new KinectMessage(MessageType.Information, "Connected");
            //await Callback.SendKinectMessage(kinectMessage.CreateBinaryMessage());
            //kinectMessage.Dispose();

            //Start sending joint position each tick of the timer
            //KeepAliveTimer.Enabled = true;
            try
            {
                await SendEyeTrackerMessage();
            }
            catch (Exception e)
            {
                Console.WriteLine("ClientConnectRequest:Error sending message. Likely client has disconnected. ");
                Console.WriteLine(e.StackTrace);
                Dispose();
                throw;
            }

        }

        private void Initialize()
        {
            host = new EyeXHost();
            host.Start();
            var gazePointDataStream = host.CreateGazePointDataStream(Tobii.EyeX.Framework.GazePointDataMode.Unfiltered);
            gazePointDataStream.Next += (s, e) => this.NextEyeTrackerMessage = new EyeTrackerMessage() { X = e.X, Y = e.Y };
        }

        protected async Task SendEyeTrackerMessage()
        {
            if (Callback == null)
            {
                return;
            }

            while (!_keepAliveFaulted)
            {
                if (NextEyeTrackerMessage != null)
                {
                    //if (!KinectServiceBase.KinectMessages.TryDequeue(out kinectMessage)) continue;
                    await Callback.SendEyePosition(NextEyeTrackerMessage.CreateBinaryMessage());


                    //await callback.SendKinectMessage(kinectMessage.CreateBinaryMessage(kinectMessage.Depth));
                    //This option converts the message to a string and sends as Text
                    //var message = JsonConvert.SerializeObject(kinectMessage);
                    //await callback.SendKinectMessage(KinectMessage.CreateMessage(message));


                }
                await Task.Delay(100);
            }
            Dispose();
        }

        public void Dispose()
        {
            if (host != null)
            {
                try
                {
                    host.Dispose();
                }
                catch (Exception) { };
            }
        }
    }
}
