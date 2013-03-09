using System;
using System.ServiceModel;
using System.Collections;
using System.Collections.Generic;

namespace WarSpot.Contracts.Service
{
	[ServiceContract]
	public interface IWarSpotService
	{
		#region Login\Registration
		[OperationContract(IsInitiating = true)]
		ErrorCode Register(string username, string pass);

		[OperationContract(IsInitiating = true)]
		ErrorCode Login(string inputUsername, string inputPass);

        [OperationContract(IsInitiating = true)]
        ErrorCode ChangePassword(string oldpassword, string newpassword);
		#endregion

		#region Intelletcs
		[OperationContract]
		ErrorCode UploadIntellect(byte[] intellect, string name);

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

		[OperationContract]
		ErrorCode UploadReplay(Guid game, byte[] replay);

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
		bool IsUser(string role, Guid user);

		[OperationContract]
		ErrorCode SetUserRole(Guid user, string role, string until);

		[OperationContract]
		string[] GetUserRoles(Guid user);
		#endregion

		#region Tournament
		[OperationContract]
		ErrorCode CreateTournament(string title, string startdate, Int64 maxplayers);

		[OperationContract]
		List<Guid> GetMyTournamets();

		[OperationContract]
		ErrorCode DeleteTournament(Guid tournamentID);

		[OperationContract]
		List<Guid> GetAvailableTournaments();

		[OperationContract]
		ErrorCode JoinTournament(Guid tournamentID);

		[OperationContract]
		ErrorCode LeaveTournament(Guid tournamentID);
		#endregion

		[OperationContract]
		void KeepAlive();

	}
}
