using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WCFChat
{
    [ServiceContract]
    public interface IMessageKeeper
    {
        [OperationContract]
        string GetNewMessages(int userid);

        [OperationContract]
        void SendMessage(int userid, string message);

        [OperationContract]
        int LogIn(string nickname);
    }
}
