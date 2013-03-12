using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Permissions;

namespace WarSpot.Security
{
    [SerializableAttribute()]
    public sealed class WarSpotSecurityPermission : CodeAccessPermission, IUnrestrictedPermission
    {      

        private bool unrestricted;

        public WarSpotSecurityPermission(PermissionState state)
        {
            unrestricted = (state == PermissionState.Unrestricted);
        }

        public bool IsUnrestricted()
        {
            return unrestricted;
        }

        public override IPermission Copy()
        {
            throw new NotImplementedException();
        }

        public override void FromXml(SecurityElement elem)
        {
            throw new NotImplementedException();
        }

        public override IPermission Intersect(IPermission target)
        {
            throw new NotImplementedException();
        }

        public override bool IsSubsetOf(IPermission target)
        {
            throw new NotImplementedException();
        }

        public override SecurityElement ToXml()
        {
            throw new NotImplementedException();
        }
    }
}
