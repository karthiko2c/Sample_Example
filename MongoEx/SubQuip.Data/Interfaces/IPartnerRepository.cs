using SubQuip.Common.CommonData;
using SubQuip.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Data.Interfaces
{
    public interface IPartnerRepository: IRepository<Partner>
    {
        /// <summary>
        /// Get All Partners.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<Partner> GetAllPartners(SearchSortModel search);
    }
}
