using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionGiveCi : GameAction
	{
		public int X { get; private set; }
        public int Y { get; private set; }
        public float Ci { set; get; }

		/// <summary>
        /// Give target being Ci energy targeting to cell (relatively to actor).
		/// </summary>
		public GameActionGiveCi(Guid senderId, int x, int y, float ci) : base(senderId)
		{
			ActionType = ActionTypes.GameActionGiveCi;
			X = x;
            Y = y;
			Ci = ci;
		}
	}
}