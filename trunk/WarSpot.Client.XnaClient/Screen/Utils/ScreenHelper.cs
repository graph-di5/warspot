using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Client.XnaClient.Screen.Utils
{
	// Helper for passing arguments through screens without direct methods
	class ScreenHelper
	{
		public string ReplayPath { get; set; }
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
