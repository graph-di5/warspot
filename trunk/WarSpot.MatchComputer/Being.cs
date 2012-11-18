using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.MatchComputer
{
	/// <summary>
	/// Main being class in the MathComputer. "Me" field if reference to custom beings or "standart".
	/// </summary>
	internal class Being : IBeingInterface
	{
		/// <summary>
		/// Characterisct of the current being. Placed here for security.
		/// </summary>
		public BeingCharacteristics Characteristics { get; set; }

		/// <summary>
		/// Reference to the concrete being.
		/// </summary>
		public IBeingInterface Me { private set; get; }

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="me">Reference to the custom object</param>
		public Being(IBeingInterface me, int team)
		{
			Me = me;
			
		}

		/// <summary>
		/// Calls think of custom being.
		/// </summary>
		/// <param name="turnNumber">Current time step</param>
		/// <param name="characteristics">Updated characteristics of the being.</param>
		/// <param name="area">Array of IWorldCell around the being describes visible part of the world. Array is (MaxSeeDistance*2+1)^2, with the being in central cell</param>
		/// <returns>Decided action of the being. May be NULL.</returns>
		public GameAction Think(ulong turnNumber, BeingCharacteristics characteristics, IWorldCell[,] area)
		{
			// todo add here catch(){}
			return Me.Think(turnNumber, characteristics, area); 
		}

		/// <summary>
		/// Calls Construct of the custmo being.
		/// </summary>
		/// <param name="team"> </param>
		/// <param name="turnNumber">Current time step</param>
		/// <param name="ci">Ci available for creating of this object.</param>
		/// <param name="area">Array of IWorldCell around the being describes visible part of the world. Array is (MaxSeeDistance*2+1)^2, with the being in central cell</param>
		/// <returns>Characteriscts of the being.</returns>
		public BeingCharacteristics Construct(int team, ulong turnNumber, float ci, IWorldCell[,] area)
		{
			// todo add here checking of the returned characteristics
			return Me.Construct(team, turnNumber, ci, area);
		}

	}
}
