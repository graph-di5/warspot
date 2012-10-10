using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Forms;

namespace ChatServer
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatCallback))]
    public interface IChat
    {
        [OperationContract(IsInitiating = true, IsOneWay = false, IsTerminating = false)]
        String[] Join(string name);

        [OperationContract(IsInitiating = false, IsOneWay = true, IsTerminating = false)]
        void Send(string msg);

        [OperationContract(IsInitiating = false, IsOneWay = true, IsTerminating = true)]
        void Leave();
    }

    
}
