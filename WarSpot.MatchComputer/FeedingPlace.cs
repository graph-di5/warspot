	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
	using WarSpot.Contracts.Service;

namespace WarSpot.MatchComputer
{
	class FeedingPlace
	{
		public int X
		{
			get;
			private set;
		}
		public int Y
		{
			get;
			private set;
		}

		private float _maxCiCount;
		private float _producingSpeed;
		private World _world;
		private List<WarSpotEvent> _eventsHistory;

		public FeedingPlace (World world, List<WarSpotEvent> eventsHistory, int x, int y, float maxCiCount, float producingSpeed)
		{
			_world = world;
			_eventsHistory = eventsHistory;
			X = x;
			Y = y;
			_maxCiCount = maxCiCount;
			_producingSpeed = producingSpeed;

			world[X, Y].Ci = Math.Max(world[X, Y].Ci, maxCiCount); 
			_eventsHistory.Add(new GameEventWorldCiChanged(X, Y, world[X, Y].Ci)); 
		}
		
		public void ProduceNewCi ()
		{
			if (_world[X, Y].Ci < _maxCiCount)
			{
				_world[X, Y].Ci = Math.Min(_maxCiCount, _world[X, Y].Ci + _producingSpeed);
				_eventsHistory.Add(new GameEventWorldCiChanged(X, Y, _world[X, Y].Ci)); 
			}	
		}
	}
}
