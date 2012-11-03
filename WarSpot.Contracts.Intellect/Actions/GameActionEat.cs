using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionEat : GameAction
	{
		public GameActionEat(Guid senderId) : base(senderId)
		{
		}

		public override float Cost()
		{
			return 0f;
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}
}