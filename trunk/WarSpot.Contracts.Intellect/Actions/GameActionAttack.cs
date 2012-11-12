using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionAttack : GameAction
	{
		public Guid TargetID { private set; get; }
		public GameActionAttack(Guid senderId, Guid targetID) : base(senderId)
		{
			ActionType = ActionTypes.GameActionAtack;
			TargetID = targetID;
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