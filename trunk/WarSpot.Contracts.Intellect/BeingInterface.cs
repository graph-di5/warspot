using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.Contracts.Intellect
{
	public interface IBeingInterface
	{
		GameAction Think(ulong step, BeingCharacteristics characteristics);
	}
}
