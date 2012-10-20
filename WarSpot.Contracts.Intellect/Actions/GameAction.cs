using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	/// <summary>
	/// Parent class for all actitions
	/// </summary>
	public class GameAction
	{
		public Guid Id { private set; get; }
		public Guid SenderId { private set; get; }

		public GameAction(Guid senderId)
		{
			SenderId = senderId;
			Id = Guid.NewGuid();
		}
	}

#if false
	// todo enable this
	public class GameActionReproduction : GameAction
	{
		public GameActionReproduction(Guid senderId) : base(senderId)
		{
		}
	}
#endif
}
