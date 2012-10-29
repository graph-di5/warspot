using System;
using WarSpot.XNA.Framework;

namespace WarSpot.Contracts.Intellect
{
	public class BeingCharacteristics
	{
#region readlony properties: constants for fixed object
		/// <summary>
		/// Unique id of the object.
		/// </summary>
		public Guid Id { private set; get; }

		/// <summary>
		/// Maximum available health for the object.
		/// </summary>
		public float MaxHealth { private set; get; }

		/// <summary>
		/// Maximum available step length.
		/// </summary>
		public float MaxStep { private set; get; }

		/// <summary>
		/// Half of the visible square edge.
		/// </summary>
		public int MaxSeeDistance { private set; get; }

#endregion 
#region variable properties for this object

		/// <summary>
		/// Health of the object.
		/// </summary>
		public float Health { get; set; }

		/// <summary>
		/// Universal energy count. 
		/// </summary>
		public float Ci { get; set; }

		/// <summary>
		/// Current coordinates of the object.
		/// </summary>
		public Vector2 Coordinates { get; set; }

#endregion

		/// <summary>
		/// Ctor for new object.
		/// </summary>
		/// <param name="id">Unique id of the object.</param>
		/// <param name="maxHealth">Maximum available health for the object.</param>
		/// <param name="maxStep">Maximum available step length.</param>
		/// <param name="maxSeeDistance">Half of the visible square edge.</param>
		public BeingCharacteristics(Guid id, float maxHealth, float maxStep, int maxSeeDistance)
		{
			Id = id;
			MaxHealth = maxHealth;
			MaxStep = maxStep;
			MaxSeeDistance = maxSeeDistance;
		}
	}
}
