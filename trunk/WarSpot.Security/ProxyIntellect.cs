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
        public bool Initialize(Assembly dll)
        {
            this.intellect = AddBeing(dll).First<IBeingInterface>();

            if (this.intellect == null)
            {
                return false;
            }
            // TO DO: Initialize field intellect.
            return true;
        }

        private static List<IBeingInterface> AddBeing(Assembly assembly)
        {
            List<IBeingInterface> objects = new List<IBeingInterface>();
            string iMyInterfaceName = typeof(IBeingInterface).ToString();
            TypeDelegator[] defaultConstructorParametersTypes = new TypeDelegator[0];
            object[] defaultConstructorParameters = new object[0];

            IBeingInterface iAI = null;

            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterface(iMyInterfaceName) != null)
                {
                    ConstructorInfo defaultConstructor = type.GetConstructor(defaultConstructorParametersTypes);
                    object instance = defaultConstructor.Invoke(defaultConstructorParameters);
                    iAI = (IBeingInterface)instance;//Достаём таки нужный интерфейс
                    //
                    objects.Add(iAI);
                }
            }
            return objects;
        }

        [WarSpotSecurityPermission(SecurityAction.Demand)] // TO DO: 
        public BeingCharacteristics Construct(ulong step, float ci)
        {
            throw new NotImplementedException();
        }

         // TO DO:
        public Contracts.Intellect.Actions.GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            throw new NotImplementedException();
        }
    }
}
