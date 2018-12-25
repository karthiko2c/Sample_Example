using Microsoft.AspNetCore.Mvc;
using SubQuip.Common.CommonData;
using Microsoft.AspNetCore.Authorization;
using SubQuip.Business.Interfaces;
using SubQuip.Entity.Models;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Search controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Search")]
    [ValidateModel]
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ISearchManagerService _searchManager;

        /// <summary>
        /// Initializes a new instance of the SearchController
        /// </summary>
        /// <param name="searchManager">Search manager.</param>
        public SearchController(ISearchManagerService searchManager)
        {
            this._searchManager = searchManager;

        }

        /// <summary>
        /// Global Search.
        /// </summary>
        /// <returns>The search.</returns>
        /// <param name="skip">Skip.</param>
        /// <param name="take">Take.</param>
        /// <param name="searchOptions">Search options.</param>
        [HttpGet]
        public IResult Search(int skip = 0, int take = 50, SearchOptions searchOptions = null)
        {
            var searchResults = _searchManager.Search();
            return searchResults;
        }

    }
}