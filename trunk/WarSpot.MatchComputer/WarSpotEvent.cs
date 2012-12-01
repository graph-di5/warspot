using System;
using WarSpot.Contracts.Intellect;

// todo spit to different files

namespace WarSpot.MatchComputer
{
	public enum EventTypes
	{
		#region Game events
		GameEventHealthChange,
		GameEventCiChange,
		GameEventMove,
		GameEventDeath,
		GameEventBirth,
		GameEventWorldCiChanged,
		#endregion

		#region System events
		SystemEventWorldCreated,
		SystemEventTurnStarted,
		SystemEventCommandDead,
		SystemEventCommandWin,
		SystemEventMatchEnd
		#endregion
	};

	[Serializable]
	public abstract class WarSpotEvent
	{
		/// <summary>
		/// Type of the event.
		/// </summary>
		public EventTypes EventType { set; get; }
	}

	#region Game events
	[Serializable]
	public abstract class GameEvent : WarSpotEvent
	{
		protected GameEvent(Guid creator)
		{
			SubjectId = creator;
		}

		public Guid SubjectId { get; private set; }
	}

	[Serializable]
	public class GameEventHealthChange : GameEvent
	{
		public GameEventHealthChange(Guid subjectId, float newHealth) :
			base(subjectId)
		{
			EventType = EventTypes.GameEventHealthChange;
			Health = newHealth;
		}

		/// <summary>
		/// new health
		/// </summary>
		public float Health { get; private set; }
	}

	[Serializable]
	public class GameEventCiChange : GameEvent
	{
		public GameEventCiChange(Guid subjectId, float newCi) :
			base(subjectId)
		{
			EventType = EventTypes.GameEventCiChange;
			Ci = newCi;
		}

		/// <summary>
		/// new Ci
		/// </summary>
		public float Ci { get; private set; }
	}

	[Serializable]
	public class GameEventMove : GameEvent
	{
		public GameEventMove(Guid subjectId, int shiftX, int shiftY) :
			base(subjectId)
		{
			EventType = EventTypes.GameEventMove;
			ShiftX = shiftX;
			ShiftY = shiftY;
		}

		public int ShiftX { get; private set; }

		public int ShiftY { get; private set; }
	}

	[Serializable]
	public class GameEventDeath : GameEvent
	{
		public GameEventDeath(Guid creator)
			: base(creator)
		{
			EventType = EventTypes.GameEventDeath;
		}
	}

	[Serializable]
	public class GameEventBirth : GameEvent
	{
		public BeingCharacteristics Newborn { set; get; }

		public GameEventBirth(Guid creator, BeingCharacteristics newborn)
			: base(creator)
		{
			EventType = EventTypes.GameEventBirth;
			Newborn = newborn;
		}
	}

	[Serializable]
	public class GameEventWorldCiChanged : WarSpotEvent
	{
		public GameEventWorldCiChanged(int x, int y, float ci)
		{
			EventType = EventTypes.GameEventWorldCiChanged;
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
	#endregion

	#region System events

	[Serializable]
	public abstract class SystemEvent : WarSpotEvent
	{
	}

	[Serializable]
	public class SystemEventWorldCreated : SystemEvent
	{
		public int Width { set; get; }
		public int Height { set; get; }

		public SystemEventWorldCreated(int width, int height)
		{
			EventType = EventTypes.SystemEventWorldCreated;
			Width = width;
			Height = height;
		}
	}

	[Serializable]
	public class SystemEventTurnStarted : SystemEvent
	{
		public ulong Number { set; get; }

		public SystemEventTurnStarted(ulong number)
		{
			EventType = EventTypes.SystemEventTurnStarted;
			Number = number;
		}
	}

	[Serializable]
	public class SystemEventCommandDead : SystemEvent
	{
		public SystemEventCommandDead(int teamId)
		{
			EventType = EventTypes.SystemEventCommandDead;
			TeamId = teamId;
		}

		public int TeamId { get; private set; }
	}

	[Serializable]
	public class SystemEventCommandWin : SystemEvent
	{
		public SystemEventCommandWin(int teamId)
		{
			EventType = EventTypes.SystemEventCommandWin;
			TeamId = teamId;
		}

		public int TeamId { get; private set; }
	}

	[Serializable]
	public class SystemEventMatchEnd : SystemEvent
	{
		public SystemEventMatchEnd()
		{
			EventType = EventTypes.SystemEventMatchEnd;
		}
	}
	#endregion
}
