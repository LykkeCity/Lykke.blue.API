using System.Security.Claims;
using System.Threading.Tasks;

namespace Lykke.blue.Api.Core.Identity
{
    public interface ILykkePrincipal
    {
        Task<ClaimsPrincipal> GetCurrent();
        string GetToken();
    }
}
