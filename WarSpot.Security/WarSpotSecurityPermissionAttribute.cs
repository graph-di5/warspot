using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Permissions;

namespace WarSpot.Security
{
    [AttributeUsageAttribute(AttributeTargets.All, AllowMultiple = true)]
    public class WarSpotSecurityPermissionAttribute: CodeAccessSecurityAttribute
    {
        bool unrestricted = false;

        public new bool Unrestricted
        {
            get { return unrestricted; }
            set { unrestricted = value; }
        }

        public WarSpotSecurityPermissionAttribute(SecurityAction action)
            : base(action)
        {
        }

        public override IPermission CreatePermission()
        {
            if (Unrestricted)
            {
                return new WarSpotSecurityPermission(PermissionState.Unrestricted);
            }
            else
            {
                return new WarSpotSecurityPermission(PermissionState.None);
            }
        }
    }
}
