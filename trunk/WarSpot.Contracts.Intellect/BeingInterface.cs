using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.Contracts.Intellect
{
	/// <summary>
	/// Iface of all beings in the world
	/// </summary>
	public interface IBeingInterface
	{
		// todo add arra of worldcells 

		/// <summary>
		/// Main function of the every being in the world.
		/// </summary>
		/// <param name="step">Current time step</param>
		/// <param name="characteristics">Updated characteristics of the being.</param>
		/// <returns>Decided action of the being. May be NULL.</returns>
		GameAction Think(ulong step, BeingCharacteristics characteristics);
	}
}
