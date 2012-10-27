using System;
using System.ServiceModel;

namespace WarSpot.Contracts.Service
{
    [ServiceContract]
    public interface IWarSpotService
    {
        [OperationContract]
        ErrorCode Register(string username, string pass);

        [OperationContract]
        ErrorCode Login(string inputUsername, string inputPass);

    }
}
