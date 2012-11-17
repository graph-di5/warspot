namespace WarSpot.Contracts.Intellect
{
	/// <summary>
	/// Iface for one cell of the whole world
	/// </summary>
	public interface IWorldCell
	{
		/// <summary>
		/// Cell coordinates
		/// </summary>
		int X { get; }
		int Y { get; }

		/// <summary>
		/// Count of free available Ci
		/// </summary>
		float Ci { get; }

		/// <summary>
		/// Reference to the object in the cell. May be NULL
		/// </summary>
		IBeingInterface Being { get; }
	}
}