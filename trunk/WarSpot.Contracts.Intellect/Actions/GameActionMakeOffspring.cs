using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Contracts.Intellect.Actions
{
	public class GameActionMakeOffspring: GameAction
	{
		/// <summary>
		/// Make an offspring.
		/// </summary>
		public GameActionMakeOffspring(Guid senderId) : base(senderId)
		{
			ActionType = ActionTypes.GameActionMakeOffspring;
		}
	}
}
