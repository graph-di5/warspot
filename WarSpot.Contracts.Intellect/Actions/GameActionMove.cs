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

		public override float Cost()
		{
			return ShiftX + ShiftY;//ћожно придумать, что поинтереснее. Ёту стоимость лучше рассчитывать в ComputerMatcher.
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}
}