using WarSpot.Contracts.Intellect;

namespace WarSpot.MatchComputer
{
	/// <summary>
	/// One cell of the whole world internal realisation
	/// </summary>
	class WorldCell : IWorldCell
	{
		public int X { get; set; }

		public int Y { get; set; }

		public float Ci { get; set; }

		public Being BeingValue;
		public IBeingInterface Being
		{
			get { return BeingValue.Me; }
			set { BeingValue = value as Being; }
		}
	}
}