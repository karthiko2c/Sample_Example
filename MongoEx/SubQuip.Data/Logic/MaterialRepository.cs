using System.Collections.Generic;
using SubQuip.Entity.Models.BillOfMaterials;
using Microsoft.Extensions.Configuration;
using SubQuip.Data.Interfaces;
using System.Linq;
using SubQuip.Common.CommonData;
using SubQuip.Entity.Models;
using SubQuip.Common.Extensions;
using MongoDB.Driver;
using System;

namespace SubQuip.Data.Logic
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the MaterialRepository
        /// </summary>
        /// <param name="configuration"></param>
        public MaterialRepository(IConfiguration configuration) : base(configuration, "material")
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get All Materials.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Material> GetAllMaterials(SearchSortModel search)
        {
            var query = from material in Query
                        select material;
            if (!string.IsNullOrEmpty(search.SearchString))
            {
                query = query.Where(t => t.Materialnumber.ToLower().Contains(search.SearchString)
                                         || t.Description.ToLower().Contains(search.SearchString)
                                         || t.ManufactorPartNumber.ToLower().Contains(search.SearchString)
                                         || t.Type.ToLower().Contains(search.SearchString)
                                         || t.ManufactorName.ToLower().Contains(search.SearchString)
                );
            }

            query = Sort(query, search.SortColumn, search.SortDirection.ToString());
            var data = Page(query, search.Page, search.PageSize).ToList();
            return data;
        }

        /// <summary>
        /// Get Recent added materials in week
        /// </summary>
        /// <returns></returns>
        public long GetRecentAddedMaterials()
        {
            var start = GenericHelper.CurrentDate.AddDays(Convert.ToDouble(_configuration["PastDays"])).Date;
            var end = GenericHelper.CurrentDate.AddDays(1).Date; // To include the today date upto 24 hrs
            var filterBuilder = Builders<Material>.Filter;
            var filter = filterBuilder.Gte(x => x.CreatedDate, start) &
                         filterBuilder.Lt(x => x.CreatedDate, end);
            return Collection.Find(filter).Count();
        }
    }
}
