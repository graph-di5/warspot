using System;
using WarSpot.Contracts.Service;

namespace WarSpot.Contracts.Intellect
{
	public class WorldCellInfo
	{
		/// <summary>
		/// Amount of the Ci energy in the cell
		/// </summary>
		public float Ci;
		/// <summary>
		/// null if the cell is empty
		/// </summary>
		public BeingCharacteristics BeingCharacteristics;
	}


	public class WorldInfo
	{
		private readonly WorldCellInfo[,] _mapPart;
		private readonly int _distance;
		public int Distance { get { return _distance; } }
		public int Length { get { return _distance * 2 + 1; } }
		public WorldInfo(int distance)
		{
			_distance = distance;
			_mapPart = new WorldCellInfo[Length, Length];
		}

		public WorldCellInfo this[int x, int y] //Перегрузка индексатора
		{
			set
			{
				if (x < -_distance || x > _distance || y < -_distance || y > _distance)
				{
					throw new IndexOutOfRangeException();
				}
				_mapPart[x + _distance, y + _distance] = value;
			}
			get
			{
				if (x < -_distance || x > _distance || y < -_distance || y > _distance)
				{
					throw new IndexOutOfRangeException();
				}
				return _mapPart[x + _distance, y + _distance];
			}
		}
	}
}
