using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	/// <summary>
	/// Parent class for all actitions
	/// </summary>
	public abstract class GameAction
	{
		/// <summary>
		/// Id of the action.
		/// </summary>
		public Guid Id { private set; get; }
		/// <summary>
		/// Id of the being that created the action.
		/// </summary>
		public Guid SenderId { private set; get; }

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="senderId">Id of the being that created the action.</param>
		protected GameAction(Guid senderId)
		{
			SenderId = senderId;
			Id = Guid.NewGuid();
		}

		/// <summary>
		/// Cost in ci of the action.
		/// </summary>
		/// <returns>Ci need for execute the sction.</returns>
		public abstract float Cost();

		/// <summary>
		/// Execute the action.
		/// </summary>
		/// <returns></returns>
		public abstract void Execute();
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
