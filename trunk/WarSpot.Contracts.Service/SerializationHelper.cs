using System.IO;
using System.Runtime.Serialization;
using WarSpot.Common.Utils;

namespace WarSpot.Contracts.Service
{
	public static class SerializationHelper
	{
		public static MatchReplay Deserialize(Stream fs)
		{
			var dcs = new DataContractSerializer(typeof(MatchReplay));
		    var replay = dcs.ReadObject(fs) as MatchReplay;
            if(!VersionHelper.CheckVersion(replay.AssemblyVersion))throw new InvalidDataException("Version incompatible");
		    return replay;
		}

		/// <summary>
		/// Returns deserializated list of GameEvents from replay
		/// </summary>
        public static MatchReplay Deserialize(string path)
		{
			using(var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
			    return Deserialize(fs);
			}
		}

        public static byte[] Serialize(MatchReplay replay)
        {
            var ms = new MemoryStream();
            var dcs = new DataContractSerializer(typeof(MatchReplay));
            dcs.WriteObject(ms, replay);
            return ms.ToArray();
        }

	}
}
