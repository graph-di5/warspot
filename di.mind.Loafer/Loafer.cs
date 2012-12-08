using System;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace di.minds.Loafer
{
    class Loafer : IBeingInterface
    {
        public float[,] MemorizedArea;//Для запоминания количества энергии вокруг.
        private int LastShiftX;//Для запоминания, куда пытался идти.
        private int LastShiftY;
        private float PreCi;//Для проверки, шагнул ли.
        private int DesiredX;//Куда хочем пойти.
        private int DesiredY;

        private void MemoryUpdate(WorldInfo area, int shiftX, int shiftY)
        {
            int memoryRadius = MemorizedArea.GetLength(0);
            var tempArea = new float[memoryRadius, memoryRadius];
            #region Если двинулись, сдвигаем соответственно запомненную карту
            if ((shiftX !=0) & (shiftY !=0))//Если двинулся
            {
                for (int a = 0; a < memoryRadius; a++)//Сдвигает запомненную карту.
                {
                    for (int b = 0; b < memoryRadius; b++)
                    {

                        if ((a + shiftX > memoryRadius) | (a + shiftX < 0) | (b + shiftY > memoryRadius) | (b + shiftY < 0))
                        {
                            tempArea[a, b] = 0;
                        }
                        else
                        {
                            tempArea[a, b] = MemorizedArea[a + shiftX, a + shiftY];
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
            LastShiftX = 0;
            LastShiftY = 0;
            PreCi = 0.0f;
            DesiredX = 0;
            DesiredY = 0;

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

            MemoryUpdate(area, LastShiftX, LastShiftY);//Обновляем карту памяти.

            if (((DesiredX != 0) | (DesiredY != 0)) & (characteristics.Ci - PreCi <= characteristics.MaxHealth * (Math.Abs(LastShiftX) + Math.Abs(LastShiftY)) / 100))
            {//Если получилось шагнуть (энергия за хождение снялась), значит сместились. Желаемые координаты стали ближе.
                DesiredX -= characteristics.X - LastShiftX;
                DesiredY -= characteristics.Y - LastShiftY;
                PreCi = characteristics.Ci;
                LastShiftX = 0;
                LastShiftY = 0;
            }

            /*План действий:
            Если может, рожает.
            Иначе, если хотел куда-то идти--идёт туда.
            Если не хотел, и есть, что поесть--ест.
            Если и поесть нечего--ищет, куда бы пойти.*/

            if (characteristics.Ci >= characteristics.MaxHealth * 0.97f)
            {//Если кто-то объелся, ему ничего не хочется, только отложить личинку. В прямом смысле.
                LastShiftX = 0;
                LastShiftY = 0;
                DesiredX = 0;
                DesiredY = 0;

                return new GameActionMakeOffspring(characteristics.Id, characteristics.MaxHealth * 0.95f);
            }

            else if ((DesiredX == 0) & (DesiredY == 0) & (area[0,0].Ci != 0))
            {
                return new GameActionEat(characteristics.Id);
            }
            
            #region Обновление карты интереса. Поиск самой интересной точки.
            
            float maxInterest = 0;

            for (int a = 0; a < characteristics.MaxSeeDistance * 4 + 1; a++)
            {
                for (int b = 0; b < characteristics.MaxSeeDistance * 4 + 1; b++)
                {//Считаем проффит: энергия в клетке - трудозатраты на достижение клетки.
                    MapOfInterest[a, b] = MemorizedArea[a, b] - characteristics.MaxStep * characteristics.MaxHealth / 100 * 
                        (Math.Abs(a - (characteristics.MaxSeeDistance*2+1))+Math.Abs(b - (characteristics.MaxSeeDistance*2+1)));//Расстояние считается до центральной клетки.
                    if(MapOfInterest[a, b] > maxInterest)
                    {
                        maxInterest = MapOfInterest[a, b];
                        DesiredX = a - (characteristics.MaxSeeDistance * 2 + 1);//Куда хочется пойти относительно текущего положения.
                        DesiredY = b - (characteristics.MaxSeeDistance * 2 + 1);
                    }
                }
            }
            #endregion
            #region Идём к интересной точке

            PreCi = characteristics.Ci;//Запоминает, сколько было энергии, чтобы знать, шаглул ли.

            if ((characteristics.MaxStep >= 1) & (characteristics.Ci > characteristics.MaxHealth * 0.01f))
            {
                LastShiftX = Math.Sign(DesiredX);
            }

            if ((characteristics.MaxStep - Math.Abs(DesiredX) >= 1) & (characteristics.Ci > (1.0f + Math.Abs(DesiredX)) * characteristics.MaxHealth * 0.01f))
            {
                LastShiftY = Math.Sign(DesiredY);
            }          

            return new GameActionMove(characteristics.Id, LastShiftX, LastShiftY);
            #endregion
        }
    }
}
