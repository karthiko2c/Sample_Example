using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubQuip.Business.Interfaces;
using SubQuip.Common.CommonData;
using SubQuip.Data.Interfaces;
using SubQuip.ViewModel.User;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// User controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/User/[Action]")]
    [ValidateModel]
    [Authorize]
    public class UserController : Controller
    {

        private readonly IGraphRepository _graphRepositoty;
        private readonly IUserManagerService _userManager;

        /// <summary>
        /// Initializes a new instance of the UserController
        /// </summary>
        /// <param name="graphRepository">graph service.</param>
        /// <param name="userManager"></param>
        public UserController(IGraphRepository graphRepository, IUserManagerService userManager)
        {
            _graphRepositoty = graphRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string userName)
        {
            var user = User.Identity;
            if (userName != user.Name && !User.Claims.Any(c => c.Value == userName) && !(await _graphRepositoty.FindExternalUser(user.Name)).Value.First().IsAdmin)
            {
                return Unauthorized();
            }

            var details = await _graphRepositoty.FindExternalUser(userName);
            return new ObjectResult(details.Value.FirstOrDefault());
        }

        /// <summary>
        /// Application users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var details = await _graphRepositoty.ApplicationUsers();
            return new ObjectResult(details.Value);
        }

        /// <summary>
        /// Get all Tabs For User.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResult GetUserTabs()
        {
            var userTabs = _userManager.GetTabsForUser();
            return userTabs;
        }

        /// <summary>
        /// Save 
        /// </summary>
        /// <param name="savedTabViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult SaveTabDetail([FromBody]SavedTabViewModel savedTabViewModel)
        {
            var savedTabDetails = _userManager.SaveTabDetail(savedTabViewModel);
            return savedTabDetails;
        }

    }
}