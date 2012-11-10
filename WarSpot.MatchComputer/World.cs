namespace WarSpot.MatchComputer
{
	/// <summary>
	/// main World class
	/// </summary>
	class World
	{
		private WorldCell[,] _content;
		public WorldCell[,] Map
		{
			get
			{
				return _content;
			}
		}
		/// <summary>
		/// Width
		/// </summary>
		private readonly int _w;
		public int Width
		{
			get
			{
				return _w;
			}
		}

		/// <summary>
		/// Height
		/// </summary>
		private readonly int _h;
		public int Height
		{
			get
			{
				return _h;
			}
		}
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
