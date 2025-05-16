using Azure;
using BackEnd.Model;
using BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly UserManager<IdentityUser> userManager;
        private ITokenService TokenService;

        public AuthController(UserManager<IdentityUser> userManager,
                                ITokenService tokenService)
        {
            this.userManager = userManager;
            this.TokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {


            IdentityUser user = new IdentityUser { 
                
                UserName = "pabloramos" };
            LoginModel Usuario = new LoginModel();
            if (user != null )
            {

                var userRoles = new List<string>(); 

                var jwtToken = TokenService.CreateToken(user, userRoles.ToList());

                Usuario.Token = jwtToken;
                Usuario.Roles = userRoles.ToList();
                Usuario.Username = user.UserName;


                return Ok(Usuario);
            }

            return Unauthorized();



        }



        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            var userExists = await userManager.FindByNameAsync(model.Username);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            IdentityUser user = new IdentityUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }

            return Ok();

        }

    }
}
