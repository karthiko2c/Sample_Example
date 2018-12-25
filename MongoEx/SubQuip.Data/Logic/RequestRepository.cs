using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using SubQuip.Common.CommonData;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models;

namespace SubQuip.Data.Logic
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public IConfiguration Configuration;

        /// <summary>
        /// Initializes a new instance of the RequestRepository
        /// </summary>
        /// <param name="configuration"></param>
        public RequestRepository(IConfiguration configuration) : base(configuration, "request")
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Get All Requests.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Request> GetAllRequests(SearchSortModel search)
        {
            var query = from request in Query
                        select request;
            if (!string.IsNullOrEmpty(search.SearchString))
            {
                query = query.Where(t => t.Description.ToLower().Contains(search.SearchString)
				                    || t.RequestedBy.ToLower().Contains(search.SearchString)
                                         || t.Company.ToLower().Contains(search.SearchString)
                                         || t.PhoneNumber.ToLower().Contains(search.SearchString)
                                         || t.MailUsers.Any(x => x.Name.Contains(search.SearchString) || x.Email.Contains(search.SearchString))
                );
                // TODO : Search by file name
            }
            query = Sort(query, search.SortColumn, search.SortDirection.ToString());
            var data = Page(query, search.Page, search.PageSize).ToList();
            return data;
        }
    }
}
