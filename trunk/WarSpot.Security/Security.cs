using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WarSpot.Contracts.Service;
using WarSpot.Contracts.Intellect;

namespace WarSpot.Security
{
    public class Security : IBeingInterface
    {

        private AppDomain SecurityDomain;
        private string SecurityAssemblyName;
        private Assembly Dll;

        private DllReferenceHandler dllReferenceHandler;
        private DllMethodsHandler dllMethodHandler;
        private ProxyIntellect proxyIntellect;

        public Security(byte[] intellect)
        {
            //BUILDING A PRISON.
            SecurityDomain = AppDomain.CreateDomain("DllPrivateDomain");
            SecurityAssemblyName = Assembly.GetEntryAssembly().FullName;

            //USER INTELLECT! GO TO PRISON UNTIL WE CAN SAY YOU ARE GOOD!
            Dll = SecurityDomain.Load(intellect);

            //CALLING FOR HORROR SECURITY EXECUTERS.
            dllReferenceHandler = (DllReferenceHandler)SecurityDomain.CreateInstanceAndUnwrap(SecurityAssemblyName, typeof(DllReferenceHandler).FullName);
            dllMethodHandler = (DllMethodsHandler)SecurityDomain.CreateInstanceAndUnwrap(SecurityAssemblyName, typeof(DllMethodsHandler).FullName);  

            //USER INTELLECT! GO TO CELL UNTIL WE CAN SAY YOU ARE GOOD!
            proxyIntellect = (ProxyIntellect)SecurityDomain.CreateInstanceAndUnwrap(SecurityAssemblyName, typeof(ProxyIntellect).FullName);
            
        }

        private void CloseSecurityDomain()
        {
            AppDomain.Unload(SecurityDomain);
        }

        public BeingCharacteristics Construct(ulong step, float ci)
        {
            return proxyIntellect.Construct(step, ci);
        }

        public Contracts.Intellect.Actions.GameAction Think(ulong step, BeingCharacteristics characteristics, WorldInfo area)
        {
            return proxyIntellect.Think(0, characteristics, area);
        }
    }
}
