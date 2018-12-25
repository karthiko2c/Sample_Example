using System.Collections.Generic;
using SubQuip.Common.CommonData;
using SubQuip.Entity.Models;
using SubQuip.Entity.Models.BillOfMaterials;

namespace SubQuip.Data.Interfaces
{
    public interface IMaterialRepository : IRepository<Material>
    {
        /// <summary>
        /// Get All Materials.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<Material> GetAllMaterials(SearchSortModel search);

        /// <summary>
        /// Get Recent added materials in week
        /// </summary>
        /// <returns></returns>
        long GetRecentAddedMaterials();
    }
}
