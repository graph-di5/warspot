using System;

namespace WarSpot.MatchComputer
{
	public abstract class WarSpotEvent
	{

	}

	public abstract class GameEvent : WarSpotEvent
	{
		protected GameEvent(Guid creator)
		{
			SubjectId = creator;
		}

		public Guid SubjectId { get; private set; }
	}

	// 		public Guid TargetID { private set; get; }
	public class GameEventHealthChange
	{
		public GameEventHealthChange(float newHealth)
		{
			Health = newHealth;
		}

		/// <summary>
		/// new health
		/// </summary>
		public float Health { get; private set; }
	}

	public abstract class SystemEvent : WarSpotEvent
	{
	}

	public class SystemEventCommandDead
	{
		public SystemEventCommandDead(int teamId)
		{
			TeamId = teamId;
		}

		public int TeamId { get; private set; }
	}
}
