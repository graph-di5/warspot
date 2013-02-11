﻿using System;
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

	}
}
