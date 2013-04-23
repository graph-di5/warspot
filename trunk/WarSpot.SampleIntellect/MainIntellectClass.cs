using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

using System;//You can use outer libraries

namespace WarSpot.SampleIntellect
{
	/// <summary>
	/// This is sample intellect for simple being. 
	/// -------------------------------------------------------------------------
	/// You can change the behavior of this intelligence, 
	/// if this will not break the rules WarSpot.
	/// You can use all algorithms in this class as you whant.
	/// WarSpot team is not responsible for any damage caused by using the 
	/// algorithms used in the class.
	/// -------------------------------------------------------------------------
	/// 
	/// ! DEBUG !
	/// You can debug your class very simple by creating breakpoints 
	/// where you need, and then use Debug->Run and step debugging.
	/// 
	/// </summary>

	//You may create your own classes
	public class Vector
	{
		public int X;
		public int Y;

		public Vector()
		{
			X = 0;
			Y = 0;
		}

		public Vector(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public class MainIntellectClass : IBeingInterface
	{
		// Here you can place your own varables, constants or functions.
		private float stepCost;

		#region Action Methods
		/// <summary>
		/// Moves to the most appetizing cell within reach
		/// </summary>
		///  <param name="characteristics">Being characteristics</param>
		///  <param name="area">Part of the world to looking-for</param>
		/// <returns>Return preferable action </returns>
		private GameAction goEat(BeingCharacteristics characteristics, WorldInfo area)
		{
			Vector _bestCell = new Vector();
			float _bestCellScore = 0.0f;
			int _maxStep = (int)(characteristics.Ci / stepCost);

			if (_maxStep > characteristics.MaxStep)
			{
				_maxStep = (int)characteristics.MaxStep;
			}

			for (int i = -area.Distance; i < area.Distance; i++)
			{
				for (int j = -area.Distance; j < area.Distance; j++)
				{
					int _distance = Math.Abs(i) + Math.Abs(j);
					float _profit = area[i, j].Ci - stepCost * (float)(_distance);

					if (_profit > _bestCellScore && _distance <= _maxStep)
					{
						_bestCellScore = _profit;
						_bestCell.X = i;
						_bestCell.Y = j;
					}
				}
			}

			if (_bestCell.X == 0 && _bestCell.Y == 0)
			{
				return new GameActionEat(characteristics.Id);
			}
			else
			{
				return new GameActionMove(characteristics.Id, _bestCell.X, _bestCell.Y);
			}
		}
		
		/// <summary>
		/// Heal itself
		/// </summary>
		///  <param name="characteristics">Being characteristics</param>
		/// <returns>Return computed action</returns>
		private GameAction selfHeal(BeingCharacteristics characteristics)
		{
			float _ci = 0.0f;
			if (characteristics.Ci >= (characteristics.MaxHealth - characteristics.Health) * 3.0f) //Заметьте, он не проверяет, сколько энергии останется. А она нужна для того, чтобы жить.
			{//Heal itself fully if it can
				_ci = (characteristics.MaxHealth - characteristics.Health) * 3.0f;

				if (_ci > characteristics.Ci * 0.6f)
				{//If more than one permitted then reduces
					_ci = characteristics.Ci * 0.6f;
				}
			}
			else
			{
				_ci = Math.Abs(characteristics.Ci);
			}

			return new GameActionTreat(characteristics.Id, 0, 0, _ci);//In relative coordinates
		}
		#endregion

		/// <summary>
		/// This method is called on every being creation. Your intellect should distribute Ci to being abilities.
		/// </summary>
		/// <param name="step">Step of the match</param>
		/// <param name="ci">Ci available for the constructing being.</param>
		/// <returns>Desired characteristic of the future being.</returns>
		public BeingCharacteristics Construct(ulong step, float ci)
		{
			//Being characteristics may be different. It's you to decide. But remember about the cost.
			return new BeingCharacteristics((ci - 13.0f) / 0.8f, 3.0f, 4);
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
			// Here you can place a lot of decisions 
			if (characteristics.Health < characteristics.MaxHealth && characteristics.Ci > characteristics.MaxHealth * 0.6f)
			{
				return selfHeal(characteristics);
			}
			else if (characteristics.Ci > 13.0f + characteristics.MaxHealth * 0.8f)
			{
				return new GameActionMakeOffspring(characteristics.Id, 13.0f + characteristics.MaxHealth * 0.8f);
			}
			else
			{
				return goEat(characteristics, area);
			}
		}
	}
}
