using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
namespace WarSpot.Contracts.Service
{
    [ServiceContract]
    public interface IWarSpotService
    {
        [OperationContract]
        bool Register(string username, string pass);

        [OperationContract]
        Guid? Login(string inputUsername, string inputPass);

    }
}
