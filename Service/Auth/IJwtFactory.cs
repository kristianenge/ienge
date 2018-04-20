using System.Security.Claims;
using System.Threading.Tasks;
using IEnge.Models;

namespace IEnge.Service.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
        Task<ValidationResponse> ValidateToken(TokenModel model);
    }
}
