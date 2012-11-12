using System;
using WarSpot.XNA.Framework;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionMove : GameAction
	{
		public Vector2 Shift { private set; get; }
		public GameActionMove(Guid senderId, Vector2 shift) : base(senderId)
		{
			ActionType = ActionTypes.GameActionMove;
			Shift = shift;
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