using System;
using WarSpot.XNA.Framework;

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

	public class GameEventHealthChange : GameEvent
	{
		public GameEventHealthChange(Guid subjectId, float newHealth) :
			base(subjectId)
		{
			Health = newHealth;
		}

		/// <summary>
		/// new health
		/// </summary>
		public float Health { get; private set; }
	}

	public class GameEventCiChange : GameEvent
	{
		public GameEventCiChange(Guid subjectId, float newCi) :
			base(subjectId)
		{
			Ci = newCi;
		}

		/// <summary>
		/// new Ci
		/// </summary>
		public float Ci { get; private set; }
	}

	public class GameEventDeath : GameEvent
	{
		public GameEventDeath(Guid creator) : base(creator)
		{
		}
	}

	public class GameEventBirth : GameEvent
	{
		public GameEventBirth(Guid creator) : base(creator)
		{
		}
	}

	public class GameEventWorldCiChanged : WarSpotEvent
	{
		public GameEventWorldCiChanged(Vector2 coordinates, float ci)
		{
			Coordinates = coordinates;
			Ci = ci;
		}

		public Vector2 Coordinates { get; private set; }
		/// <summary>
		/// new Ci
		/// </summary>
		public float Ci { get; private set; }
	}

	public abstract class SystemEvent : WarSpotEvent
	{
	}

	public class SystemEventCommandDead : SystemEvent
	{
		public SystemEventCommandDead(int teamId)
		{
			TeamId = teamId;
		}

		public int TeamId { get; private set; }
	}

	public class SystemEventCommandWin : SystemEvent
	{
		public SystemEventCommandWin(int teamId)
		{
			TeamId = teamId;
		}

		public int TeamId { get; private set; }
	}

	public class SystemEventMatchEnd : SystemEvent
	{
	}
}
