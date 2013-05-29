using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using WarSpot.Contracts.Service;
using WarSpot.Cloud.Storage;
using System.Security.Permissions;

namespace WarSpot.Security
{
    public class IntellectStaticSecurityChecking
    {
        private static List<AssemblyName> currentReferenceLevel;
        private static List<string> illegalReferences = new List<string>();       

        public static ErrorCode StaticSecurityChecking(byte[] intellect)
        {
            ErrorCode result;

            Assembly dll = Assembly.Load(intellect);

            if ((result = AnalyzeDllMethods(dll)).Type == ErrorType.IllegalDll || (result = AnalyzeDllReferences(dll)).Type == ErrorType.IllegalDll)
            {
                return result;
            }
            else
                return result;
        }

        public static ErrorCode AnalyzeDllReferences(Assembly Dll)
        {
            currentReferenceLevel = Dll.GetReferencedAssemblies().ToList<AssemblyName>();

            foreach (AssemblyName referenceName in currentReferenceLevel)
            {
                if (illegalReferences.Contains(referenceName.Name))
                {
                    return new ErrorCode(ErrorType.IllegalReference, "Dll references illegal dll with name " + referenceName.Name);
                }
            }

            return new ErrorCode(ErrorType.Ok, "Dll references allowed dll's.");
        }

        public static ErrorCode AnalyzeDllMethods(Assembly Dll)
        {
            // Проверяем атрибуты библиотеки.
            try
            {
                // Ищем атрибур SecurityPermission.
                Attribute[] DllSecurityAttributes = Attribute.GetCustomAttributes(Dll);
                Attribute DllSecurityAttribute = Attribute.GetCustomAttribute(Dll, typeof(SecurityPermissionAttribute));
                if (DllSecurityAttribute != null)
                {
                    if (((SecurityPermissionAttribute)DllSecurityAttribute).SkipVerification)
                    {
                        return new ErrorCode(ErrorType.IllegalDll, "Dll has unsafe-code.");
                    }
                }
                else
                {
                    return new ErrorCode(ErrorType.Ok);
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

            return new ErrorCode(ErrorType.Ok);
        }   
    }
}
