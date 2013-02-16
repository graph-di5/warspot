using System.Security.Principal;

namespace MvcFormsAuth.Security
{
    public interface ICustomIdentity : IIdentity
    {
        bool IsInRole(string role);
        string ToJson();
    }
}