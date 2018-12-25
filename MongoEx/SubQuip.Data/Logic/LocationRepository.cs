using Microsoft.Extensions.Configuration;
using SubQuip.Common.CommonData;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubQuip.Data.Logic
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the LocationRepository
        /// </summary>
        /// <param name="configuration"></param>
        public LocationRepository(IConfiguration configuration) : base(configuration, "location")
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get All Locations.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Location> GetAllLocations(SearchSortModel search)
        {
            var query = from location in Query
                        select location;
            if (!string.IsNullOrEmpty(search.SearchString))
            {
                query = query.Where(t => t.LocationName.ToLower().Contains(search.SearchString));
            }

            query = Sort(query, search.SortColumn, search.SortDirection.ToString());
            var data = Page(query, search.Page, search.PageSize).ToList();
            return data;
        }
    }
}
