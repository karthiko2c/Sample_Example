using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using SubQuip.Common.CommonData;
using SubQuip.Entity.Models.BillOfMaterials;
using SubQuip.Common.Enums;

namespace SubQuip.Data.Interfaces
{
    public interface IBillOfMaterialRepository : IRepository<BillOfMaterial>
    {

        /// <summary>
        /// Get all Bill of Materials.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="userId"></param>
        /// <param name="bomType"></param>
        /// <returns></returns>
        List<BillOfMaterial> GetAllBillOfMaterial(SearchSortModel search, string userId, BomType bomType = BomType.Bom);

		int Count(Expression<Func<BillOfMaterial, bool>> filter);

        /// <summary>
        /// Get comments for specified bill of material
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        List<BomComment> GetCommentsForBom(string bomId);

    }
}
