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

        [OperationContract]
        ErrorCode UploadIntellect(byte[] intellect, string name);

        [OperationContract]
        string[] GetListOfIntellects();

        [OperationContract]
        ErrorCode DeleteIntellect(string name);

		[OperationContract]
		Replay SendReplay(string name);

    }
}
