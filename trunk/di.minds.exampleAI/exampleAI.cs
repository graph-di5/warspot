using System;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace di.minds.exampleAI
{
	//Можно создавать свои классы, и использовать их.
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

	public class exampleAI: IBeingInterface
	{
		 private float stepCost; //Можно что-то запоминать.
		
		/// <summary>
		/// Двигается в самую аппетитную досягаемую за ход точку.
		/// </summary>
		 ///  <param name="characteristics">Характеристики существа</param>
		 ///  <param name="area">Кусок мира для поиска</param>
		/// <returns>Возвращает предпочтительное действие</returns>
		private GameAction goEat(BeingCharacteristics characteristics, WorldInfo area)
		{
			Vector _bestCell = new Vector();
			float _bestCellScore = 0.0f;
			int _maxStep = (int)(characteristics.Ci / stepCost);

			if (_maxStep > characteristics.MaxStep)
			{
				_maxStep = (int)characteristics.MaxStep;//ToDo: Исправить на int, если изменится контракт.
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
		/// Лечит себя.
		/// </summary>
		///  <param name="characteristics">Характеристики существа</param>
		/// <returns>Возвращает рассчитанное действие</returns>
		private GameAction selfHeal(BeingCharacteristics characteristics)
		{
			float _ci = 0.0f;
			if (characteristics.Ci >= (characteristics.MaxHealth - characteristics.Health) * 3.0f) //Заметьте, он не проверяет, сколько энергии останется. А она нужна для того, чтобы жить.
			{//Если энергии хватает--лечит себя полностью.
				_ci = (characteristics.MaxHealth - characteristics.Health) * 3.0f;

				if (_ci > characteristics.Ci * 0.6f)
				{//Если получилось больше разрешённого (В лечение можно вливать <= 60% текущей энергии), уменьшает до разрешённого.
					_ci = characteristics.Ci * 0.6f;
				}
			}
			else
			{
				_ci = Math.Abs(characteristics.Ci);
			}

			return new GameActionTreat(characteristics.Id, 0, 0, _ci);//Лечение "бьёт" по относительным координатам.
		}

		public BeingCharacteristics Construct(ulong step, float ci)
		{//При создании существа вызывается этот метод. В нём стоит определить, какие характеристики должны быть у существа.
			var being = new BeingCharacteristics((ci - 13.0f) / 0.8f, 3.0f, 4);//Стоимость получения характеристик смотри в правилах игры.
			stepCost = being.MaxHealth / 100.0f;//В нём же можно инициализировать все свойства.
			return being;
		}

		public GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
		{//Каждый ход у каждого существа вызывается данный метод. Метод должен вернуть желаемое действие.
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
