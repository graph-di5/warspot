using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace WarSpot.MetroClient.ViewModel
{
    public class TurnViewModel
    {
        public TurnViewModel()
        {
            CreaturePositions = new Dictionary<int, List<Point>>();
        }

        public Dictionary<int, List<Point>> CreaturePositions
        {
            get;
            set;
        }
    }
}
