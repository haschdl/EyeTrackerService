using System;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;

namespace EyeTracker.Service.Core.Model
{
    [ComVisible(true)]
    public abstract class MessageBase : IDisposable
    {
        public byte[] DataArray;

        public MessageType Type;

        public MessageBase(MessageType type)
        {
            Type = type;
        }

        public void Dispose()
        {
            
        }


        public abstract Message CreateBinaryMessage();
    }
}