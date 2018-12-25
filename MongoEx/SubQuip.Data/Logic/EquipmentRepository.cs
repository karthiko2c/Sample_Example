using System;
using System.Collections.Generic;
using SubQuip.Entity.Models;
using SubQuip.Entity.Models.BillOfMaterials;
using Microsoft.Extensions.Configuration;
using SubQuip.Data.Interfaces;
using System.Linq;
using MongoDB.Driver;
using SubQuip.Common.CommonData;
using SubQuip.Common.Extensions;

namespace SubQuip.Data.Logic
{
    public class EquipmentRepository : Repository<Equipment>, IEquipmentRepository
    {
        public IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the EquipmentRepository
        /// </summary>
        /// <param name="configuration"></param>
        public EquipmentRepository(IConfiguration configuration) : base(configuration, "equipment")
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get All Equipments.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Equipment> GetAllEquipments(SearchSortModel search)
        {
            var query = from equipment in Query
                        select equipment;
            if (!string.IsNullOrEmpty(search.SearchString))
            {
                query = query.Where(t => t.EquipmentNumber.ToLower().Contains(search.SearchString)
                                         || t.SerialNumber.ToLower().Contains(search.SearchString)
                                         || t.Owner.ToLower().Contains(search.SearchString)
                                         || t.License.ToLower().Contains(search.SearchString)
                                         || t.Location.ToLower().Contains(search.SearchString)
                                        );
            }
            query = Sort(query, search.SortColumn, search.SortDirection.ToString());
            var data = Page(query, search.Page, search.PageSize).ToList();
            return data;
        }

        /// <summary>
        /// Get Recent added equipments in week
        /// </summary>
        /// <returns></returns>
        public long GetRecentAddedEquipment()
        {
            var start = GenericHelper.CurrentDate.AddDays(Convert.ToDouble(_configuration["PastDays"])).Date;
            var end = GenericHelper.CurrentDate.AddDays(1).Date; // To include the today date upto 24 hrs
            var filterBuilder = Builders<Equipment>.Filter;
            var filter = filterBuilder.Gte(x => x.CreatedDate, start) &
                         filterBuilder.Lt(x => x.CreatedDate, end);
          return Collection.Find(filter).Count();
        }
    }
}
