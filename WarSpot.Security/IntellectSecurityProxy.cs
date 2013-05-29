using System;
using System.Linq;
using WarSpot.Contracts.Intellect;
using System.Reflection;
using System.Net;
using System.Security.Permissions;

namespace WarSpot.Security
{
    public class IntellectSecurityProxy : MarshalByRefObject, IBeingInterface
    {
        // 
        private IBeingInterface _intellect;

        public IntellectSecurityProxy()
        {
            byte[] assembly = (byte[])AppDomain.CurrentDomain.GetData("Intellect");

            var loadedAssembly = Assembly.Load(assembly);

            var types = loadedAssembly.GetTypes();
            var type = types.First(x => x.GetInterface("IBeingInterface") != null);
            _intellect = (IBeingInterface) loadedAssembly.CreateInstance(type.FullName);
        }


        [FileIOPermissionAttribute(SecurityAction.Deny),
            ReflectionPermissionAttribute(SecurityAction.Deny),
                     SecurityPermissionAttribute(SecurityAction.PermitOnly, Execution = true),
                            SocketPermission(SecurityAction.Deny)]
        public BeingCharacteristics Construct(ulong step, float ci)
        {
            BeingCharacteristics result = _intellect.Construct(step, ci);
            return result; //Таким образом, вызываем метод Construct у проверяемой библиотеки, 
                                                       //с уже навешенной проверкой безопасности.
        }

        [FileIOPermissionAttribute(SecurityAction.Deny),
            ReflectionPermissionAttribute(SecurityAction.Deny),
                     SecurityPermissionAttribute(SecurityAction.PermitOnly, Execution = true),
                            SocketPermissionAttribute(SecurityAction.Deny)]
        public Contracts.Intellect.Actions.GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            return _intellect.Think(step, characteristics, area);
        }
    }
}
