using System;

namespace WarSpot.Cloud.Storage
{
	[Flags]
	public enum RoleType : int
	{
		User = 0x0,
		Admin = 0x1,
		MetaAdmin = 0x2,
		Developer = 0x4,
		NoUser = 0x1000000,
	}
	public static class Role
	{

		public static int GetRoleCode(string role)
		{
			switch (role)
			{
			case "MetaAdmin":
				return (int)RoleType.MetaAdmin;
			case "Admin":
				return (int)RoleType.Admin;
			case "User":
				return (int)RoleType.User;
			default:
				return -1;
			}
		}

		public static string GetRoleName(int id)
		{
			return ((RoleType) id).ToString();

			switch ((RoleType)id)
			{
			case RoleType.User:
				return RoleType.User.ToString();
			case RoleType.Admin:
				return RoleType.Admin.ToString();
			case RoleType.MetaAdmin:
				return RoleType.MetaAdmin.ToString();
			case RoleType.Developer:
				return RoleType.Developer.ToString();
			default:
				return RoleType.NoUser.ToString();

			}
		}
	}
}
