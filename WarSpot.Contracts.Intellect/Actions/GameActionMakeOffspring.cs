using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionMakeOffspring: GameAction
	{

		public int X;
		public int Y;
		public float MaxHealth;
		public float MaxStep;
		public int MaxSeeDistance;

		/// <summary>
		/// Make an offspring.
		/// </summary>
		public GameActionMakeOffspring(Guid senderId, int x, int y, float maxHealth, float maxStep, int maxSeeDistance ) : base(senderId)
		{
			ActionType = ActionTypes.GameActionMakeOffspring;
			X = x;
			Y = y;
			MaxHealth = maxHealth;
			MaxStep = maxStep;
			MaxSeeDistance = maxSeeDistance;
		}
	}
}
