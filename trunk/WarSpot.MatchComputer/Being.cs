using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.MatchComputer
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
		public GameAction Think(ulong step, BeingCharacteristics characteristics)
		{
			return Me.Think(step, characteristics); 
		}
	}
}
