using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WarSpot.MetroClient.ViewModel
{
    public class ReplayViewModel
    {
        const int MaximumSpeedConst = 16;

        const int MinimumSpeedConst = 1;

        public void LoadReplay(byte[] replay)
        {
            DataContractSerializer bf = new DataContractSerializer(typeof(Version));
        }

        public ICommand NextTurnCommand
        {
            get
            {
                return null;
            }
        }

        public ICommand PreviousTurnCommand
        {
            get
            {
                return null;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return null;
            }
        }

        public ICommand StartCommand
        {
            get
            {
                return null;
            }
        }

        public int MaximumSpeed
        {
            get
            {
                return MaximumSpeedConst;
            }
        }

        public int MinimumSpeed
        {
            get
            {
                return MinimumSpeedConst;
            }
        }

        public int CurrentSpeed
        {
            get;
            set;
        }
    }
}
