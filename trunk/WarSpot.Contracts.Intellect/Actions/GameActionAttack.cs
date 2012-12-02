using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionAttack : GameAction
	{
		public int X { get; private set; }
        public int Y { get; private set; }
		public float Damage { set; get; }

		/// <summary>
        /// Atack target cell (relatively to actor) using Ci energy.
		/// </summary>
		public GameActionAttack(Guid senderId, int x, int y, float ci) : base(senderId)
		{
			ActionType = ActionTypes.GameActionAtack;
            X = x;
            Y = y;
			Damage = ci;
		}
	}
}