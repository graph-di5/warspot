using System;
using System.Collections.Generic;
using System.Linq;
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Storage;
using WarSpot.Security;

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
		public Guid? BeginMatch(List<Guid> intellects, string title)
		{
			if (!_loggedIn)
			{
				return null;
			}

			return Warehouse.BeginMatch(intellects, _userID, title);

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
        public ErrorCode Register(string accountname, string pass, string username, string usersurname, string institution, int course, string email)
		{
            if (Warehouse.Register(accountname, pass, username, usersurname, institution, course, email))
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

		public ErrorCode ChangePassword(string oldpassword, string newpassword)
		{
			if (!_loggedIn)
			{
				return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
			}
			else
			{
				if (Warehouse.ChangePassword((from a in Warehouse.db.Account
																			where a.Account_ID == _userID
																			select a.Account_Name).FirstOrDefault<string>(), oldpassword, newpassword))
				{
					return new ErrorCode(ErrorType.Ok, "Password has been changed.");
				}
				else
				{
					return new ErrorCode(ErrorType.UnknownException, "Current password is incorrect or database problems.");
				}

			}
		}
		#endregion login and registration

		#region intellect's stuff
		public ErrorCode UploadIntellect(byte[] intellect, string name, string description)
		{
			if (_loggedIn)
			{
				
				if (Warehouse.UploadIntellect(_userID, name, intellect, description))
					return new ErrorCode(ErrorType.Ok, "Intellect has been successfully uploaded.");
				else
					return new ErrorCode(ErrorType.BadFileName, "Inttellect with the same name is already existed");
			}
			else
				return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
		}

		public byte[] DownloadIntellect(Guid intellectID)
		{
			if (!_loggedIn)
			{
				return null;
			}
			else
				return Warehouse.DownloadIntellect(intellectID);
		}

		public List<KeyValuePair<Guid, string>> GetListOfIntellects()
		{
			if (!_loggedIn)
			{
				return null;
			}

			return Warehouse.GetListOfIntellects(_userID);
		}

		public ErrorCode DeleteIntellect(string name)
		{
			if (!_loggedIn)
			{
				return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
			}

			if (Warehouse.DeleteIntellect(name, _userID))
			{
				return new ErrorCode(ErrorType.Ok, "Intellect has been deleted.");
			}
			else
				return new ErrorCode(ErrorType.BadFileName, "No intellect with that name");

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

		// WTF
#if false
        public ErrorCode UploadReplay(Guid gameID, byte[] replay)
        {
            if (!_loggedIn)
            {
                return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
            }
            
            return Warehouse.UploadReplay(replay, gameID);
        }
#endif

		public List<ReplayDescription> GetListOfReplays()
		{
			if (!_loggedIn)
			{
				return null;
			}

			return Warehouse.GetListOfReplays(_userID);
		}
		#endregion replay's stuff

		#region tournament stuff
		public ErrorCode CreateTournament(string title, DateTime startDate, long maxPlayers)
		{
			if (!_loggedIn)
			{
				return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
			}

			return Warehouse.CreateTournament(title, startDate, maxPlayers, this._userID);
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
				return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
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

		public List<Guid> GetActiveTournaments()
		{
			return Warehouse.GetActiveTournaments();
		}
		
		public ErrorCode UpdateTournament(Guid tournamentID, string newState)
		{
			try
            {
                Warehouse.UpdateTournament(tournamentID, (State) Enum.Parse(typeof(State), newState));
                return new ErrorCode(ErrorType.Ok, "Stage has been updated.");
            }
            catch (Exception e)
            {
                return new ErrorCode(ErrorType.UnknownException, "Warehouse.UpdateTournament goes wrong. Exception" + e.ToString());
            }
		}

		public ErrorCode JoinTournament(Guid tournamentID)
		{
			if (!_loggedIn)
			{
				return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
			}

			return Warehouse.JoinTournament(tournamentID, this._userID);
		}


		public ErrorCode LeaveTournament(Guid tournamentID)
		{
			if (!_loggedIn)
			{
				return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
			}

			return Warehouse.LeaveTournament(tournamentID, this._userID);

		}

        #region stage stuff

        public ErrorCode AddStage(Guid tournamentID, DateTime startTime)
        {
            if (!_loggedIn)
            {
                return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
            }
            return Warehouse.AddStage(tournamentID, startTime);
        }

        public ErrorCode DeleteStage(Guid stageID)
        {
            if (!_loggedIn)
            {
                return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
            }
            return Warehouse.DeleteStage(stageID);
        }

        public ErrorCode UpdateStage(Guid stageID, string newState)
        {
            try
            {
                Warehouse.UpdateStage(stageID, (State) Enum.Parse(typeof(State), newState));
                return new ErrorCode(ErrorType.Ok, "Stage has been updated.");
            }
            catch (Exception e)
            {
                return new ErrorCode(ErrorType.UnknownException, "Warehouse.UpdateStage goes wrong. Exception" + e.ToString());
            }
        }

		public List<Guid> GetStageGames(Guid stageID)
		{
			return Warehouse.GetStageGames(stageID);		
		}
		
        #endregion


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

		public bool IsUser(Guid user, string role)
		{
			if (!_loggedIn)
			{
				return false;
			}

			return Warehouse.IsUser(user, (RoleType)Enum.Parse(typeof(RoleType), role));
		}

		public ErrorCode SetUserRole(Guid userID, string role, DateTime until)
		{
			if (!_loggedIn)
			{
				return new ErrorCode(ErrorType.NotLoggedIn, "Not logged in yet.");
			}

			return Warehouse.SetUserRole((RoleType)Enum.Parse(typeof(RoleType), role), userID, until);
		}

		public string[] GetUserRoles(Guid user)
		{
			if (!_loggedIn)
			{
				return null;
			}

			return Warehouse.GetUserRoles(user);
		}

		#endregion

		public void KeepAlive()
		{
		}
	}
}
