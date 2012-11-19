namespace WarSpot.MatchComputer
{
	/// <summary>
	/// main World class
	/// </summary>
	class World
	{
		/// <summary>
		///Обращение к ячейке мира [x,y].
		/// </summary>
		public WorldCell this[int x, int y]//Перегрузка индексатора
		{
			set
			{
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
			}
			get
			{
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
			}
		}

		private WorldCell[,] _content;

		public WorldCell[,] Map
		{
			get
			{
				return _content;
			}
		}
		/// <summary>
		/// Width
		/// </summary>
		private readonly int _w;
		public int Width
		{
			get
			{
				return _w;
			}
		}

		/// <summary>
		/// Height
		/// </summary>
		private readonly int _h;
		public int Height
		{
			get
			{
				return _h;
			}
		}
		/// <summary>
		/// Конструктор
		/// </summary>
		public World()
		{
			_w = 100;
			_h = 70;
			_content = new WorldCell[_w, _h];
		}
	}
}
