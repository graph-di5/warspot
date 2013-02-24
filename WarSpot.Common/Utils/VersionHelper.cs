using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace WarSpot.Common.Utils
{
	public class VersionHelper
	{
		public static Version Version
		{
			get { return Assembly.GetAssembly(typeof(VersionHelper)).GetName().Version; }
		}

#if fasle
		public static bool CheckVersionFromStream(Stream fs, Version currVersion)
		{
			var bf = new BinaryFormatter();
			var wsVersion = (Version)bf.Deserialize(fs);
			return CheckVersion(currVersion, wsVersion);
		}

		public static bool CheckVersion(Version currVersion, Version versToCheck)
		{
			return (currVersion.Major == versToCheck.Major && currVersion.Minor == versToCheck.Minor);
		}
#endif

		public static bool CheckVersionFromStream(Stream fs)
		{
			var bf = new BinaryFormatter();
			var wsVersion = (Version)bf.Deserialize(fs);
			return CheckVersion(wsVersion);
		}


		public static bool CheckVersion(Version versToCheck)
		{
			return (Version.Major == versToCheck.Major && Version.Minor == versToCheck.Minor);
		}
	}
}
