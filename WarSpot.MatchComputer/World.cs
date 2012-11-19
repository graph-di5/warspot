namespace WarSpot.MatchComputer
{
	/// <summary>
	/// main World class
	/// </summary>
	class World
	{
		/// <summary>
		/// Обращение к ячейке мира [x,y].
		/// </summary>
		public WorldCell this[int x, int y]//Перегрузка индексатора
		{
			set
			{
#if true
				while (x < 0)
				{
					x += Width;
				}
				x %= Width;
				while (y < 0)
				{
					y += Height;
				}
				y %= Height;
				Map[x, y] = value;
#else
				int _x, _y;

				if (x > this.Width)
				{
					_x = x % this.Width;//Вычитаем лишнее				
				}
				else if (x < 0)
				{
					_x = this.Width - (x % this.Width);
				}
				else
				{
					_x = x;
				}

				if (y > this.Height)
				{
					_y = y % this.Height;
				}
				else if (y < 0)
				{
					_y = this.Height - (y % this.Height);
				}
				else
				{
					_y = y;
				}
				
				this.Map[_x, _y] = value;//Обращаемся к нужной точке.
#endif
			}
			get
			{
#if true
				while (x < 0)
				{
					x += Width;
				}
				x %= Width;
				while (y < 0)
				{
					y += Height;
				}
				y %= Height;
				return Map[x, y];
#else
				int _x, _y;

				if (x > this.Width)
				{
					_x = x % this.Width;
				}
				else if (x < 0)
				{
					_x = this.Width - (x % this.Width);
				}
				else
				{
					_x = x;
				}

				if (y > this.Height)
				{
					_y = y % this.Height;
				}
				else if (y < 0)
				{
					_y = this.Height - (y % this.Height);
				}
				else
				{
					_y = y;
				}
				return this.Map[_x, _y];
#endif
			}
		}

		//
		private WorldCell[,] Map { get; set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		public World()
		{
			Width = 100;
			Height = 70;
			Map = new WorldCell[Width, Height];
		}
	}
}
