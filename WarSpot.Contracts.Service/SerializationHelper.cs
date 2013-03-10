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
			// rewind to start
			fs.Seek(0, SeekOrigin.Begin);
			var replay = dcs.ReadObject(fs) as MatchReplay;
			if (replay == null || !VersionHelper.CheckVersion(replay.AssemblyVersion))
			{
				return null;
			}
			return replay;
		}

		/// <summary>
		/// Returns deserializated list of GameEvents from replay
		/// </summary>
		public static MatchReplay Deserialize(string path)
		{
			using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return Deserialize(fs);
			}
		}

		public static void Serialize(MatchReplay replay, Stream stream)
		{
			var dcs = new DataContractSerializer(typeof(MatchReplay));
			dcs.WriteObject(stream, replay);
		}

		public static byte[] Serialize(MatchReplay replay)
		{
			var ms = new MemoryStream();
			Serialize(replay, ms);
			return ms.ToArray();
		}

	}
}
