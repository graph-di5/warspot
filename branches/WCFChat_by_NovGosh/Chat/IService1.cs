using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Chat
{
    [ServiceContract]
    public interface IChatService
    {
        [OperationContract]
        bool Register (string login);
    
        [OperationContract]
        void SendMessage (Message mess);

        [OperationContract]
        Message GetNewMessages (int last);
    }
}
