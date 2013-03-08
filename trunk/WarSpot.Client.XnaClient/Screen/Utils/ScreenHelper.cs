using System;
using System.Collections.Generic;
using WarSpot.Contracts.Service;

namespace WarSpot.Client.XnaClient.Screen.Utils
{
	// Helper for passing arguments through screens without direct methods and for storage screens's data for other screens 
	class ScreenHelper
	{
		public List<KeyValuePair<Guid, string>> AvailableIntellects { get; set; }
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
		public List<WarSpotEvent> ReplayEvents { get; set;}
		public List<Guid> ListOfGames { get; set; }
		public bool IsOnline { get; set; }
		private static ScreenHelper _instance;
		public static ScreenHelper Instance
		{
			get { return _instance; }
		}

		static ScreenHelper()
		{
			_instance = new ScreenHelper();
			_instance.IsOnline = false;
		}

		private ScreenHelper()
		{
		}
	}
}
