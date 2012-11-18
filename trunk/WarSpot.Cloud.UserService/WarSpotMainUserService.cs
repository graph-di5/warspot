
using System.Collections;
using System.Linq;
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Storage;


namespace WarSpot.Cloud.UserService
{
    public class WarSpotMainUserService : IWarSpotService
    {
        

        public bool loggedIn;        
        public System.Guid user_ID;

        public Storage.Storage storage;

        public WarSpotMainUserService()
        {
            storage = new Storage.Storage();
            loggedIn = false;
        }

        public ErrorCode Register(string username, string pass)
        {
            if (storage.Register(username, pass))
                return new ErrorCode(ErrorType.Ok, "New account has been successfully created.");
            else
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Existed account with same name.");
        }

        public ErrorCode Login(string username, string pass)
        {
			if (storage.Login(username, pass))
			{
				loggedIn = true;
				user_ID = (from b in storage.db.Account
									where b.Account_Name == username
									select b).First().Account_ID;

				return new ErrorCode(ErrorType.Ok, "Logged in successfully.");
			}
			else
			{
				return new ErrorCode(ErrorType.WrongLoginOrPassword, "Wrong username or password.");
			}

        }

        public ErrorCode UploadIntellect(byte[] intellect, string name)
        {
            if (loggedIn)
            {
                storage.Upload(user_ID, name, intellect);
                return new ErrorCode(ErrorType.Ok, "Intellect has been successfully uploaded.");
            }
            else
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
        }

        
        public string[] GetListOfIntellects()
        {
            // TODO: получать список всех интеллектов пользователя и возвращать их массивом

            if (!loggedIn)
            {
                return (new string[1] {"Not logged in yet."});
            }

            return storage.GetListOfIntellects(user_ID);
        }

        public ErrorCode DeleteIntellect(string name)
        {
            // TODO: корректная работа с удалением

            if (!loggedIn)
            {
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
            }

            if (storage.DeleteIntellect(name, user_ID))
            {
                return new ErrorCode(ErrorType.Ok, "Intellect has been deleted.");
            }
            else
                return new ErrorCode(ErrorType.BadFileType, "No intellect with that name");

        }
    }
}
