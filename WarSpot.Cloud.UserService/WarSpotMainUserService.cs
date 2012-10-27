using System;
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Storage.Account;


namespace WarSpot.Cloud.UserService
{
    public class WarSpotMainUserService : IWarSpotService
    {

        public AccountDataSource DataBase;


        public bool Register(string username, string pass)
        {
            return DataBase.AddAccountEntry(new AccountEntry(username, pass));
        }

        public bool Login(string inputUsername, string inputPass)
        {
            return DataBase.CheckAccountEntry(inputUsername, inputPass);
        }

    }
}