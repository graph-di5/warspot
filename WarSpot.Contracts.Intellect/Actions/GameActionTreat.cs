using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionTreat : GameAction
	{
		public float HealthAmount { private set; get; }
		// todo may be added target parameter or another action
		public GameActionTreat(Guid senderId, float healthAmount) : base(senderId)
		{
			HealthAmount = healthAmount;
		}
	}
}