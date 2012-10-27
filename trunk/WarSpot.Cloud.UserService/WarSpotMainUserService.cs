using WarSpot.Cloud.Storage.Account;
using WarSpot.Contracts.Service;

namespace WarSpot.Cloud.UserService
{
	public class WarSpotMainUserService : IWarSpotService
	{
		public AccountDataSource DataBase;
		public WarSpotMainUserService()
		{
			DataBase = new AccountDataSource();
		}

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
				new ErrorCode(ErrorType.WrongLoginOrPassword);
		}

		public ErrorCode UploadIntellect(byte[] data, string name)
		{
			// Проверка и что-то типа return isCorrect(intellect) ? new ErrorCode(ErrorType.ok) :
			// new ErrorCode(ErrorType.ForbiddenUsages)
			return new ErrorCode(ErrorType.Ok);
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
