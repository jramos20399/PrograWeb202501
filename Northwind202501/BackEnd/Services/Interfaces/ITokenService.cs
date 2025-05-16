using BackEnd.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace BackEnd.Services.Interfaces
{
    public interface ITokenService
    {
        TokenModel CreateToken(IdentityUser user, List<string> roles);

    }
}
