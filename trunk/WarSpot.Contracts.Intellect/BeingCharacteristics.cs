using System;
using WarSpot.XNA.Framework;

namespace WarSpot.Contracts.Intellect
{
	public class BeingCharacteristics
	{
#region статические

		public Guid Id { private set; get; }

		//  определяет максимальное количество жизненной энергии
		//    определяет максимальны процент восполнения
		public float MaxHealth { private set; get; }

		public float MaxStep { private set; get; }

		// дальность просмотра
		public float MaxSeeDistance { private set; get; }

#endregion 
#region динамические

		public float Health { get; set; }

		// 	"очки хода" ? = энергии
		public float Ci { get; set; }

		public Vector2 Coordinates { get; set; }

#endregion

		BeingCharacteristics(Guid id, float maxHealth, float maxStep, float maxSeeDistance)
		{
			Id = id;
			MaxHealth = maxHealth;
			MaxStep = maxStep;
			MaxSeeDistance = maxSeeDistance;
		}
	}
}
