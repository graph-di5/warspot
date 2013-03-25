using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Contracts.Intellect;
using System.Reflection;
using System.Security.Permissions;
using System.Security.CodeAccessPermission;

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

        [FileIOPermissionAttribute(SecurityAction.PermitOnly, AllFiles=FileIOPermissionAccess.NoAccess),
            ReflectionPermissionAttribute(SecurityAction.PermitOnly, RestrictedMemberAccess = true),
                EnvironmentPermission(SecurityAction.PermitOnly, All = "TO DO: имя переменных"),
                     SecurityPermission(SecurityAction.PermitOnly, Flags = SecurityPermissionFlag.AllFlags, ControlAppDomain = false, ControlThread = false, Infrastructure = false)]
        public BeingCharacteristics Construct(ulong step, float ci)
        {
            return this.intellect.Construct(step, ci); //Таким образом, вызываем метод Construct у проверяемой библиотеки, 
                                                       //с уже навешенной проверкой безопасности.
        }

        [FileIOPermissionAttribute(SecurityAction.PermitOnly, AllFiles = FileIOPermissionAccess.NoAccess),
            ReflectionPermissionAttribute(SecurityAction.PermitOnly, RestrictedMemberAccess = true),
                EnvironmentPermission(SecurityAction.PermitOnly, All = "TO DO: имя переменных"),
                     SecurityPermission(SecurityAction.PermitOnly, Flags = SecurityPermissionFlag.AllFlags, ControlAppDomain = false, ControlThread = false, Infrastructure = false)]
        public Contracts.Intellect.Actions.GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            return this.intellect.Think(step, characteristics, area);
        }
    }
}
