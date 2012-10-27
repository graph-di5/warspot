using System;
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Storage.Account;


namespace WarSpot.Cloud.UserService
{
    public class WarSpotMainUserService : IWarSpotService
    {

        public AccountDataSource DataBase;


        public ErrorCode Register(string username, string pass)
		{
			return DataBase.AddAccountEntry(new AccountEntry(username, pass)) ?
				new ErrorCode(ErrorType.Ok) :
				new ErrorCode(ErrorType.UnknownException); ;
        }

        public ErrorCode Login(string inputUsername, string inputPass)
        {
            return DataBase.CheckAccountEntry(inputUsername, inputPass) ? 
				new ErrorCode(ErrorType.Ok) : 
				new ErrorCode(ErrorType.UnknownException);
        }

    }
}