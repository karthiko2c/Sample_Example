using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SubQuip.Business.Interfaces;
using SubQuip.Common.CommonData;
using SubQuip.ViewModel.User;
using SubQuip.Common.Enums;


namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Login controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Login/[Action]")]
    [ValidateModel]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SubQuip.WebApi.Controllers.LoginController"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <returns>Access token for the logged in user</returns>
        /// <param name="loginViewModel">Login view model.</param>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginUser([FromBody]UserLoginViewModel loginViewModel)
        {
            UserViewModel userView = null;
            if (loginViewModel.UserName == "test" && loginViewModel.UserPassword == "test")
            {
                userView = new UserViewModel
                {
                    UserId = "5b4c73928ac043509057e353",
                    UserName = "adminuser",
                    GivenName = "Admin",
                    Surname = "User",
                    Mail = "admin@subquip.com",
                    Company = "SubquipAdmin",
                    IsAdmin = true
                };
                userView.DisplayName = userView.GivenName + " " + userView.Surname;
            }
            if (userView == null)
                return Unauthorized();
            userView.Token = GenerateToken(userView);
            return new ObjectResult(userView);
        }

        #region Private methods

        private string GenerateToken(UserViewModel user)
        {
            var claims = new Claim[]
            {
                new Claim("Name", user.DisplayName),
                new Claim(ClaimTypes.Email,user.Mail),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SigningKey"])),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion

    }
}