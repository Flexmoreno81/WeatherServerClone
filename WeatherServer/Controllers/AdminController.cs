using CountryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WeatherServer.DTO;


namespace WeatherServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<WorldCitiesUser> userManager,
        JwtHandler jwtHandler) : ControllerBase
    {
        //We need method
        [HttpPost("Login")]
        public async Task<IActionResult> Loggin(LogginRequest login_request)
        {
            WorldCitiesUser? user = await userManager.FindByNameAsync(login_request.UserName);

            if (user == null) { 
                return Unauthorized("Bad username");
            }

            bool isSucess =  await userManager.CheckPasswordAsync(user, login_request.Password);

            if (!isSucess) {
                return Unauthorized("Wrong Password");
            }

            JwtSecurityToken token = await jwtHandler.GetTokenAsync(user);

            string str_token = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LogginResult {
                Success = true,
                Message = "Logging Sucesss", 
                Token = str_token
            } ); 

        }
    }
}
