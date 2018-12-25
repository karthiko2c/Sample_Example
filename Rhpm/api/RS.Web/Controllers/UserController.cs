using Microsoft.AspNetCore.Mvc;
using RS.Common.CommonData;
using RS.ViewModel.User;
using System;
using System.Net;
using RS.Service.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using RS.ViewModel.SearchAndSortModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Security.Principal;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RS.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/User/[Action]")]
    [ValidateModel]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserManagerService _userService;
        private readonly ClaimsPrincipal _principal;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserController(IPrincipal principal, IUserManagerService userService, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _userService = userService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            this._principal = principal as ClaimsPrincipal;
        }

        [HttpGet]
        public IResult GetUserDetails()
        {
            var identity = (ClaimsIdentity)_principal.Identity;
            var userDetails = _userService.GetUserById(new Guid(identity.FindFirst(ClaimTypes.Sid).Value));
            if (userDetails.Body != null)
            {
                userDetails.Body.ProfileImage = GetProfilePicture(userDetails.Body.UserId);
            }
            return userDetails;
        }

        [HttpPost]
        public IResult CreateUser()
        {
            var userViewModel = JsonConvert.DeserializeObject<UserViewModel>(Request.Form["userView"]);
            var file = Request.Form.Files["uploadProfile"];
            var createdUser = _userService.CreateUser(userViewModel);
            if (createdUser.Body != null)
            {
                var allowedExtensions = _configuration["ProfileExtension"].Split(',');
                var docName = createdUser.Body.ToString() + ".png";
                FileHelper.SaveFile(file, docName, allowedExtensions, _hostingEnvironment, "uploadProfiles");
            }
            return createdUser;
        }

        [HttpPut]
        public IResult UpdateUser()
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

        [HttpGet]
        public IResult GetAllUser()
        {
            var userList = _userService.GetAllUser();
            return userList;
        }

        [HttpPost]
        public ActionResult GetUserById(Guid userId)
        {
            var userRecord = _userService.GetUserById(userId);
            var ProfileImage = new List<FileStreamResult>();
            if (userRecord.Body != null)
            {
                ProfileImage = GetProfilePicture(userId);
            }
            return ProfileImage[0];
        }

        [HttpGet]
        public IResult GetUsersByRole(int id)
        {
            var userRecord = _userService.GetUsersByRole(id);
            return userRecord;
        }

        [HttpPost]
        public IResult GetUsersResults([FromBody]SearchAndSortModel searchAndSortModel)
        {
            var userList = _userService.GetUsersResults(searchAndSortModel);
            return userList;
        }

        private List<FileStreamResult> GetProfilePicture(Guid id)
        {
            var folder = _configuration["uploadProfiles"];
            var files = new List<string>();
            files.Add(Path.Combine(_hostingEnvironment.ContentRootPath, folder + id + ".png"));
            files.Add(Path.Combine(_hostingEnvironment.ContentRootPath, folder + "suresh.png"));
            var fileStreams = new List<FileStreamResult>();
            foreach (var path in files)
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                fileStreams.Add(File(memory, FileHelper.GetContentType(path), Path.GetFileName(path)));
            }
            return fileStreams;
           
        }

    }
}