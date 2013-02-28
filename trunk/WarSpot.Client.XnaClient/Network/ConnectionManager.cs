using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using WarSpot.Client.XnaClient.Screen;
using WarSpot.Common.Utils;
using WarSpot.Contracts.Service;
using System.IO;

namespace WarSpot.Client.XnaClient.Network
{
	class ConnectionManager
	{
		private static ConnectionManager _localInstance;
		public static ConnectionManager Instance
		{
			get { return _localInstance; }
		}

		static ConnectionManager()
		{
			_localInstance = new ConnectionManager();
		}

		private ConnectionManager()
		{
			// Nothing to do here
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
				return new ErrorCode(ErrorType.UnknownException, e.ToString());
			}
		}

		public ErrorCode Login(string username, string password)
		{
			InitializeConnection();
			try
			{
				return _service.Login(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return new ErrorCode(ErrorType.UnknownException, e.ToString());
			}
		}

		public ErrorCode UploadIntellect(AIManager.Intellect intellect)
		{
			InitializeConnection();
			return _service.UploadIntellect(intellect.ByteDll, intellect.Name);
		}

		public string[] GetListOfIntellects()
		{
			InitializeConnection();
			return _service.GetListOfIntellects().Select(re => re.Value).ToArray();
		}

		public ErrorCode DeleteIntellect(string name)
		{
			InitializeConnection();
			return _service.DeleteIntellect(name);
		}

		public void KeepAlive()
		{
			InitializeConnection();
			try
			{
				if(_service != null)
				{
					_service.KeepAlive();
				}
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
			}
		}
	}
}
