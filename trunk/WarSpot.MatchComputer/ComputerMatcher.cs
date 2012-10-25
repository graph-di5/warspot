using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.Cloud.MatchComputer
{
	class ComputerMatcher
	{
		private List <IBeingInterface> _objects;
		private List <GameAction> _actions;

		ComputerMatcher ()
		{
			_objects = new List<IBeingInterface>();
			_actions= new List <GameAction>();
		}

		public void Update ()
		{
			lock (_objects)
			{
				//Obtaining new actions from beings
				foreach (var curObject in _objects)
				{
					_actions.Add(curObject.Think());
				}

				//Doing somethink with received actions
				/*
				foreach (var curAction in _actions)
				{
					curAction.Execute();
				}
				*/
 
				_actions.Clear();
			}	
		}
	}
}
