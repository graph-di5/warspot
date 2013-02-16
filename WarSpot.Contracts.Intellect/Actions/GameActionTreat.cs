using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionTreat : GameAction
	{
        public int X { get; private set; }
        public int Y { get; private set; }
		public float UsingCi { private set; get; }

		/// <summary>
        /// Theat target being using Ci energy targeting to cell (relatively to actor).
		/// </summary>
		public GameActionTreat(Guid senderId, int x, int y, float ci) : base(senderId)
		{
			ActionType = ActionTypes.GameActionTreat;
            X = x;
            Y = y;
			UsingCi = ci;
			
		}

		public Guid TargetId { get; private set; }
	}
}