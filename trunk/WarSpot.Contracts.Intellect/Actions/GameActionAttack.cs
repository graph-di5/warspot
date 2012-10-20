using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionAttack : GameAction
	{
		public Guid TargetID { private set; get; }
		public GameActionAttack(Guid senderId, Guid targetID) : base(senderId)
		{
			TargetID = targetID;
		}
	}
}