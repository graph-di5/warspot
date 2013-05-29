using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WarSpot.Security;
using WarSpot.Contracts.Service;
using WarSpot.Contracts.Intellect;


namespace Warspot.SecurityHandler
{
    public static class SecurityHandler
    {
        public static IntellectDomainProxy securityDomain;


        public static ErrorCode CheckUserIntellect(byte[] intellect)
        {
            ErrorCode staticSecurityChechingErrorCode;
            if ((staticSecurityChechingErrorCode =  IntellectStaticSecurityChecking.StaticSecurityChecking(intellect)).Type == ErrorType.IllegalDll)
            {
                return staticSecurityChechingErrorCode;
            }

            securityDomain = new IntellectDomainProxy(intellect);
            return DynamicalSecurityChecking();
        }

        private static ErrorCode DynamicalSecurityChecking()
        {           

            try
            {
                securityDomain.Construct(0, 0);
                securityDomain.Think(0, new BeingCharacteristics(), new WorldInfo(0));
            }
            catch (Exception e)
            {
                return new ErrorCode(ErrorType.IllegalDll, "Exception with that message: " + e.Message + " has been cought.");
            }

            return new ErrorCode(ErrorType.Ok, "No security permissions or exceptions have been thrown.");
        }

    }
}
