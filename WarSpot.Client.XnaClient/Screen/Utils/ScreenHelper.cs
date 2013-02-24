using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.MatchComputer;

namespace WarSpot.Client.XnaClient.Screen.Utils
{
	// Helper for passing arguments through screens without direct methods
	class ScreenHelper
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
		private static ScreenHelper _instance;
		public static ScreenHelper Instance
		{
			get { return _instance; }
		}

		static ScreenHelper()
		{
			_instance = new ScreenHelper();
		}

		private ScreenHelper()
		{
		}
	}
}
