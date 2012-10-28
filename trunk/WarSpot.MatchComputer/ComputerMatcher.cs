﻿using System.Collections.Generic;
using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.MatchComputer
{
	class ComputerMatcher
	{
		private List<Being> _objects;
		private List<GameAction> _actions;
		private ulong _step;

		ComputerMatcher()
		{
			_objects = new List<Being>();
			_actions = new List<GameAction>();
			_step = 0;
		}

		public void Update()
		{
			_actions.Clear();
			_step++;
			//Obtaining new actions from beings
			foreach (var curObject in _objects)
			{
				// todo send NOT NULL world info
				_actions.Add(curObject.Think(_step, curObject.Characteristics, null));
			}

			//Doing somethink with received actions
			/*
			foreach (var curAction in _actions)
			{
				curAction.Execute();
				// todo save action if succeded
			}
			*/

			_actions.Clear();
		}
	}
}
