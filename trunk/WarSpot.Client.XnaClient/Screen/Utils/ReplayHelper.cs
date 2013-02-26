using System;
using System.Collections.Generic;
using WarSpot.MatchComputer;

namespace WarSpot.Client.XnaClient.Screen.Utils
{
	// Helper for passing arguments through screens without direct methods
	class ReplayHelper
	{
		private string _replayPath;
		public string ReplayPath
		{
			get
			{
				return _replayPath;
			}
			set
			{
				_replayPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), value);
			}
		}
		public List<WarSpotEvent> replayEvents;
		public Version CurrReplayVersion { get; set; }
		private static ReplayHelper _instance;
		public static ReplayHelper Instance
		{
			get { return _instance; }
		}

		static ReplayHelper()
		{
			_instance = new ReplayHelper();
		}

		private ReplayHelper()
		{
		}
	}
}
