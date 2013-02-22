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
			Developer = 0x4,
			NoUser = 0x1000000,
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

		public static string GetRoleName(int id)
		{
			switch ((EnumRoleType)id)
			{
			case EnumRoleType.User:
				return EnumRoleType.User.ToString();
			case EnumRoleType.Admin:
				return EnumRoleType.Admin.ToString();
			case EnumRoleType.MetaAdmin:
				return EnumRoleType.MetaAdmin.ToString();
			case EnumRoleType.Developer:
				return EnumRoleType.Developer.ToString();
			default:
				return EnumRoleType.NoUser.ToString();

			}
		}
	}
}
