using Microsoft.AspNetCore.Mvc;
using RS.Common.CommonData;
using RS.Common.Enums;
using RS.ViewModel.User;
using System;
using RS.Service.Interfaces;
using static RS.ViewModel.User.LoginModel;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace RS.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Login/[Action]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManagerService _userService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public LoginController(IConfiguration configuration, IUserManagerService userService, IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [ValidateModel]
        public IResult LoginUser([FromBody]UserLoginModel loginModel)
        {
            var userResult = _userService.LoginUser(loginModel.UserEmail, loginModel.UserPassword);
            if (userResult.Status == Status.Success)
            {
                var token = new ObjectResult(GenerateToken(userResult.Body));
                userResult.Body = token.Value;
            }
            else
            {
                userResult.Body = Unauthorized();
            }
            return userResult;
        }

        [HttpPut]
        public IResult ChangePassword([FromBody]ChangePassword changePassword)
        {
            var user = _userService.ChangePassword(changePassword.OldPassword, changePassword.NewPassword);
            return user;
        }

        [HttpPost]
        public IResult ForgotPassword([FromBody]string userName)
        {
            var userEmail = _userService.ForgotPassword(userName);
            return userEmail;
        }

        [HttpPut]
        public IResult UpdateUserProfile()
        {
            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(Request.Form["userView"]);
            var file = Request.Form.Files["uploadProfile"];
            var updatedUser = _userService.UpdateUser(userViewModel);
            if (updatedUser.Body != null && file != null)
            {
                var allowedExtensions = _configuration["ProfileExtension"].Split(',');
                var docName = updatedUser.Body.ToString() + ".png";
                FileHelper.SaveFile(file, docName, allowedExtensions, _hostingEnvironment, "uploadProfiles");
            }
            return updatedUser;
        }


        #region Private methods

        private string GenerateToken(UserViewModel user)
        {
            var approval = JsonConvert.SerializeObject(user.ApprovalDetail);
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.NameIdentifier, user.LastName),
                new Claim(ClaimTypes.Surname, user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Sid,user.UserId.ToString()),
                new Claim(ClaimTypes.Actor, approval),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
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