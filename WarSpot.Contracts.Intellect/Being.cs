using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.Contracts.Intellect
{
	internal class Being : IBeingInterface
	{
		public BeingCharacteristics Characteristics { get; set; }
		public IBeingInterface Me { private set; get; }

		public Being(IBeingInterface me)
		{
			Me = me;
		}

		// todo set parameters
		public GameAction Think()
		{
			return Me.Think(); 
		}
	}
}
