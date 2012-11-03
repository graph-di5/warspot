using WarSpot.Cloud.Storage.Account;
using WarSpot.Contracts.Service;

namespace WarSpot.Cloud.UserService
{
	public class WarSpotMainUserService : IWarSpotService
	{
		public AccountDataSource DataBase;

		public bool loggedIn;


		public WarSpotMainUserService()
		{
			DataBase = new AccountDataSource();
			loggedIn = false;
		}

		public ErrorCode Register(string username, string pass)
		{
			return DataBase.AddAccountEntry(new AccountEntry(username, pass)) ?
				new ErrorCode(ErrorType.Ok) :
				new ErrorCode(ErrorType.UnknownException); ;
		}

		public ErrorCode Login(string inputUsername, string inputPass)
		{
			if (DataBase.CheckAccountEntry(inputUsername, inputPass))
			{
				loggedIn = true;
				return new ErrorCode(ErrorType.Ok);
			}
			else
				return new ErrorCode(ErrorType.WrongLoginOrPassword);
		}

		public ErrorCode UploadIntellect(byte[] intellect, string name)
		{
			throw new System.NotImplementedException();
		}

		public ErrorCode UploadIntellect(byte[] data)
		{
			// Проверка и что-то типа return isCorrect(intellect) ? new ErrorCode(ErrorType.ok) :
			// new ErrorCode(ErrorType.ForbiddenUsages)

			if (loggedIn)
			{
				DataBase.UploadDLL(data);
				return new ErrorCode(ErrorType.Ok);
			}
			else
				return new ErrorCode(ErrorType.ForbiddenUsages);
		}
		public string[] GetListOfIntellects()
		{
			// TODO: получать список всех интеллектов пользователя и возвращать их массивом

			// Заглушка
			string[] intellects = new string[20];
			return intellects;
		}

		public ErrorCode DeleteIntellect(string name)
		{
			// TODO: корректная работа с удалением
			return new ErrorCode(ErrorType.Ok);
		}
	}
}
