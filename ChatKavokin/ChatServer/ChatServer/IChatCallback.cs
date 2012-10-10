using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Forms;

namespace ChatServer
{
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receive(string name, string msg);

        [OperationContract(IsOneWay = true)]
        void UserEnter(string name);

        [OperationContract(IsOneWay = true)]
        void UserLeave(string name);
    }
}
