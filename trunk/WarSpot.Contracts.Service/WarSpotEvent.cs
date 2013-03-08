using System;
using System.Runtime.Serialization;
using WarSpot.Contracts.Intellect;

// todo spit to different files

// ATTENTION: Constructors without parameters are for serializing deserializing

namespace WarSpot.Contracts.Service
{
    [DataContract]
	public enum EventTypes
	{
		#region Game events
        [EnumMember]
		GameEventHealthChange,
        [EnumMember]
		GameEventCiChange,
        [EnumMember]
		GameEventMove,
        [EnumMember]
		GameEventDeath,
        [EnumMember]
		GameEventBirth,
        [EnumMember]
		GameEventWorldCiChanged,
		#endregion

		#region System events
        [EnumMember]
		SystemEventWorldCreated,
        [EnumMember]
		SystemEventTurnStarted,
        [EnumMember]
		SystemEventCommandDead,
        [EnumMember]
		SystemEventCommandWin,
        [EnumMember]
		SystemEventMatchEnd
		#endregion
	};

	[DataContract]
    [KnownType(typeof(GameEventHealthChange))]
    [KnownType(typeof(GameEventCiChange))]
    [KnownType(typeof(GameEventMove))]
    [KnownType(typeof(GameEventDeath))]
    [KnownType(typeof(GameEventBirth))]
    [KnownType(typeof(GameEventWorldCiChanged))]
    [KnownType(typeof(SystemEventWorldCreated))]
    [KnownType(typeof(SystemEventTurnStarted))]
    [KnownType(typeof(SystemEventCommandDead))]
    [KnownType(typeof(SystemEventCommandWin))]
    [KnownType(typeof(SystemEventMatchEnd))]
	public abstract class WarSpotEvent
	{
		/// <summary>
		/// Type of the event.
		/// </summary>
		[DataMember]
		public EventTypes EventType { set; get; }
	}

	#region Game events
	[DataContract]
	public abstract class GameEvent : WarSpotEvent
	{
		protected GameEvent(Guid creator)
		{
			SubjectId = creator;
		}

        protected GameEvent()
        {
        }

	    [DataMember]
        public Guid SubjectId { get; set; }
	}

	[DataContract]
	public class GameEventHealthChange : GameEvent
	{

		public GameEventHealthChange(Guid subjectId, float newHealth) :
			base(subjectId)
		{
			EventType = EventTypes.GameEventHealthChange;
			Health = newHealth;
		}

        public GameEventHealthChange()
        {
        }

	    /// <summary>
		/// new health
		/// </summary>
		[DataMember]
		public float Health { get; set; }
	}

    [DataContract]
    public class GameEventCiChange : GameEvent
    {
        public GameEventCiChange(Guid subjectId, float newCi) :
            base(subjectId)
        {
            EventType = EventTypes.GameEventCiChange;
            Ci = newCi;
        }

        public GameEventCiChange()
        {
        }

        /// <summary>
		/// new Ci
		/// </summary>
		[DataMember]
		public float Ci { get; set; }
	}

	[DataContract]
	public class GameEventMove : GameEvent
	{
		public GameEventMove(Guid subjectId, int shiftX, int shiftY) :
			base(subjectId)
		{
			EventType = EventTypes.GameEventMove;
			ShiftX = shiftX;
			ShiftY = shiftY;
		}

        public GameEventMove()
        {
        }

	    [DataMember]
		public int ShiftX { get; private set; }

        [DataMember]
		public int ShiftY { get; private set; }
	}

    [DataContract]
	public class GameEventDeath : GameEvent
	{
        public GameEventDeath()
        {
        }

        public GameEventDeath(Guid creator)
			: base(creator)
		{
			EventType = EventTypes.GameEventDeath;
		}
	}

    [DataContract]
	public class GameEventBirth : GameEvent
	{
        [DataMember]
		public BeingCharacteristics Newborn { set; get; }

        public GameEventBirth()
        {
        }

        public GameEventBirth(Guid creator, BeingCharacteristics newborn)
			: base(creator)
		{
			EventType = EventTypes.GameEventBirth;
			Newborn = newborn;
		}
	}

	[DataContract]
	public class GameEventWorldCiChanged : WarSpotEvent
	{
		public GameEventWorldCiChanged(int x, int y, float ci)
		{
			EventType = EventTypes.GameEventWorldCiChanged;
			X = x;
			Y = y;
			Ci = ci;
		}

        public GameEventWorldCiChanged()
        {
        }

	    [DataMember]
		public int X { get; set; }

        [DataMember]
		public int Y { get; set; }

		/// <summary>
		/// new Ci
		/// </summary>
        [DataMember]
		public float Ci { get; set; }
	}
	#endregion

	#region System events

	[DataContract]
    public class SystemEventWorldCreated : WarSpotEvent
	{
        [DataMember]
		public int Width { set; get; }

        [DataMember]
		public int Height { set; get; }

        public SystemEventWorldCreated()
        {
        }

	    public SystemEventWorldCreated(int width, int height)
		{
			EventType = EventTypes.SystemEventWorldCreated;
			Width = width;
			Height = height;
		}
	}

	[DataContract]
    public class SystemEventTurnStarted : WarSpotEvent
	{
        [DataMember]
		public ulong Number { set; get; }

        public SystemEventTurnStarted()
        {
        }

	    public SystemEventTurnStarted(ulong number)
		{
			EventType = EventTypes.SystemEventTurnStarted;
			Number = number;
		}
	}

    [DataContract]
    public class SystemEventCommandDead : WarSpotEvent
	{
        public SystemEventCommandDead()
        {
        }

		public SystemEventCommandDead(int teamId)
		{
			EventType = EventTypes.SystemEventCommandDead;
			TeamId = teamId;
		}

        [DataMember]
		public int TeamId { get; set; }
	}

    [DataContract]
    public class SystemEventCommandWin : WarSpotEvent
	{
        public SystemEventCommandWin()
        {
        }

        public SystemEventCommandWin(int teamId)
		{
			EventType = EventTypes.SystemEventCommandWin;
			TeamId = teamId;
		}

        [DataMember]
		public int TeamId { get; set; }
	}

    [DataContract]
    public class SystemEventMatchEnd : WarSpotEvent
	{
		public SystemEventMatchEnd()
		{
			EventType = EventTypes.SystemEventMatchEnd;
		}
	}
	#endregion
}
