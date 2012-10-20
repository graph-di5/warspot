using System;
using WarSpot.XNA.Framework;

namespace WarSpot.Contracts.Intellect
{
	public class BeingCharacteristics
	{
#region статические
		/// <summary>
		/// Unique id of the object
		/// </summary>
		public Guid Id { private set; get; }

		// todo максимальный процент чего ?
		/// <summary>
		/// определяет максимальное количество жизненной энергии
		/// определяет максимальны процент восполнения 
		/// </summary>
		public float MaxHealth { private set; get; }

		/// <summary>
		/// max step length
		/// </summary>
		public float MaxStep { private set; get; }

		/// <summary>
		/// Half of the visible square edge
		/// </summary>
		public int MaxSeeDistance { private set; get; }

#endregion 
#region динамические

		public float Health { get; set; }

		/// <summary>
		/// Universal energy count. 
		/// </summary>
		public float Ci { get; set; }

		public Vector2 Coordinates { get; set; }

#endregion

		BeingCharacteristics(Guid id, float maxHealth, float maxStep, int maxSeeDistance)
		{
			Id = id;
			MaxHealth = maxHealth;
			MaxStep = maxStep;
			MaxSeeDistance = maxSeeDistance;
		}
	}
}
