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
}
