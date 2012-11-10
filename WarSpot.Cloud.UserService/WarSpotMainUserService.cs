
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Storage;
using System.Data;
using System.Collections;
using System.Linq;

namespace WarSpot.Cloud.UserService
{
    public class WarSpotMainUserService : IWarSpotService
    {
        public DBContext db;      

        public bool loggedIn;

        
        public System.Guid user_ID;

        public Storage.Storage blob;

        public WarSpotMainUserService()
        {
            db = new DBContext();
            blob = new Storage.Storage();
            loggedIn = false;
        }

        public ErrorCode Register(string username, string pass)
        {
            var test = (from b in db.Account
                        where b.Account_Name == username
                        select b).ToList<Account>();

            if (test != null)
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Existed account with same name.");
            else
            {                
                db.AddToAccount(Account.CreateAccount(new System.Guid(), username, pass));
                return new ErrorCode(ErrorType.Ok, "New account has been successfully created.");
            }
        }

        public ErrorCode Login(string username, string pass)
        {
            var test = (from b in db.Account
                        where b.Account_Name == username
                        select b).FirstOrDefault();

            if (test.Account_Password == pass)
            {           

                loggedIn = true;
                user_ID = (from b in db.Account
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
                blob.Upload(user_ID, name, intellect);
                return new ErrorCode(ErrorType.Ok, "Intellect has been successfully uploaded.");
            }
            else
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
        }

        
        public string[] GetListOfIntellects()
        {
            // TODO: получать список всех интеллектов пользователя и возвращать их массивом

            // Заглушка
            if (!loggedIn)
            {
                return (new string[1] {"Not logged in yet."});
            }

            ArrayList result = new ArrayList();

            var test = (from b in db.Intellect
                        where b.AccountAccount_ID == user_ID
                        select b).ToList<Intellect>();

            foreach (Intellect i in test)
            {
                result.Add(i.Intellect_Name);
            }

            return (string[]) result.ToArray();
        }

        public ErrorCode DeleteIntellect(string name)
        {
            // TODO: корректная работа с удалением

            if (!loggedIn)
            {
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
            }

            var test = (from b in db.Intellect
                        where b.AccountAccount_ID == user_ID
                        select b).ToList<Intellect>();

            var result = test.Where(b => b.Intellect_Name == name);

            if (result != null)
            {
               // db.DeleteFromIntellect(result);
                return new ErrorCode(ErrorType.Ok, "Inttellect has been deleted.");
            }
            else
            {
                return new ErrorCode(ErrorType.BadFileType, "No intellect with that name.");
            }

        }
    }
}
