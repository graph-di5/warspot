using System;

namespace WarSpot.Client.XnaClient
{
#if WINDOWS || XBOX
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			using (WarSpotGame game = new WarSpotGame())
			{
				game.Run();
			}
		}
	}
#endif
}

