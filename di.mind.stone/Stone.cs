using System;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace di.mind.stone
{
	public class Stone : IBeingInterface
	{
		public BeingCharacteristics Construct(ulong step, float ci)
		{
			return new BeingCharacteristics(Guid.NewGuid(), 100.0f, 5.0f, 1);
		}

		public GameAction Think(ulong step, BeingCharacteristics characteristics, IWorldCell[,] area)
		{
			return new GameActionEat(characteristics.Id);
		}
	}
}
