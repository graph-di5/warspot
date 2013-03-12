using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Contracts.Intellect;
using System.Reflection;
using System.Security.Permissions;

namespace WarSpot.Security
{
    public class ProxyIntellect : IBeingInterface
    {
        // 
        private IBeingInterface intellect;

        public ProxyIntellect()
        {
            this.intellect = null;
        }

        
        // Этим методом следует иницилизировать поле intellect?
        public static bool Initialize(Assembly dll)
        {
            // TO DO: Initialize field intellect.
            throw new NotImplementedException();
        }

        public BeingCharacteristics Construct(ulong step, float ci)
        {
            throw new NotImplementedException();
        }


        public Contracts.Intellect.Actions.GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            throw new NotImplementedException();
        }
    }
}
