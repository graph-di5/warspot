using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionGiveCi : GameAction
	{
		public GameActionGiveCi(Guid senderId) : base(senderId)
		{
		}

		public override float Cost()
		{
			throw new NotImplementedException();
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}
}