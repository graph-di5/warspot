using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServerClient
{   
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChatController
    {
        [OperationContract]
        string[] ConnectToChat(string name);

        [OperationContract]
        void SendMessage(string message);

        [OperationContract]
        void LeaveChat();
    }

    public interface IChatCallback
    {

        [OperationContract]
        void ReceiveMsg(string nickname, string message);

        [OperationContract]
        void UserLeaveChat(string nickname);

        [OperationContract]
        void UserConnectChat(string nickname);
    }
}
