using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionAttack : GameAction
	{
		public Guid TargetId { get; private set; }

		public GameActionAttack(Guid senderId, Guid targetId) : base(senderId)
		{
			ActionType = ActionTypes.GameActionAtack;
			TargetId = targetId;
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