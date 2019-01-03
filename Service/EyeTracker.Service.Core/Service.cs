using EyeTracker.Service.Core.Model;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Tobii.Interaction;
using Tobii.Interaction.Framework;
using Tobii.Interaction.Model;

namespace EyeTracker.Service.Core
{
    [ServiceBehavior(AutomaticSessionShutdown = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IEyeTrackerService, IDisposable
    {
        private Host host;
        public IEyeTrackerCallback Callback;
        private bool _keepAliveFaulted = false;
        private GazeMessage NextGazeMessage;
        private UserPresenceMessage NextUserPresenceMessage;
        private bool HasLeftEyePosition;
        private bool HasRightEyePosition;
        private UserPresence LastUserPresence = UserPresence.NotPresent;

        public double lastGazeMessage = 0;
        public double lastLeftEyeValid = 0;
        public double lastRightEyeValid = 0;


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
 
            Callback = OperationContext.Current.GetCallbackChannel<IEyeTrackerCallback>();

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
            host = new Host();
            var presenceObserver = host.States.CreateUserPresenceObserver();
            presenceObserver.Changed += presenceObserverChange;
        

            var gazePointStream = host.Streams.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered, true);
            gazePointStream.Next += gazePointStreamNext;

            var eyePositionStream = host.Streams.CreateEyePositionStream(true);
            eyePositionStream.Next += EyePositionStream_Next;

            /*
            var fixationStream = host.Streams.CreateFixationDataStream(FixationDataMode.Sensitive, true);
            fixationStream.Next += fixationStreamNext;
            */
        }

        private void EyePositionStream_Next(object sender, StreamData<EyePositionData> e)
        {
            this.HasLeftEyePosition = e.Data.HasLeftEyePosition;
            this.HasRightEyePosition = e.Data.HasRightEyePosition;
           //Console.WriteLine("IsValid: (Left :{0}, Right: {1}) @{2:0}", e.Data.HasLeftEyePosition, e.Data.HasRightEyePosition, e.Data.Timestamp);
        }

        private void completionHandler(IAsyncData asyncData)
        {
            Console.WriteLine("Loaded profile");
        }


        private void gazePointStreamNext(object sender, StreamData<GazePointData> e)
        {
            this.NextGazeMessage = new GazeMessage(e.Data.X, e.Data.Y);

            this.NextGazeMessage.HasLeftEyePosition = this.HasLeftEyePosition;
            this.NextGazeMessage.HasRightEyePosition = this.HasRightEyePosition;

            //Logging at every n-th milliseconds
            if (e.Data.Timestamp - lastGazeMessage > 1000)
            {
                Console.WriteLine("Interactor: " + e.InteractorId + " " + NextGazeMessage.ToString());
                lastGazeMessage = e.Data.Timestamp;
            }
        }

        private void presenceObserverChange(object sender, EngineStateValue<UserPresence> e)
        {
            if (e.IsValid)
            {
                NextUserPresenceMessage = new UserPresenceMessage(e.Value == UserPresence.Present);
            }
            //Console.WriteLine($"Presence observer value: {e.Value} (Valid: {e.IsValid})");
        }

        protected async Task SendEyeTrackerMessage()
        {
            if (Callback == null)
            {
                return;
            }

            while (!_keepAliveFaulted)
            {

                if (NextGazeMessage != null)
                {
                    await Callback.SendTrackerMessage(NextGazeMessage.CreateBinaryMessage());
                    NextGazeMessage = null;
                }
                if (NextUserPresenceMessage != null)
                {
                    await Callback.SendTrackerMessage(NextUserPresenceMessage.CreateBinaryMessage());
                    NextUserPresenceMessage = null;
                }


                await Task.Delay(20);
            }
            Dispose();
        }

        public void Dispose()
        {
            if (host != null)
            {
                try
                {
                    Console.WriteLine("Disabling connection to device...");
                    if (host != null)
                    {
                        
                        host.DisableConnection();
                        host.Dispose();
                    }

                }
                catch (Exception) { };
            }
        }
    }
}
