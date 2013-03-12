using System;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;
using WarSpot.Contracts.Service;

namespace WarSpot.MatchComputer
{
	/// <summary>
	/// Main being class in the MathComputer. "Me" field if reference to custom beings or "standart".
	/// </summary>
	public class Being : IBeingInterface
	{
		/// <summary>
		/// Characterisct of the current being. Placed here for security.
		/// </summary>
		public BeingCharacteristics Characteristics { get; set; }

		/// <summary>
		/// Reference to the concrete being.
		/// </summary>
		public IBeingInterface Me { private set; get; }

		public Type TypeOfMe { get; private set; }


		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="typeOfMe">Reference to the custom object</param>
		/// <param name="team"> </param>
		public Being(Type typeOfMe, Guid team)
		{
			TypeOfMe = typeOfMe;
			var defaultCtor = typeOfMe.GetConstructor(new Type[0]);
			if (defaultCtor != null)
			{
				var inst = defaultCtor.Invoke(new Type[0]);
				Me = inst as IBeingInterface;
			}
		}

		/// <summary>
		/// Calls think of custom being.
		/// </summary>
		/// <param name="turnNumber">Current time step</param>
		/// <param name="characteristics">Updated characteristics of the being.</param>
		/// <param name="area">Array of IWorldCell around the being describes visible part of the world. Array is (MaxSeeDistance*2+1)^2, with the being in central cell</param>
		/// <returns>Decided action of the being. May be NULL.</returns>
		public GameAction Think(ulong turnNumber, BeingCharacteristics characteristics, WorldInfo area)
		{
			// todo add here catch(){}
			return Me.Think(turnNumber, characteristics, area); 
		}

		/// <summary>
		/// Calls Construct of the custmo being.
		/// </summary>
		/// <param name="turnNumber">Current time step</param>
		/// <param name="ci">Ci available for creating of this object.</param>
		/// <returns>Characteriscts of the being.</returns>
		public BeingCharacteristics Construct(ulong turnNumber, float ci)
		{
			// todo add here checking of the returned characteristics
			Characteristics = Me.Construct(turnNumber, ci);
            Characteristics.Health = Characteristics.MaxHealth;
            if (ci < 0)
            {
                Characteristics.Health += ci;
                Characteristics.Ci = 0;
            }
			else Characteristics.Ci = ci;
			
			return Characteristics;
		}

	}
}
