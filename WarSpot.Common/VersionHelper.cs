using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WarSpot.Common
{
	public class VersionHelper
	{
		public static bool CheckVersionFromStream(Stream fs, Version currVersion)
		{
			var bf = new BinaryFormatter();
			var wsVersion = (System.Version)bf.Deserialize(fs);
			return CheckVersion(currVersion, wsVersion);
		}

		public static bool CheckVersion(Version currVersion, Version versToCheck)
		{
			return (currVersion.Major == versToCheck.Major && currVersion.Minor == versToCheck.Minor);
		}
	}
}
