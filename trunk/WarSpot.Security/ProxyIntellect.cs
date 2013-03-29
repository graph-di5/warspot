using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Contracts.Intellect;
using System.Reflection;
using System.Net;
using System.Security.Permissions;

namespace WarSpot.Security
{
    public class ProxyIntellect : IBeingInterface
    {
        // 
        private IBeingInterface intellect;

        public ProxyIntellect(Assembly dll)
        {
            this.Initialize(dll);
        }
       
        // Этим методом следует иницилизировать поле intellect?
        private void Initialize(Assembly dll)
        {
            this.intellect = AddBeing(dll).First<IBeingInterface>();
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

        [FileIOPermissionAttribute(SecurityAction.Deny, Unrestricted = true),
            ReflectionPermissionAttribute(SecurityAction.Deny, Unrestricted = true),
                EnvironmentPermissionAttribute(SecurityAction.PermitOnly, Read = "TO DO: переменные среды"),
                     SecurityPermissionAttribute(SecurityAction.PermitOnly, Execution = true),
                            SocketPermissionAttribute(SecurityAction.Deny, Unrestricted=true)]
        public BeingCharacteristics Construct(ulong step, float ci)
        {
            return this.intellect.Construct(step, ci); //Таким образом, вызываем метод Construct у проверяемой библиотеки, 
                                                       //с уже навешенной проверкой безопасности.
        }

        [FileIOPermissionAttribute(SecurityAction.Deny, Unrestricted=true),
            ReflectionPermissionAttribute(SecurityAction.Deny, Unrestricted=true),
                EnvironmentPermissionAttribute(SecurityAction.PermitOnly, Read = "TO DO: переменные среды"),
                     SecurityPermissionAttribute(SecurityAction.PermitOnly, Execution = true),                   
                            SocketPermissionAttribute(SecurityAction.Deny, Unrestricted=true)]
        public Contracts.Intellect.Actions.GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            return this.intellect.Think(step, characteristics, area);
        }
    }
}
