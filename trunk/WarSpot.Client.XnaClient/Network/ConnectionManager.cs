using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Contracts.Service;
using System.ServiceModel;

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
				
			}
		}
	}
}
