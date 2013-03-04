using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarSpot.Cloud.Storage;
using System.Reflection;
using WarSpot.Contracts.Service;

namespace WarSpot.Security
{
    public static class DllReferenceHandler
    {
        private static int referenceLevelDeep; 
        private static List<AssemblyName> currentReferenceLevel;
        private static List<string> illegalReferences = Warehouse.GetListOfIllegalReferences();

        public static ErrorCode ReferenceAnalyzer(Assembly Dll)
        {
            currentReferenceLevel = Dll.GetReferencedAssemblies().ToList<AssemblyName>();
            referenceLevelDeep = 1;
            while (currentReferenceLevel.Any<AssemblyName>())
            {
                List<AssemblyName> nextReferenceLevel = GetNextReferenceLevel();

                foreach (AssemblyName referenceName in currentReferenceLevel)
                {
                    if (illegalReferences.Contains(referenceName.Name))
                    {
                        return new ErrorCode(ErrorType.IllegalReference, "Dll references illegal dll with name " + referenceName.Name + " at " + referenceLevelDeep + " reference level.");
                    }
                }

                currentReferenceLevel = nextReferenceLevel;
                referenceLevelDeep++;
            }

            return new ErrorCode(ErrorType.Ok, "Dll references allowed dll's.");
        }


        private static List<AssemblyName> GetNextReferenceLevel()
        {
            List<AssemblyName> result = new List<AssemblyName>();
            foreach (AssemblyName referenceName in currentReferenceLevel)
            {
                foreach (AssemblyName nextreferenceName in (Assembly.Load(referenceName)).GetReferencedAssemblies())
                {
                    result.Add(nextreferenceName);
                }
            }
            return result;
        }
    }
}
