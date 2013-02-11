﻿using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace WarSpot.SampleIntellect
{
	public class MainIntellectClass : IBeingInterface
	{
		// Here you can place your own varables, constants or functions.

		/// <summary>
		/// This method is called on every being creation. Your intellect should distribute Ci to being abilities.
		/// </summary>
		/// <param name="step">Step of the match</param>
		/// <param name="ci">Ci available for the constructing being.</param>
		/// <returns>Desired characteristic of the future being.</returns>
		public BeingCharacteristics Construct(ulong step, float ci)
		{
			// small simple being with low characteristics
			return new BeingCharacteristics(100, 1, 1);
		}

		/// <summary>
		/// Main function of the being. Here your code must make decision what the being shoud do.
		/// </summary>
		/// <param name="step">Current game step</param>
		/// <param name="characteristics">Currnet characteristics of this being.</param>
		/// <param name="area">Information about world in the visiable area.</param>
		/// <returns>GameAction that represents decision.</returns>
		public GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
		{
			// if there's some Ci in current world cell we can eat it 
			if(area[0, 0].Ci > 0)
			{
				return new GameActionEat(characteristics.Id);
			}
			// here you can place a lot of decesions 

			// default action
			return new GameActionMove(characteristics.Id, 1, 0);
		}
	}
}
