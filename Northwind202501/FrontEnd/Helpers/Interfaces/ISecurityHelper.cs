using FrontEnd.ApiModels;
using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface ISecurityHelper
    {
        LoginAPI Login(UserViewModel user);

    }
}
