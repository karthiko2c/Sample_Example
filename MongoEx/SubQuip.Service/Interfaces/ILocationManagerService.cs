using SubQuip.Common.CommonData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Business.Interfaces
{
    public interface ILocationManagerService
    {
        /// <summary>
        /// Get all Locations.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetAllLocations(SearchSortModel search);
    }
}
