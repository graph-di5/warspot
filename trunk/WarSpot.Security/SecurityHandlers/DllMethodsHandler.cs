using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using WarSpot.Contracts.Service;
using System.Security.Permissions;

namespace WarSpot.Security
{
    class DllMethodsHandler
    {
        
        public ErrorCode AnalyzeDllMethods(Assembly Dll)
        {
            // Проверяем атрибуты библиотеки.
            try
            {
                // Ищем атрибур SecurityPermission.
                Attribute DllSecurityAttribute = Attribute.GetCustomAttribute(Dll, typeof(SecurityPermissionAttribute));
                if (((SecurityPermissionAttribute)DllSecurityAttribute).SkipVerification)
                {
                    return new ErrorCode(ErrorType.IllegalDll, "Dll has unsafe-code.");
                }
            }
            // Для отладки
            catch (Exception e)
            {
                if (e is AmbiguousMatchException)
                {

                }
                if (e is ArgumentException)
                {

                }
            }
            
            // Гарантирует ли отсутствие атрибута SecurityPermission отсутствие unsafe-code? Стоит ли оставлять эту проверку? 
            /*
            // TO DO: Или список типов. Спросить у СЮ.
            Type Class = Dll.GetType();

            List<MethodInfo> Methods = Class.GetMethods().ToList<MethodInfo>();
              
            foreach (MethodInfo method in Methods)
            {
                //TO DO: Возвратить все keywords метода или возвратить IsUnsafe

                if (IsUnsafe(method))
                    return new ErrorCode(ErrorType.IllegalMethod, "Method " + method.Name + " is unsafe.");
            }
             */

            return new ErrorCode(ErrorType.Ok);
        }        

        /*
        private static bool IsUnsafe(MethodInfo methodInfo)
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
         */

    }
}
