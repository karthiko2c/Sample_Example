using SubQuip.Entity.Models.BillOfMaterials;
using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.CommonData;
using SubQuip.Entity.Models;

namespace SubQuip.Data.Interfaces
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        /// <summary>
        /// Get All Equipments.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<Equipment> GetAllEquipments(SearchSortModel search);

        /// <summary>
        /// Get Recent added equipments in week
        /// </summary>
        /// <returns></returns>
        long GetRecentAddedEquipment();
    }
}
