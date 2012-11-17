using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionGiveCi : GameAction
	{
		public float Ci;

		public Guid TargetId { get; private set; }

		/// <summary>
		/// Give target Ci energy
		/// </summary>
		public GameActionGiveCi(Guid senderId, Guid targetId, float ci) : base(senderId)
		{
			ActionType = ActionTypes.GameActionGiveCi;
			TargetId = targetId;
			Ci = ci;
		}
	}
}