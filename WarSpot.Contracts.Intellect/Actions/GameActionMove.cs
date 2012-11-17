using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionMove : GameAction
	{
		public GameActionMove(Guid senderId, int shiftX, int shiftY) : base(senderId)
		{
			ActionType = ActionTypes.GameActionMove;
			ShiftX = shiftX;
			ShiftY = shiftY;
		}

		public int ShiftX { private set; get; }

		public int ShiftY { private set; get; }
	}
}