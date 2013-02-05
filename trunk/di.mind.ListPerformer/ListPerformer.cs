using System;
using WarSpot.Contracts.Intellect;
using WarSpot.Contracts.Intellect.Actions;

namespace di.mind.ListPerformer
{
    public class ListPerformer : IBeingInterface
    {
        private int taskNumber;

        public BeingCharacteristics Construct(ulong step, float ci)
        {
            taskNumber = 0;

            if (ci >= 100)
            {
                return new BeingCharacteristics(Guid.NewGuid(), 100.0f, 5.0f, 1);
            }
            else
            {
                return new BeingCharacteristics(Guid.NewGuid(), 1.0f, 1.0f, 1);
            }
        }

        public GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            if (taskNumber == 6)
            {
                taskNumber = 1;
            }
            else
            {
                taskNumber++;
            }

            switch (taskNumber)
            {
                case 1:
                    return new GameActionMakeOffspring(characteristics.Id, 10.0f);
                    break;
                case 2:
                    return new GameActionEat(characteristics.Id);
                    break;
                case 3:
                    return new GameActionMove(characteristics.Id, 1, 0);
                    break;
                case 4:
                    return new GameActionMove(characteristics.Id, 0, 1);
                    break;
                case 5:
                    return new GameActionMove(characteristics.Id, -1, 0);
                    break;
                case 6:
                    return new GameActionMove(characteristics.Id, 0, -1);
                    break;

            }
            return new GameActionEat(characteristics.Id);
        }

    }
}
