using System.Collections.Generic;
using RS.Entity.Models;
using System;

using RS.Entity.DTO;

using RS.ViewModel.Approval;
using RS.ViewModel.ChartViewModel;


namespace RS.Data.Interfaces
{
    public interface IApprovalRepository : IRepository<Approvals>
    {
        List<ApprovalEvents> GetApprovalEvents(int approvalId);
        List<Approvals> GetAllApprovals();
        void AddApprovalEventRole(ApprovalEventRoles approvalEventRole);
        List<ApprovalEventRoles> GetAllApprovalEventRole();
        Dictionary<string, List<int>> GetApprovalEventsOfUserForOpening(Guid UserId);
        List<ApprovalTransactionDetails> GetApprovalTransactionDetails();
        List<ApprovalTransactionDetailsDTO> ApprovalTransactionDetails(Guid entityId);
        int GetApprovalEventOrderNumber(ApprovalEventViewModel approvalEventViewModel);
        List<Users> GetApprovedUsersByRole(int roleId, int approvalEventId);
        List<Users> GetApprovedUsers(int approvalEventId);
        void CreateApprovalTransaction(ApprovalTransactions approvalTransaction);
        ApprovalTransactions GetApprovalTransactionByEntity(Guid entityId);
        void UpdateApprovalTransaction(ApprovalTransactions approvalTransaction, ApprovalTransactionDetails approvalTransactionDetail);
        void AddApprovalTransactionDetails(ApprovalTransactionDetails approvalTransactionDetails);
        List<ApprovalTransactions> GetAllApprovalTransactions(List<Guid> openingIds);
        int GetApprovalEventOrderOfUser(Guid entityId, Guid userId, int approvalId);
        int GetTotalOpenOpenings();
        int GetTotalCloseOpenings();
        int GetTotalCandidatesHired();
        int GetTotalCandidatesAttendedInterview(int month);
        List<SeriesModel> GetSeriesDetail(int type);
        Users GetUserForCandidateApproval(Guid entityId, int nextEventOrderNumber);
        List<Users> GetUserForOpeningApproval(ApprovalTransactionViewModel approvalTransactionViewModel);
        int GetNextEventOrderForCandidate(Guid candidateId);
        bool CheckForStartInterview(Guid candidateId, int approvalEventId, Guid userId);
        Dictionary<string, List<int>> GetApprovalEventsOfUserForCandidate(Guid candidateId, Guid UserId);

    }
}
