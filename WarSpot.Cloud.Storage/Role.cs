using System;

namespace WarSpot.Cloud.Storage
{
	public static class Role
	{
		[Flags]
		public enum EnumRoleType : int
		{
			User = 0x0,
			Admin = 0x1,
			MetaAdmin = 0x2,
			Developer = 0x4
		}

		public static int GetRoleCode(string role)
		{
			switch (role)
			{
			case "MetaAdmin":
				return (int)EnumRoleType.MetaAdmin;
			case "Admin":
				return (int)EnumRoleType.Admin;
			case "User":
				return (int)EnumRoleType.User;
			default:
				return -1;
			}
		}
	}
}
