using System;

namespace WarSpot.Contracts.Intellect
{
	[Serializable]
	public class BeingCharacteristics
	{
		#region readlony properties: constants for fixed object
		/// <summary>
		/// Unique id of the object.
		/// </summary>
		public Guid Id { private set; get; }

		/// <summary>
		/// Team of the object.
		/// In some game modes different intelects can be in one team, so this property can be used for determining the teammates.
		/// </summary>
		public int Team { private set; get; }

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

		private float _ci;
		/// <summary>
		/// Universal energy count. 
		/// </summary>
		public float Ci
		{
			get { return _ci; }
			set
			{
				_ci = value;
				if(_ci < 0)
				{
					Health += _ci;
					_ci = 0;
				}
			}
		}

		/// <summary>
		/// Current X coordinate of the object.
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Current Y coordinate of the object.
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Cost of such Being in Ci.
		/// </summary>
		public float Cost()
		{
			return (MaxHealth * 0.8f) + (MaxStep * MaxStep) + ((MaxSeeDistance / 2) * (MaxSeeDistance / 2));
		}
		#endregion

		/// <summary>
		/// Ctor for new object.
		/// </summary>
		/// <param name="id">Unique id of the object.</param>
		/// <param name="team"> Team number.</param>
		/// <param name="maxHealth">Maximum available health for the object.</param>
		/// <param name="maxStep">Maximum available step length.</param>
		/// <param name="maxSeeDistance">Half of the visible square edge.</param>
		public BeingCharacteristics(Guid id, int team, float maxHealth, float maxStep, int maxSeeDistance)
		{
			Id = id;
			Team = team;
			MaxHealth = maxHealth;
			MaxStep = maxStep;
			MaxSeeDistance = maxSeeDistance;
		}
	}
}
