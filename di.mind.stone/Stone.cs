using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace di.mind.stone
{
	public class Stone : IBeingInterface
	{
		public GameAction Think(ulong step, BeingCharacteristics characteristics)
		{
			return new GameAction(characteristics.Id);
		}
	}
}
