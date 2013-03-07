using System;

namespace WarSpot.Cloud.Storage.Models
{
	public class BlobFile
	{
		public Guid ID;
		public string Name;
		public DateTime CreationTime;
		public string Description;
		public string LongDescription;
		public byte[] Data;
	}
}
