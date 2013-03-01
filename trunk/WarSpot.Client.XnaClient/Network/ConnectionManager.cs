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
			if (_service == null)
			{
				var channelFactory = new ChannelFactory<IWarSpotService>("WarSpotEndpoint");
				_service = channelFactory.CreateChannel();
			}
		}

		public ErrorCode Register(string username, string password)
		{
			try
			{
				InitializeConnection();
				return _service.Register(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return new ErrorCode(ErrorType.UnknownException, e.Message);
			}
		}

		public ErrorCode Login(string username, string password)
		{
			try
			{
				InitializeConnection();
				return _service.Login(username, HashHelper.GetMd5Hash(password));
			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return new ErrorCode(ErrorType.UnknownException, e.Message);
			}
		}

		public ErrorCode UploadIntellect(AIManager.Intellect intellect)
		{
			try
			{
				InitializeConnection();
				return _service.UploadIntellect(intellect.ByteDll, intellect.Name);
			}
			catch (Exception e)
			{
				return new ErrorCode(ErrorType.UnknownException, e.Message);
			}
		}

		public ErrorCode GetListOfIntellects()
		{
			try
			{
				InitializeConnection();
				Screen.Utils.ScreenHelper.Instance.AvailableIntellects = _service.GetListOfIntellects();
				return new ErrorCode();
			}
			catch (Exception e)
			{
				return new ErrorCode(ErrorType.UnknownException, e.Message);
			}
		}

		public ErrorCode DeleteIntellect(string name)
		{
			try
			{
				InitializeConnection();
				return _service.DeleteIntellect(name);
			}
			catch (Exception e)
			{
				return new ErrorCode(ErrorType.UnknownException, e.Message);
			}
		}

		public ErrorCode BeginMatch(List<Guid> intellects, string title)
		{
			try
			{
				InitializeConnection();
				_service.BeginMatch(intellects, title);
				return new ErrorCode();
			}
			catch (Exception e)
			{
				return new ErrorCode(ErrorType.UnknownException, e.Message);
			}
		}

		public void KeepAlive()
		{
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
