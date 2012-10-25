namespace WarSpot.MatchComputer
{
	/// <summary>
	/// main World class
	/// </summary>
	class World
	{
		private WorldCell[,] _content;

		/// <summary>
		/// Width
		/// </summary>
		private readonly int _w;

		/// <summary>
		/// Height
		/// </summary>
		private readonly int _h;

		/// <summary>
		/// ctor
		/// </summary>
		public World()
		{
			_w = 100;
			_h = 70;
			_content = new WorldCell[_w, _h];
		}
	}
}
