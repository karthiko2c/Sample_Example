using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubQuip.Common.CommonData;
using Microsoft.AspNetCore.Authorization;
using SubQuip.Business.Interfaces;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Location Controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Location/[Action]")]
    [ValidateModel]
    [Authorize]
    public class LocationController : Controller
    {
        private readonly ILocationManagerService _locationManager;

        /// <summary>
        /// Initializes a new instance of the LocationController.
        /// </summary>
        /// <param name="locationManager"></param>
        public LocationController(ILocationManagerService locationManager)
        {
            _locationManager = locationManager;
        }

        /// <summary>
        /// Get all Locations.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public IResult Locations(SearchSortModel search)
        {
            var locationList = _locationManager.GetAllLocations(search);
            return locationList;
        }
    }
}