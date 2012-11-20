using System;
using System.Diagnostics;
using System.ServiceModel;
using WarSpot.Client.XnaClient.Utils;
using WarSpot.Contracts.Service;
using System.IO;

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
		#region login and registration

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
		#endregion login and registration

		#region DLLUploadControl

		public ErrorCode UploadIntellect(AIManager.Intellect intellect)
		{
			return _service.UploadIntellect(intellect.ByteDll, intellect.Name);
		}

		public string[] GetListOfIntellects()
		{
			return _service == null ? new string[0] : _service.GetListOfIntellects();
		}

		public ErrorCode DeleteIntellect(string name)
		{
			return _service.DeleteIntellect(name);
		}

		#endregion DLLUploadControl

		#region recieving replays

		public ErrorCode RecieveReplay(string name)
		{
			InitializeConnection();
			try
			{
				// TODO: refactor this
				Replay tmp;
				tmp = _service.SendReplay(name);
				string absolutePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string path = Path.Combine(absolutePath, tmp.name + ".log");
				File.WriteAllBytes(path, tmp.data);

			}
			catch (Exception e)
			{
				Trace.WriteLine(e);
				return new ErrorCode(ErrorType.UnknownException, e.ToString());
			}
			return new ErrorCode();
		}

		#endregion
	}
}
