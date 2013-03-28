using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using WarSpot.Contracts.Service;

namespace WarSpot.Security
{
    class DllMethodsHandler
    {
        
        public static ErrorCode AnalyzeDllMethods(Assembly Dll)
        {
            // TO DO: Или список типов. Спросить у СЮ.
            Type Class = Dll.GetType();

            List<MethodInfo> Methods = Class.GetMethods().ToList<MethodInfo>();
              
            foreach (MethodInfo method in Methods)
            {
                //TO DO: Возвратить все keywords метода или возвратить IsUnsafe
                if (IsUnsafe(method))
                    return new ErrorCode(ErrorType.IllegalMethod, "Method " + method.Name + " is unsafe.");
            }

            return NotImplementedException();
        }

        private static ErrorCode NotImplementedException()
        {
            throw new NotImplementedException();
        }

        public static bool IsUnsafe(MethodInfo methodInfo)
        {
            if (HasUnsafeParameters(methodInfo))
            {
                return true;
            }

            return methodInfo.ReturnType.IsPointer;
        }

        private static bool HasUnsafeParameters(MethodInfo methodBase)
        {
            List<ParameterInfo> parameters = methodBase.GetParameters().ToList<ParameterInfo>();
            bool hasUnsafe = parameters.Any(p => p.ParameterType.IsPointer);

            return hasUnsafe;
        }

    }
}
