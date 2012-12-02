using WarSpot.Contracts.Intellect;

namespace WarSpot.MatchComputer
{
	/// <summary>
	/// One cell of the whole world internal realisation
	/// </summary>
	class WorldCell
	{
		public int X { get; set; }

		public int Y { get; set; }

		public float Ci { get; set; }

		// todo //!! delete this and merge
		public Being BeingValue;
#if false
		public IBeingInterface Being
		{
			get { return BeingValue.Me; }
			// todo check this
			set { BeingValue = value as Being; }
		}
#endif
	}
}