using SubQuip.Common.CommonData;
using SubQuip.ViewModel.Partner;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Business.Interfaces
{
    public interface IPartnerManagerService
    {
        /// <summary>
        /// Get a list of all partners registered in SubQuip.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult GetPartnerById(string id);

        /// <summary>
        /// Get a specific partner by its identifier.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetAllPartners(SearchSortModel search);

        /// <summary>
        /// Create a Partner.
        /// </summary>
        /// <param name="partnerViewModel"></param>
        /// <returns></returns>
        IResult Create(PartnerViewModel partnerViewModel);

        /// <summary>
        /// Update a Partner.
        /// </summary>
        /// <param name="partnerViewModel"></param>
        /// <returns></returns>
        IResult Update(PartnerViewModel partnerViewModel);

        /// <summary>
        /// Delete a Partner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult Delete(string id);

        /// <summary>
        /// Delete All Partners.
        /// </summary>
        /// <returns></returns>
        IResult DeleteAllPartners();
    }
}
