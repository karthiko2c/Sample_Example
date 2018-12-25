using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Healthcheck enpoint should be useable as a health and liveness check
    /// </summary>
    [Produces("application/json")]
    [Route("api/Health")]
    public class HealthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="hostingEnvironment">Hosting environment.</param>
        public HealthController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// This method will just to check wheather the API is up or not
        /// (without authentication).
        /// </summary>
        /// <returns>Result model with status</returns>
        [AllowAnonymous]
        [HttpGet]
        public IResult Index()
        {
            return new Result { Status = Status.Success, Message = "SubQuip API is running", Body = "SubQuip API is running", Operation = Operation.Read };
        }
    }
}