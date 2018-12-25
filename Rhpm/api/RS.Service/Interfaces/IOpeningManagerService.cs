using RS.Common.CommonData;
using RS.ViewModel.Opening;
using System;

namespace RS.Service.Interfaces
{
    public interface IOpeningManagerService
    {
        /// <summary>
        /// Create a Opening
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        IResult InsertOrUpdateOpening(EntityAndApprovalViewModel entityAndApprovalViewModel);

        /// <summary>
        /// Delete a Opening
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult DeleteOpening(Guid id);

        /// <summary>
        /// Get All Openings
        /// </summary>
        /// <returns></returns>
        IResult GetAllOpenings();

        /// <summary>
        /// Get Opening By id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult GetOpeningById(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IResult GetOpeningsCorrespondingToLoggedUser(Guid userId);
    }
}
