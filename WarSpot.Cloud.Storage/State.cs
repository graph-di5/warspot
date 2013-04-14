using System;

namespace WarSpot.Cloud.Storage
{
	[Flags]
	public enum State : int
	{
		NotStarted = 0x0,
		Started = 0x1,
		Finished = 0x2,
	}
}
