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

	public class GameEventMove : GameEvent
	{
		public GameEventMove(Guid subjectId, int shiftX, int shiftY):
			base(subjectId)
		{
			ShiftX = shiftX;
			ShiftY = shiftY;
		}

		public int ShiftX { get; private set; }

		public int ShiftY { get; private set; }
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
		public GameEventWorldCiChanged(int x, int y, float ci)
		{
			X = x;
			Y = y;
			Ci = ci;
		}

		public int X { get; private set; }

		public int Y { get; private set; }

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
