using System.Security.Principal;

namespace WarSpot.WebFace.Security
{
	public interface ICustomIdentity : IIdentity
	{
		bool IsInRole(string role);
		string ToJson();
	}
}