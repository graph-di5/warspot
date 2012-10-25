using WarSpot.Contracts.Intellect;
using WarSpot.XNA.Framework;

namespace WarSpot.MatchComputer
{
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
}