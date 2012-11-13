using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionTreat : GameAction
	{
		public float HealthAmount { private set; get; }
		// todo may be added target parameter or another action
		public GameActionTreat(Guid senderId, Guid targetId, float healthAmount) : base(senderId)
		{
			ActionType = ActionTypes.GameActionTreat;
			HealthAmount = healthAmount;
			TargetId = targetId;
		}

		public Guid TargetId { get; private set; }

		public override float Cost()
		{
			return HealthAmount;//Можно множитель добавить.
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}
}