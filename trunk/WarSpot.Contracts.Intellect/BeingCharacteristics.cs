using System;
using System.Runtime.Serialization;

namespace WarSpot.Contracts.Intellect
{
	[DataContract]
    [Serializable]
	public class BeingCharacteristics
	{
		#region readlony properties: constants for fixed object
		/// <summary>
		/// Unique id of the object.
		/// </summary>
		[DataMember]
		public Guid Id { set; get; }

		/// <summary>
		/// Team of the object.
		/// In some game modes different intelects can be in one team, so this property can be used for determining the teammates.
		/// </summary>
		[DataMember]
		public Guid Team { set; get; }

		/// <summary>
		/// Maximum available health for the object.
		/// </summary>
		[DataMember]
		public float MaxHealth { set; get; }

		/// <summary>
		/// Maximum available step length.
		/// </summary>
		[DataMember]
		public float MaxStep { set; get; }

		/// <summary>
		/// Half of the visible square edge.
		/// </summary>
		[DataMember]
		public int MaxSeeDistance { set; get; }

		#endregion
		#region variable properties for this object

		/// <summary>
		/// Health of the object.
		/// </summary>
		[DataMember]
		public float Health { get; set; }

		private float _ci;
		/// <summary>
		/// Universal energy count. 
		/// </summary>
		[DataMember]
		public float Ci
		{
			get { return _ci; }
			set
			{
				_ci = value;
				if (_ci < 0)
				{
					Health += _ci;
					_ci = 0;
				}
			}
		}

		/// <summary>
		/// Current X coordinate of the object.
		/// </summary>
		[DataMember]
		public int X { get; set; }

		/// <summary>
		/// Current Y coordinate of the object.
		/// </summary>
		[DataMember]
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
		/// <param name="maxHealth">Maximum available health for the object.</param>
		/// <param name="maxStep">Maximum available step length.</param>
		/// <param name="maxSeeDistance">Half of the visible square edge.</param>
		public BeingCharacteristics(float maxHealth, float maxStep, int maxSeeDistance)
		{
			Id = Guid.NewGuid();
			MaxHealth = maxHealth;
			MaxStep = maxStep;
			MaxSeeDistance = maxSeeDistance;
		}

		public BeingCharacteristics()
		{
		}
	}
}
