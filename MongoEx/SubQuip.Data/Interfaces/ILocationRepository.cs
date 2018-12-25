using SubQuip.Common.CommonData;
using SubQuip.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Data.Interfaces
{
    public interface ILocationRepository : IRepository<Location>
    {
        /// <summary>
        /// Get all Locations.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<Location> GetAllLocations(SearchSortModel search);
    }
}
