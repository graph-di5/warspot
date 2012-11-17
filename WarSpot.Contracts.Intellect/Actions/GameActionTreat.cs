using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionTreat : GameAction
	{
		public float UsingCi { private set; get; }

		/// <summary>
		/// Theat target using Ci energy.
		/// </summary>
		public GameActionTreat(Guid senderId, Guid targetId, float Ci) : base(senderId)
		{
			ActionType = ActionTypes.GameActionTreat;
			UsingCi = Ci;
			TargetId = targetId;
		}

		public Guid TargetId { get; private set; }
	}
}