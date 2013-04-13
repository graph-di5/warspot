using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Contracts.Intellect;
using System.Reflection;
using System.Net;
using System.Security.Permissions;
using System.Threading;
using WarSpot.Contracts.Service;

namespace WarSpot.Security
{
    public class ProxyIntellect : MarshalByRefObject, IBeingInterface
    {
        // 
        private IBeingInterface intellect;

        public ProxyIntellect()
        {
            Type t = typeof(IBeingInterface);

            this.intellect = (IBeingInterface)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(AppDomain.CurrentDomain.GetAssemblies().First<Assembly>().FullName, t.FullName);  
        }

        private ErrorCode SecurityChecking()
        {
            try
            {
                this.Construct(0, 0);
                this.Think(0, new BeingCharacteristics(), new WorldInfo(0));
            }
            catch (Exception e)
            {
                return new ErrorCode(ErrorType.IllegalDll, "Exception with that message: " + e.Message + " has been cought.");
            }

            return new ErrorCode(ErrorType.Ok, "No security permissions or exceptions have been thrown.");
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
