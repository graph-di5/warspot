
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Storage;
using System.Data;

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
            loggedIn = false;            
        }

        public ErrorCode Register(string username, string pass)
        {
            var test = (from b in db.Account
                        where b.Account_Name = username
                        select b).ToList<Account>();
            if (test != null)
                return new ErrorCode(ErrorType.WrongLoginOrPassword);
            else
            {
                db.AddToAccount(Account.CreateAccount(new System.Guid(), username, pass));
                return new ErrorCode(ErrorType.Ok);
            }
        }

        public ErrorCode Login(string username, string pass)
        {
            var test = (from b in db.Account
                        where b.Account_Name = username
                        select b).ToList<Account>();

            var res = test.Where(b => b.Account_Password == pass).FirstOrDefault<Account>();

            if (res == null)
            {
                return new ErrorCode(ErrorType.WrongLoginOrPassword);
            }
            else
            {
                loggedIn = true;
                user_ID = (from b in db.Account
                           where b.Account_Name = username
                           select b).Account_ID;

                return new ErrorCode(ErrorType.Ok);
            }

        }

        public ErrorCode UploadIntellect(byte[] intellect, string name)
        {
            if (loggedIn)
            {
                blob.Upload(user_ID, name, intellect);
                return new ErrorCode(ErrorType.Ok);
            }
            else
                return new ErrorCode(ErrorType.WrongLoginOrPassword);
        }

        
        public string[] GetListOfIntellects()
        {
            // TODO: получать список всех интеллектов пользователя и возвращать их массивом

            // Заглушка
            string[] result = new string[20];
            int i = 0;

            var test = (from b in db.Intellect
                        where b.AccountAccount_ID = user_ID
                        select b).ToList<Intellect>;

            foreach (Intellect j in test)
            {
                result[i] = j.Intellect_Name;
                i++;
            }

            return result;
        }

        public ErrorCode DeleteIntellect(string name)
        {
            // TODO: корректная работа с удалением

            var test = (from b in db.Intellect
                        where b.AccountAccount_ID = user_ID
                        select b).ToList<Intellect>;
            var result = test.Where(b => b.Intellect_Name == name);

            if (result)
            {
               // db.DeleteFromIntellect(result);
                return new ErrorCode(ErrorType.Ok);
            }
            else
            {
                return new ErrorCode(ErrorType.BadFileType);
            }

        }
    }
}
