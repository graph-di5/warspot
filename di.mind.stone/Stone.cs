using System;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace di.mind.stone
{
	public class Stone : IBeingInterface
	{
		public BeingCharacteristics Construct(ulong step, float ci, IWorldCell[,] area)
		{
			return new BeingCharacteristics(Guid.NewGuid(), 100500, 0, 1);
		}

		public GameAction Think(ulong step, BeingCharacteristics characteristics, IWorldCell[,] area)
		{
			return new GameActionEat(characteristics.Id);
		}
	}
}
