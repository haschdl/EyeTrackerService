using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using EyeTracker.Service.Core;

namespace EyeTracker.Service.Host
{
    class Program
    {
        //The URL to be reserved for the service
        //Reserving a URL in Windows requires Administrative permissions,
        //Therefore the app.manifest is marked with <requestedExecutionLevel  level="requireAdministrator" uiAccess="false" />        
        private const string BaseAddress = @"http://127.0.0.1:8080/eyetrackerservice";
        private static Uri _serviceAddressGlaze = new Uri(BaseAddress + "/glaze");
        internal static ServiceHost serviceHostGlazeService = null;


        static void Main(string[] args)
        {
            var version = typeof(EyeTracker.Service.Core.Service).Assembly.GetName().Version;
            Console.WriteLine("Initiating host for Eye Tracker...", EventLogEntryType.Information);
            Console.WriteLine($"Assembly version: {version.ToString()}", EventLogEntryType.Information);
            InitService(serviceHostGlazeService, typeof(EyeTracker.Service.Core.Service), _serviceAddressGlaze);
            Console.WriteLine($"The service {typeof(EyeTracker.Service.Core.Service).Name} is ready at {_serviceAddressGlaze}");

            Console.WriteLine("Press <Enter> to stop the service.");
            Console.ReadLine();
        }

        private static void InitService(ServiceHost serviceHost, Type serviceImplementationType, Uri serviceAddress)
        {
            if (serviceHost != null)
            {
                //this.EventLog.WriteEntry("Host wast not null at service start-up. Closing host.", EventLogEntryType.Information);
                serviceHost.Close();
            }

            // Create the ServiceHost
            serviceHost = new ServiceHost(serviceImplementationType, serviceAddress);


            ServiceMetadataBehavior smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = false
            };
            serviceHost.Description.Behaviors.Add(smb);

            CustomBinding binding = new CustomBinding();
            binding.Elements.Add(new ByteStreamMessageEncodingBindingElement());

            binding.ReceiveTimeout = TimeSpan.FromHours(5);
            binding.SendTimeout = TimeSpan.FromHours(5);

            HttpTransportBindingElement transport = new HttpTransportBindingElement
            {

                WebSocketSettings =
                {
                    TransportUsage = WebSocketTransportUsage.Always,
                    CreateNotificationOnConnection = true,


                }
            };

            binding.Elements.Add(transport);

            serviceHost.AddServiceEndpoint(typeof(IEyeTrackerService), binding, "");
            serviceHost.Closed += ServiceHostOnClosed;
            serviceHost.Faulted += ServiceHost_Faulted;

            serviceHost.Open();
        }

        private static void ServiceHost_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Service host faulted!" + Environment.NewLine + e);
        }

        private static void ServiceHostOnClosed(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Host was closed.");
        }

    }
}
