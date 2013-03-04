using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WarSpot.Contracts.Service;

namespace WarSpot.Security
{
    public static class DllVerificationHandler
    {

        public static void Execute(byte[] intellect)
        {
            Assembly Dll = Assembly.Load(intellect);

            if (DllReferenceHandler.ReferenceAnalyzer(Dll).Type == ErrorType.IllegalReference)
                return;
        }


    }
}
