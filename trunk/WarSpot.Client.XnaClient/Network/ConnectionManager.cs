using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Contracts.Service;
using System.ServiceModel;
using WarSpot.Client.XnaClient.Utils;
using System.Diagnostics;
using WarSpot.Client.XnaClient.Screen;

namespace WarSpot.Client.XnaClient.Network
{
	class ConnectionManager
	{
		private static ConnectionManager _localInstance;
		public static ConnectionManager Instance
		{
			get { return _localInstance ?? (_localInstance = new ConnectionManager()); }
		}

		private IWarSpotService _service;

		private void InitializeConnection()
		{
			try
			{
				var channelFactory = new ChannelFactory<IWarSpotService>("WarSpotEndpoint");
				_service = channelFactory.CreateChannel();
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
			}
		}

		public ErrorCode Register(string username, string password)
		{
			InitializeConnection();
			try
			{
				return _service.Register(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return new ErrorCode(ErrorType.UnknownException);
			}
		}

		public ErrorCode Login(string username, string password)
		{
			try
			{
				return _service.Login(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return new ErrorCode(ErrorType.UnknownException);
			}
		}
	}
}
