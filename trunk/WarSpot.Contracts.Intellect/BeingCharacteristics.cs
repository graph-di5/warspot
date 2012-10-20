using WarSpot.XNA.Framework;

namespace WarSpot.Contracts.Intellect
{
	public class BeingCharacteristics
	{
#region статические
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

		BeingCharacteristics(float maxHealth, float maxStep, float maxSeeDistance)
		{
			MaxHealth = maxHealth;
			MaxStep = maxStep;
			MaxSeeDistance = maxSeeDistance;
		}
	}
}
