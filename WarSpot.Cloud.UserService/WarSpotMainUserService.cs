using System;
using System.Collections.Generic;
using System.Linq;
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Storage;

namespace WarSpot.Cloud.UserService
{

   
	public class WarSpotMainUserService : IWarSpotService
	{
		private bool _loggedIn;
		private Guid _userID;       
		

		public WarSpotMainUserService()
		{
			_loggedIn = false;
		}

        #region match's stuff
        public Guid? BeginMatch(List<Guid> intellects)
		{
			if (!_loggedIn)
			{
				return null;
			}
            
			return Warehouse.BeginMatch(intellects, _userID);

		}

		public List<Guid> GetListOfGames()
		{
			if (!_loggedIn)
			{
				return null;
			}

			return Warehouse.GetListOfGames(_userID);
		}
        #endregion

        #region login and registration
        public ErrorCode Register(string username, string pass)
		{
			if (Warehouse.Register(username, pass))
				return new ErrorCode(ErrorType.Ok, "New account has been successfully created.");
			else
				return new ErrorCode(ErrorType.WrongLoginOrPassword, "Existed account with same name.");
		}

		public ErrorCode Login(string username, string pass)
		{
			if (Warehouse.Login(username, pass))
			{
				_loggedIn = true;
				_userID = (from b in Warehouse.db.Account
									 where b.Account_Name == username
									 select b).First().Account_ID;

				return new ErrorCode(ErrorType.Ok, "Logged in successfully.");
			}
			else
			{
				return new ErrorCode(ErrorType.WrongLoginOrPassword, "Wrong username or password.");
			}

		}
		#endregion login and registration

		#region intellect's stuff
		public ErrorCode UploadIntellect(byte[] intellect, string name)
		{
			if (_loggedIn)
			{
				Warehouse.UploadIntellect(_userID, name, intellect);
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

			return Warehouse.GetListOfIntellects(_userID);
		}

		public ErrorCode DeleteIntellect(string name)
		{
			// TODO: корректная работа с удалением

			if (!_loggedIn)
			{
				return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
			}

            if (Warehouse.DeleteIntellect(name, _userID))
			{
				return new ErrorCode(ErrorType.Ok, "Intellect has been deleted.");
			}
			else
				return new ErrorCode(ErrorType.BadFileType, "No intellect with that name");

		}

		#endregion intellect's stuff

		#region replay's stuff
		public Replay DownloadReplay(Guid gameID)
		{
			if (!_loggedIn)
			{
				return null;
			}

            return Warehouse.GetReplay(gameID);

		}

		#endregion replay's stuff

        #region tournament stuff
        public ErrorCode CreateTournament(string title, string startdate, Int64 maxplayers)
        {
            if (!_loggedIn)
            {
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
            }

            return Warehouse.CreateTournament(title, startdate, maxplayers, this._userID);            
        }


        public List<Guid> GetMyTournamets()
        {
            if (!_loggedIn)
            {
                return null;
            }

            return Warehouse.GetMyTournaments(this._userID);
        }


        public ErrorCode DeleteTournament(Guid tournamentID)
        {
            if (!_loggedIn)
            {
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
            }

            return Warehouse.DeleteTournament(tournamentID, this._userID);

        }


        public List<Guid> GetAvailableTournaments()
        {
            if (!_loggedIn)
            {
                return null;
            }

            return Warehouse.GetAvailableTournaments(this._userID);

        }


        public ErrorCode JoinTournament(Guid tournamentID)
        {
            if (!_loggedIn)
            {
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
            }

            return Warehouse.JoinTournament(tournamentID, this._userID);
        }


        public ErrorCode LeaveTournament(Guid tournamentID)
        {
            if (!_loggedIn)
            {
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
            }

            return Warehouse.LeaveTournament(tournamentID, this._userID);

        }
        #endregion

        #region role's stuff


        public bool IsUserAdmin(Guid userID)
        {
            if (!_loggedIn)
            {
                return false;
            }

            return Warehouse.IsUserAdmin(userID);
        }

        public bool IsUser(string role, Guid user)
        {
            if (!_loggedIn)
            {
                return false;
            }

            return Warehouse.IsUser(Role.GetRoleCode(role), user);
        }

        public ErrorCode SetUserRole(Guid userID, string role, string until)
        {
            if (!_loggedIn)
            {
                return new ErrorCode(ErrorType.WrongLoginOrPassword, "Not logged in yet.");
            }

            return Warehouse.SetUserRole(Role.GetRoleCode(role), userID, until);
        }


        public List<Guid> GetUserRole(Guid user)
        {
            if (!_loggedIn)
            {
                return null;
            }

            return Warehouse.GetUserRole(user);
        }


        #endregion 

        public void KeepAlive()
		{
		}
	}
}
