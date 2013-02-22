using System;
using System.ServiceModel;
using System.Collections;
using System.Collections.Generic;

namespace WarSpot.Contracts.Service
{
	[ServiceContract]
	public interface IWarSpotService
	{
		[OperationContract(IsInitiating = true)]
		ErrorCode Register(string username, string pass);

		[OperationContract(IsInitiating = true)]
		ErrorCode Login(string inputUsername, string inputPass);

		[OperationContract]
		ErrorCode UploadIntellect(byte[] intellect, string name);

		[OperationContract]
		string[] GetListOfIntellects();

		[OperationContract]
		ErrorCode DeleteIntellect(string name);

		[OperationContract]
		Replay DownloadReplay(Guid game);

		[OperationContract]
		Guid? BeginMatch(List<Guid> intellects);

		[OperationContract]
		List<Guid> GetListOfGames();

		[OperationContract]
		void KeepAlive();

        [OperationContract]
        bool IsUserAdmin(Guid user);

        [OperationContract]
        bool IsUser(string role, Guid user);

        [OperationContract]
        ErrorCode CreateTournament(string title, string startdate, Int64 maxplayers);

        [OperationContract]
        ErrorCode SetUserRole(Guid user, string role, string until);

        [OperationContract]
        List<Guid> GetUserRole(Guid user);

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

	}
}
