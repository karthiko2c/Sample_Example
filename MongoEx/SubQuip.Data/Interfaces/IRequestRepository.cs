using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.CommonData;
using SubQuip.Entity.Models;

namespace SubQuip.Data.Interfaces
{
    public interface IRequestRepository : IRepository<Request>
    {
        /// <summary>
        /// Get All Request.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<Request> GetAllRequests(SearchSortModel search);
    }
}
