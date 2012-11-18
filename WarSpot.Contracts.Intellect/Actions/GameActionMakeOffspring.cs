using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionMakeOffspring: GameAction
	{		
		public float Ci;
		
		/// <summary>
		/// Make an offspring.
		/// </summary>
		public GameActionMakeOffspring(Guid senderId, float ci) : base(senderId)
		{
			ActionType = ActionTypes.GameActionMakeOffspring;
			Ci = ci;
		}
	}
}
