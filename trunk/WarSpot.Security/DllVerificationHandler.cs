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
    public static class DllVerificationHandler
    {

        public static void Execute(byte[] intellect)
        {
               
            AppDomain DllPrivateDomain = AppDomain.CreateDomain("DllPrivateDomain");
            // SecurityAssemblyName = WarSpot.Security
            string SecurityAssemblyName = Assembly.GetEntryAssembly().FullName;

            Assembly Dll = DllPrivateDomain.Load(intellect);

            DllReferenceHandler dllReferenceHandler = (DllReferenceHandler)DllPrivateDomain.CreateInstanceAndUnwrap(SecurityAssemblyName, typeof(DllReferenceHandler).FullName);
            DllMethodsHandler dllMethodHandler = (DllMethodsHandler)DllPrivateDomain.CreateInstanceAndUnwrap(SecurityAssemblyName, typeof(DllMethodsHandler).FullName);
            
            if (dllReferenceHandler.AnalyzeDllReferences(Dll).Type == ErrorType.IllegalReference || dllMethodHandler.AnalyzeDllMethods(Dll).Type == ErrorType.IllegalMethod)
                return;

            ProxyIntellect proxyIntellect = (ProxyIntellect)DllPrivateDomain.CreateInstanceAndUnwrap(SecurityAssemblyName, typeof(ProxyIntellect).FullName);

            if (proxyIntellect.SecurityChecking().Type == ErrorType.IllegalDll)
                return;

            // Удаляем DllPrivateDomain из списка доменов.
            AppDomain.Unload(DllPrivateDomain);

        }


    }
}
