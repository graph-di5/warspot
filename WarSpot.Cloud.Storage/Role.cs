using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarSpot.Cloud.Storage
{
    public static class Role
    {
        [Flags]
        public enum EnumRoleType : int
        {
            MetaAdmin = 2,
            Admin = 1,
            User = 0
        }

        public static int GetRoleCode(string role)
        {
            switch (role)
            {
                case "MetaAdmin": return (int)EnumRoleType.MetaAdmin; 

                case "Admin": return (int)EnumRoleType.Admin; 

                case "User": return (int)EnumRoleType.User;

                default: return -1; 
            }
        }
            
    }
}
