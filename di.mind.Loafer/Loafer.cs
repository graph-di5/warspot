using System;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace di.minds.Loafer
{
    class Loafer : IBeingInterface
    {
        public float[,] MemorizedArea;//Для запоминания количества энергии вокруг.
        private int PreX;
        private int PreY;
        private int DesiredX;//Куда хочем пойти.
        private int DesiredY;
        private bool Semental;//Будет ли рожать мощнее себя, или ограничится меньшими. 

        private void MemoryUpdate(WorldInfo area, int x, int y)
        {
            int memoryRadius = MemorizedArea.GetLength(0);
            var tempArea = new float[memoryRadius, memoryRadius];
            #region Если двинулись, сдвигаем соответственно запомненную карту
            if ((PreX != x) & (PreY != y))//Если двинулся
            {
                int _shiftX = x - PreX;
                int _shiftY = y - PreY;
                for (int a = 0; a < memoryRadius; a++)//Сдвигает запомненную карту.
                {
                    for (int b = 0; b < memoryRadius; b++)
                    {

                        if ((a + _shiftX > memoryRadius) | (a + _shiftX < 0) | (b + _shiftY > memoryRadius) | (b + _shiftY < 0))
                        {
                            tempArea[a, b] = 0;
                        }
                        else
                        {
                            tempArea[a, b] = MemorizedArea[a + _shiftX, a + _shiftY];
                        }
                    }
                }
            }
            #endregion

            int seeDistance = (memoryRadius - 1) / 4;

						for (int a = -seeDistance; a <= seeDistance; a++)//Дописываем в память увиденное в этом ходу.
            {
							for (int b = -seeDistance; b <= seeDistance; b++)
                {
                    tempArea[a + seeDistance, b + seeDistance] = area[a, b].Ci;
                }
            }

            MemorizedArea = tempArea;//Вот и обновили память.
        }

        public BeingCharacteristics Construct(ulong step, float ci)
        {
            PreX = -1;//Пока не знаю, что с ними делать в таком случае.
            PreY = -1;
            DesiredX = 0;
            DesiredY = 0;
            Random _rnd = new Random();

            if (_rnd.Next(5) < 2) //2 из 6 будут рождать существ мощнее себя.
            {
                Semental = true;
            }
            else
            {
                Semental = false;
            }

            if (ci<10)
            {//Не родится--и фиг с ним.
                MemorizedArea = new float[5,5];//Помним больше, чем видим.
                MemorizedArea.Initialize();                
                return new BeingCharacteristics(Guid.NewGuid(), (ci - 1.25f) / 0.8f, 1.0f, 1);
            }
            else if (ci < 30)
            {//4 на ход, 1 на зрение
                MemorizedArea = new float[9, 9];
                MemorizedArea.Initialize();
                return new BeingCharacteristics(Guid.NewGuid(), (ci - 5.0f) / 0.8f , 2.0f, 2);
            }
            else if (ci < 70)
            {//9 на ход, 4 на зрение
                MemorizedArea = new float[17, 17];
                MemorizedArea.Initialize();
                return new BeingCharacteristics(Guid.NewGuid(), (ci - 13.0f) / 0.8f, 3.0f, 4);
            }
            else
            {//16 на ход, 9 на зрение
                MemorizedArea = new float[25, 25];
                MemorizedArea.Initialize();
                return new BeingCharacteristics(Guid.NewGuid(), (ci - 13.0f) / 0.8f, 4.0f, 6);
            }
        }

        public GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            float[,] MapOfInterest = new float[characteristics.MaxSeeDistance * 4 + 1, characteristics.MaxSeeDistance * 4 + 1];//Будем думать, куда лучше пойти.
            MapOfInterest.Initialize();

            if ((PreX == -1) & (PreY == -1))
            {
                PreX = characteristics.X;
                PreY = characteristics.Y;
            }
          
            MemoryUpdate(area, characteristics.X, characteristics.Y);//Обновляем карту памяти.            

            if ((DesiredX != characteristics.X) | (DesiredY != characteristics.Y))
            {
                PreX = characteristics.X;
                PreY = characteristics.Y;
            }

            /*План действий:
            Если может, рожает.
            Если дошёл, куда хотел, и есть, что поесть--ест.
            Если и поесть нечего--ищет, куда бы пойти.
            Иначе, если хотел куда-то идти--идёт туда.*/

            #region 1.Рожает, если может
            if ((Semental == true) & (characteristics.Ci >= characteristics.MaxHealth * 0.97f))
            {
                DesiredX = characteristics.X;
                DesiredY = characteristics.Y;

                return new GameActionMakeOffspring(characteristics.Id, characteristics.MaxHealth * 0.95f);
            }

            else if ((Semental == false) & (characteristics.Ci >= characteristics.MaxHealth * 0.6f))
            {
                DesiredX = characteristics.X;
                DesiredY = characteristics.Y;

                return new GameActionMakeOffspring(characteristics.Id, characteristics.MaxHealth * 0.5f);
            }
            #endregion
            #region 2.Если дошёл, и есть, что поесть--ест.
            else if ((DesiredX == characteristics.X) & (DesiredY == characteristics.Y) & (area[0, 0].Ci != 0))
            {
                return new GameActionEat(characteristics.Id);
            }
            #endregion
            #region 3.Обновление карты интереса. Поиск самой интересной точки.    
            else if ((DesiredX == characteristics.X) & (DesiredY == characteristics.Y))
            {
                for (int a = 0; a < characteristics.MaxSeeDistance * 4 + 1; a++)
                {
                    float _maxInterest = 0;
                    for (int b = 0; b < characteristics.MaxSeeDistance * 4 + 1; b++)
                    {//Считаем проффит: энергия в клетке - трудозатраты на достижение клетки.
                        MapOfInterest[a, b] = MemorizedArea[a, b] - characteristics.MaxStep * characteristics.MaxHealth / 100 * 
                            (Math.Abs(a - (characteristics.MaxSeeDistance*2+1))+Math.Abs(b - (characteristics.MaxSeeDistance*2+1)));//Расстояние считается до центральной клетки.
                        if(MapOfInterest[a, b] > _maxInterest)
                        {
                            _maxInterest = MapOfInterest[a, b];
                            DesiredX = a - (characteristics.MaxSeeDistance * 2 + 1) + characteristics.X;//Куда хочется пойти.
                            DesiredY = b - (characteristics.MaxSeeDistance * 2 + 1) + characteristics.Y;
                        }
                    }
                }
            }
            #endregion
            #region 4.Идёт к интересной точке            
            PreX = characteristics.X;
            PreY = characteristics.Y;

            //Если Х совпадает, идёт по Y. Если нет, пытается по обоим. Не получается--идёт по Y. Если совсем всё плохо--ест.
           
            if ((DesiredX == characteristics.X) & (characteristics.MaxStep >= 1) & (characteristics.Ci > characteristics.MaxHealth * 0.01f))
            {
                return new GameActionMove(characteristics.Id, 0, Math.Sign(DesiredY - characteristics.Y)); 
            }

            else if ((characteristics.MaxStep >= 2) & (characteristics.Ci > 2.0f * characteristics.MaxHealth * 0.01f))
            {
                return new GameActionMove(characteristics.Id, Math.Sign(DesiredX - characteristics.X), Math.Sign(DesiredY - characteristics.Y));  
            }

            else if ((characteristics.MaxStep >= 1) & (characteristics.Ci > characteristics.MaxHealth * 0.01f))
            {
                return new GameActionMove(characteristics.Id, Math.Sign(DesiredX - characteristics.X), 0);  
            }

            else
            {
                return new GameActionEat(characteristics.Id);
            }
            #endregion
        }
    }
}
