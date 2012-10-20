using WarSpot.XNA.Framework;

namespace WarSpot.Contracts.Intellect
{
	/// <summary>
	/// One cell of the whole world iface
	/// </summary>
	public interface IWorldCell
	{
		Vector2 Coordinates { get; }

		float Ci { get; }
		IBeingInterface Being { get; }
	}

	/// <summary>
	/// One cell of the whole world internal realisation
	/// </summary>
	class WorldCell : IWorldCell
	{
		public Vector2 Coordinates { get; set; }

		public float Ci { get; set; }

		public Being BeingValue;
		public IBeingInterface Being
		{
			get { return BeingValue.Me; }
		}
	}

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
