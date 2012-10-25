using WarSpot.XNA.Framework;

namespace WarSpot.Contracts.Intellect
{
	/// <summary>
	/// One cell of the whole world iface
	/// </summary>
	public interface IWorldCell
	{
		Vector2 Coordinates { get; }

		float Ci { get; }
		IBeingInterface Being { get; }
	}
}