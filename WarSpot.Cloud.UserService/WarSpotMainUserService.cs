﻿using System.Linq;
using WarSpot.Contracts.Service;

namespace WarSpot.Cloud.UserService
{
	public class WarSpotMainUserService : IWarSpotService
	{
		private bool _loggedIn;
		private System.Guid _userID;

		private readonly Storage.Storage _storage;

		public WarSpotMainUserService()
		{
			_storage = new Storage.Storage();
			_loggedIn = false;
		}

		public ErrorCode Register(string username, string pass)
		{
			if (_storage.Register(username, pass))
				return new ErrorCode(ErrorType.Ok, "New account has been successfully created.");
			else
				return new ErrorCode(ErrorType.WrongLoginOrPassword, "Existed account with same name.");
		}

		public ErrorCode Login(string username, string pass)
		{
			if (_storage.Login(username, pass))
			{
				_loggedIn = true;
				_userID = (from b in _storage.db.Account
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
			if (_loggedIn)
			{
				_storage.Upload(_userID, name, intellect);
				return new ErrorCode(ErrorType.Ok, "Intellect has been successfully uploaded.");
			}
			else
				return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
		}


		public string[] GetListOfIntellects()
		{
			// TODO: получать список всех интеллектов пользователя и возвращать их массивом

			if (!_loggedIn)
			{
				return (new string[1] { "Not logged in yet." });
			}

			return _storage.GetListOfIntellects(_userID);
		}

		public ErrorCode DeleteIntellect(string name)
		{
			// TODO: корректная работа с удалением

			if (!_loggedIn)
			{
				return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
			}

			if (_storage.DeleteIntellect(name, _userID))
			{
				return new ErrorCode(ErrorType.Ok, "Intellect has been deleted.");
			}
			else
				return new ErrorCode(ErrorType.BadFileType, "No intellect with that name");

		}
	}
}
