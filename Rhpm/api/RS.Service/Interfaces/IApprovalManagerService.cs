using System;
using System.Collections.Generic;
using System.Text;
using RS.Common.CommonData;
using RS.ViewModel.Approval;
using RS.ViewModel.User;
using RS.Entity.Models;
using RS.ViewModel.Opening;
using RS.ViewModel.Candidate;

namespace RS.Service.Interfaces
{
    public interface IApprovalManagerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        IResult GetApprovalEvents(int approvalId, Guid entityId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IResult GetAllApprovals();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IResult GetAllApprovalEventRoles();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IResult GetApprovedUsersByRole(int roleId, int approvalEventId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="approvalEventRoleViewModel"></param>
        /// <returns></returns>
        IResult ManageApprovalEventRole(ApprovalEventRoleViewModel approvalEventRoleViewModel);

        /// <summary>
        /// 
        /// </summary>

        /// <param name="entityId"></param>
        /// <returns></returns
        IResult ApprovalTransactionDetails(Guid entityId);

        /// <param name="openingId"></param>
        /// <returns></returns>
        IResult GetApprovalTransactionByEntity(Guid openingId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="approvalTransactionViewModel"></param>
        /// <returns></returns>
        ApprovalTransactionViewModel AddApprovalTransaction(EntityAndApprovalViewModel entityAndApprovalViewModel);

        /// <summary>
        /// Update Approval Transaction
        /// </summary>
        /// <param name="approvalTransaction"></param>
        /// <returns></returns>
        IResult ManageApprovalTransaction(EntityAndApprovalViewModel entityAndApprovalViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IResult GetDashboardDetails();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IResult GetChartDetails(int type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="approvalEvent"></param>
        /// <returns></returns>
        IResult GetUsersToScheduleInterview(int approvalEvent);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidateId"></param>
        /// <returns></returns>
        IResult GetNextEventOrderForCandidate(Guid candidateId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        IResult CheckForStartInterview(CandidateListModel candidate);

        IResult GetApprovalEventsOfUserForCandidate(Guid candidateId);
    }
}
