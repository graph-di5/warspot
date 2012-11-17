using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionAttack : GameAction
	{
		public Guid TargetId { get; private set; }
		public float Ci { set; get; }

		/// <summary>
		/// Atack target using Ci energy.
		/// </summary>
		public GameActionAttack(Guid senderId, Guid targetId, float ci) : base(senderId)
		{
			ActionType = ActionTypes.GameActionAtack;
			TargetId = targetId;
			Ci = ci;
		}
	}
}