using Microsoft.Extensions.Configuration;
using SubQuip.Common.CommonData;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Data.Logic
{
    public class PartnerRepository : Repository<Partner>, IPartnerRepository
    {
        public IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the PartnerRepository
        /// </summary>
        /// <param name="configuration"></param>
        public PartnerRepository(IConfiguration configuration) : base(configuration, "partner")
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get All Partners.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Partner> GetAllPartners(SearchSortModel search)
        {
            var query = from partner in Query
                        select partner;
            if (!string.IsNullOrEmpty(search.SearchString))
            {
                query = query.Where(t => t.Name.ToLower().Contains(search.SearchString)
                                         || t.Email.ToLower().Contains(search.SearchString)
                                        );
            }
            query = Sort(query, search.SortColumn, search.SortDirection.ToString());
            var data = Page(query, search.Page, search.PageSize).ToList();
            return data;
        }
    }
}
