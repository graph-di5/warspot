using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionDie : GameAction
	{
		public GameActionDie(Guid senderId) : base(senderId)
		{
			ActionType = ActionTypes.GameActionDie;
		}
	}
}