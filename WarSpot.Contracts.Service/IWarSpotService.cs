using System;
using System.ServiceModel;
using System.Collections.Generic;

namespace WarSpot.Contracts.Service
{
	[ServiceContract]
	public interface IWarSpotService
	{
		#region Login\Registration
		[OperationContract(IsInitiating = true)]
		ErrorCode Register(string accountname, string    pass, string username, string usersurname, string institution, int course, string email);

		[OperationContract(IsInitiating = true)]
		ErrorCode Login(string inputUsername, string inputPass);

        [OperationContract(IsInitiating = true)]
        ErrorCode ChangePassword(string oldpassword, string newpassword);
		#endregion

		#region Intelletcs
		[OperationContract]
		ErrorCode UploadIntellect(byte[] intellect, string name, string description);

		[OperationContract]
		byte[] DownloadIntellect(Guid intellectID);

		[OperationContract]
		List<KeyValuePair<Guid, string>> GetListOfIntellects();

		[OperationContract]
		ErrorCode DeleteIntellect(string name);
		#endregion

		#region Replay
		[OperationContract]
		Replay DownloadReplay(Guid game);

		// WTF!
#if false
		[OperationContract]
		ErrorCode UploadReplay(Guid game, byte[] replay);
#endif

		[OperationContract]
		List<ReplayDescription> GetListOfReplays();
		#endregion

		#region Match
		[OperationContract]
		Guid? BeginMatch(List<Guid> intellects, string title);

		[OperationContract]
		List<Guid> GetListOfGames();
		#endregion

		#region Role
		[OperationContract]
		bool IsUserAdmin(Guid user);

		[OperationContract]
		bool IsUser(Guid user, string role);

		[OperationContract]
		ErrorCode SetUserRole(Guid user, string role, DateTime until);

		[OperationContract]
		string[] GetUserRoles(Guid user);
		#endregion

		#region Tournament
		[OperationContract]
		ErrorCode CreateTournament(string title, DateTime startDate, long maxPlayers);

		[OperationContract]
		List<Guid> GetMyTournamets();

		[OperationContract]
		ErrorCode DeleteTournament(Guid tournamentID);

		[OperationContract]
		List<Guid> GetAvailableTournaments();
		
		[OperationContract]		
		List<Guid> GetActiveTournaments();
		
		[OperationContract]
		ErrorCode UpdateTournament(Guid tournamentID, string newState);

		[OperationContract]
		ErrorCode JoinTournament(Guid tournamentID);

		[OperationContract]
		ErrorCode LeaveTournament(Guid tournamentID);			

        #region Stage
        [OperationContract]
        ErrorCode AddStage(Guid tournamentID, DateTime startTime);

        [OperationContract]
        ErrorCode DeleteStage(Guid stageID);

        [OperationContract]
        ErrorCode UpdateStage(Guid stageID, string newState);
		
		[OperationContract]
		List<Guid> GetStageGames(Guid stageID);

        #endregion

        #endregion

        [OperationContract]
		void KeepAlive();
	}
}
