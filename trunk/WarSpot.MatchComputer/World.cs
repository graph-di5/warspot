namespace WarSpot.MatchComputer
{
	/// <summary>
	/// main World class
	/// </summary>
	class World
	{
		/// <summary>
		/// ��������� � ������ ���� [x,y].
		/// </summary>
		public WorldCell this[int x, int y]//���������� �����������
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
					_x = x % this.Width;//�������� ������				
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
				
				this.Map[_x, _y] = value;//���������� � ������ �����.
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
		/// �����������
		/// </summary>
		public World()
		{
			Width = 32;
			Height = 24;
			Map = new WorldCell[Width, Height];
			for (var i = 0; i < Width; i++)
			{
				for (var j = 0; j < Height; j++)
				{
					Map[i, j] = new WorldCell();
				}
			}
		}
	}
}
