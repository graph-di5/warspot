﻿using System;
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
		public List<ReplayDescription> ListOfReplays { get; set; }
		public Guid DownloadedGameGuid { get; set; }
		private static ScreenHelper _instance;
		public static ScreenHelper Instance
		{
			get { return _instance; }
		}
        public bool SaveReplay { get; set; }
        public bool OnlineReplayMode { get; set; }
        public MatchReplay ToSerialize { get; set; }

		static ScreenHelper()
		{
			_instance = new ScreenHelper();
			_instance.DownloadedGameGuid = new Guid();
            _instance.SaveReplay = false;
            _instance.OnlineReplayMode = false;
		}

		private ScreenHelper()
		{
		}
	}
}
