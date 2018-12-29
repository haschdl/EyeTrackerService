using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
namespace EyeTracker.Service.Core
{

    /// <summary>
    /// See: https://msdn.microsoft.com/en-us/library/system.servicemodel.servicecontractattribute.sessionmode(v=vs.110).aspx
    /// </summary>
    [ServiceContract(Namespace = "http://Kinect.Toolbox.Service", CallbackContract = typeof(IEyeTrackerCallback), SessionMode = SessionMode.Required)]
    public interface IEyeTrackerService
    {
        /// <summary>
        /// This method is called when javascript instantiates a WebSocket, example: ws = new WebSocket(serverUrl);
        /// 
        /// A service contract can have only one service operation with the Action property set to "*". 
        /// https://msdn.microsoft.com/en-us/library/system.servicemodel.operationcontractattribute.action(v=vs.110).aspx
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [OperationContract(IsOneWay = true, Action = "*")]
        Task ClientConnectRequest(Message msg);
    }

    [ServiceContract]
    public interface IEyeTrackerCallback
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        Task SendTrackerMessage(Message msg);
    }
}
