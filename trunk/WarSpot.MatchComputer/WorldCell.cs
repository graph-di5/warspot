namespace WarSpot.MatchComputer
{
	/// <summary>
	/// One cell of the whole world internal realisation
	/// </summary>
	class WorldCell
	{
		public int X {  get; private set; }

		public int Y { get; private set; }

		public float Ci { get; set; }

		// todo //!! delete this and merge
		private Being _beingValue;
		public Being BeingValue
		{
			get
			{
				return _beingValue;
			}
			set
			{
				_beingValue = value;
				if(_beingValue != null)
				{
					_beingValue.Characteristics.X = X;
					_beingValue.Characteristics.Y = Y;
				}
			}
		}

		public WorldCell(int x, int y)
		{
			Y = y;
			X = x;
		}

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