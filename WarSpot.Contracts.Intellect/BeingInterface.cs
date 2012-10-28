using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.Contracts.Intellect
{
	/// <summary>
	/// Iface of all beings in the world
	/// </summary>
	public interface IBeingInterface
	{
		// todo write good wrapper for worldcells 

		/// <summary>
		/// First function called for every being object. 
		/// </summary>
		/// <param name="step">Current time step</param>
		/// <param name="ci">Ci available for creating of this object.</param>
		/// <param name="area">Array of IWorldCell around the being describes visible part of the world. Array is (MaxSeeDistance*2+1)^2, with the being in central cell</param>
		/// <returns>Characteriscts of the object.</returns>
		BeingCharacteristics Construct(ulong step, float ci, IWorldCell[,] area);

		/// <summary>
		/// Main function of the every being in the world.
		/// </summary>
		/// <param name="step">Current time step</param>
		/// <param name="characteristics">Updated characteristics of the being.</param>
		/// <param name="area">Array of IWorldCell around the being describes visible part of the world. Array is (MaxSeeDistance*2+1)^2, with the being in central cell</param>
		/// <returns>Decided action of the being. May be NULL.</returns>
		GameAction Think(ulong step, BeingCharacteristics characteristics, IWorldCell[,] area);
	}
}
