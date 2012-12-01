using System;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace di.minds.Loafer
{
    class Loafer : IBeingInterface
    {
        public float[,] MemorizedArea;//Для запоминания количества энергии вокруг.
        private int PreX;//Для запоминания, где мы были
        private int PreY;
        private int DesiredX;//Куда хочем пойти
        private int DesiredY;

        private void MemoryUpdate(IWorldCell[,] area, int shiftX, int shiftY)
        {
            int memoryRadius = MemorizedArea.GetLength(0);
            var tempArea = new float[memoryRadius, memoryRadius];
            #region Если двинулись, сдвигаем соответственно запомненную карту
            if ((shiftX !=0) & (shiftY !=0))//Если двинулись
            {
                for (int a = 0; a < memoryRadius; a++)//Сдвигаем запомненную карту.
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

            for (int a = 0; a < seeDistance*2+1; a++)//Дописываем в память увиденное в этом ходу.
            {
                for (int b = 0; b < seeDistance * 2 + 1; b++)
                {
                    tempArea[a + seeDistance, b + seeDistance] = area[a, b].Ci;
                }
            }

            MemorizedArea = tempArea;//Вот и обновили память.
        }

        public BeingCharacteristics Construct(ulong step, float ci)
        {
            PreX = -1;
            PreY = -1;
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

        public GameAction Think(ulong step, BeingCharacteristics characteristics, IWorldCell[,] area)
        {
            float[,] MapOfInterest = new float[characteristics.MaxSeeDistance * 4 + 1, characteristics.MaxSeeDistance * 4 + 1];//Будем думать, куда лучше пойти.
            MapOfInterest.Initialize();

            MemoryUpdate(area, characteristics.X - PreX, characteristics.Y - PreY);//Обновляем карту памяти.

            DesiredX -= characteristics.X - PreX;
            DesiredY -= characteristics.Y - PreY;

            if (characteristics.Ci >= characteristics.MaxHealth * 0.97f)
            {
                PreX = characteristics.X;//Не сдвигаемся. Пишем координаты для использования в следующем году.
                PreY = characteristics.Y;

                return new GameActionMakeOffspring(characteristics.Id, characteristics.MaxHealth * 0.95f);
            }
            
            else if ((DesiredX == 0) & (DesiredY == 0) & (area[0,0].Ci != 0))
            {
                return new GameActionEat(characteristics.Id);
            }

            DesiredX = 0;
            DesiredY = 0;

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
                        DesiredX = a;//Пишем пока сюда.
                        DesiredY = b;
                    }
                }
            }

            DesiredX -= characteristics.MaxSeeDistance * 2 - 1;
            DesiredY -= characteristics.MaxSeeDistance * 2 - 1;//Переписываем в относительные координаты.
            #endregion
            #region Идём к интересной точке
            PreX = characteristics.X;
            PreY = characteristics.Y;

            if (Math.Abs(DesiredX) < characteristics.MaxStep)
            {
                
                if (characteristics.MaxStep - Math.Abs(DesiredX) >= Math.Abs(DesiredY))
                {
                    return new GameActionMove(characteristics.Id, DesiredX, DesiredY);
                }
                else
                {
                    return new GameActionMove(characteristics.Id, DesiredX, ((int)characteristics.MaxStep - Math.Abs(DesiredX)) * Math.Sign(DesiredY));
                }
            }
            else
            {
                return new GameActionMove(characteristics.Id, Math.Sign(DesiredX)*(int)characteristics.MaxStep, 0);
            }
            #endregion
        }
    }
}
