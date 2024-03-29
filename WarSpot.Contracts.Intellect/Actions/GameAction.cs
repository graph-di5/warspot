﻿using System;

namespace WarSpot.Contracts.Intellect.Actions
{
	public enum ActionTypes 
	{
		GameActionAtack, 
		GameActionEat, 
		GameActionGiveCi, 
		GameActionMove,
		GameActionTreat, 
		GameActionMakeOffspring
	};
	/// <summary>
	/// Parent class for all actitions
	/// </summary>
	public abstract class GameAction
	{
		/// <summary>
		/// Id of the unique action.
		/// </summary>
		public Guid Id { private set; get; }

		/// <summary>
		/// Type of the action.
		/// </summary>
		public WarSpot.Contracts.Intellect.Actions.ActionTypes ActionType {set; get; }

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
