using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionDie : GameAction
	{
		public GameActionDie(Guid senderId) : base(senderId)
		{
			ActionType = ActionTypes.GameActionDie;
		}

		public override float Cost()
		{
			return 0;
		}

		public override void Execute()
		{
			//Видимо, здесь будет выброс энергии из трупа.
			throw new NotImplementedException();
		}
	}
}