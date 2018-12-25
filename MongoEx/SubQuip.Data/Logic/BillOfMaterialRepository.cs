using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Extensions.Configuration;
using SubQuip.Common.CommonData;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models.BillOfMaterials;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using SubQuip.Common.Enums;

namespace SubQuip.Data.Logic
{
    public class BillOfMaterialRepository : Repository<BillOfMaterial>, IBillOfMaterialRepository
    {
        public IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the BillOfMaterialRepository
        /// </summary>
        /// <param name="configuration"></param>
        public BillOfMaterialRepository(IConfiguration configuration) : base(configuration, "billofmaterial")
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get all Bill Of Materials.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="userId"></param>
        /// <param name="bomType"></param>
        /// <returns></returns>
        public List<BillOfMaterial> GetAllBillOfMaterial(SearchSortModel search, string userId, BomType bomType = BomType.Bom)
        {
            var query = from bom in Query
                        where bom.Type == bomType
                        select bom;

            if (!string.IsNullOrEmpty(search.SearchString))
            {
                query = query.Where(t => t.Name.ToLower().Contains(search.SearchString)
                                         || t.Description.ToLower().Contains(search.SearchString));
            }

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(t => t.CreatedBy.ToLower().Contains(userId.ToLower()));

            query = Sort(query, search.SortColumn, search.SortDirection.ToString());
            var data = Page(query, search.Page, search.PageSize).ToList();
            return data;
        }

        public int Count(Expression<Func<BillOfMaterial, bool>> filter)
        {
            return Filter(filter).Count();
        }

        public List<BomComment> GetCommentsForBom(string bomId)
        {
            var comments = new List<BomComment>();
            var bom = GetChildDocument(c => c.BomId == ObjectId.Parse(bomId), "Comments", "BomId");
            if (bom != null && bom.Comments != null && bom.Comments.Any())
                comments = bom.Comments.OrderBy(c => c.CreatedDate).ToList();
            return comments;
        }
    }
}
