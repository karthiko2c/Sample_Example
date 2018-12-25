using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RS.Data;
using RS.Data.Interfaces;
using RS.Entity.Models;
using RS.ViewModel.Approval;
using RS.Entity.DTO;
using RS.Common.Enums;
using RS.ViewModel.ChartViewModel;


namespace RS.Data.Logic
{
    public class ApprovalRepository : Repository<Approvals>, IApprovalRepository
    {
        private readonly RSContext _context;
        public ApprovalRepository(RSContext context) : base(context)
        {

            _context = context;
        }

        public List<ApprovalEvents> GetApprovalEvents(int approvalId)
        {
            return _context.ApprovalEvents.Include(t => t.ApprovalActions)
                .Where(t => t.Approval.ApprovalId == approvalId)
                .OrderBy(t => t.ApprovalEventOrder).ToList();
        }

        public void AddApprovalEventRole(ApprovalEventRoles approvalEventRole)
        {
            _context.ApprovalEventRoles.Add(approvalEventRole);
        }

        public List<ApprovalEventRoles> GetAllApprovalEventRole()
        {
            return _context.ApprovalEventRoles.Include(t => t.ApprovalEvent).Include(s => s.Role).Include(r => r.User)
                .Where(x => x.IsActive && !x.IsDeleted).ToList();
        }

        public List<Approvals> GetAllApprovals()
        {
            return _context.Approvals.ToList();
        }

        public Dictionary<string, List<int>> GetApprovalEventsOfUserForOpening(Guid UserId)
        {
            Dictionary<string, List<int>> dictionary = new Dictionary<string, List<int>>();
            var events = _context.ApprovalEventRoles.Where(x => x.UserId == UserId && x.IsActive && !x.IsDeleted).ToList();

            var approvalEvents = events.Distinct().Select(t => t.ApprovalEventId).ToList();
            approvalEvents.ForEach(x =>
            {
                string approvalName = _context.ApprovalEvents.Include(s => s.Approval).FirstOrDefault(t => t.ApprovalEventId == x).Approval.ApprovalName;
                if (dictionary.ContainsKey(approvalName))
                {
                    dictionary[approvalName].Add(x);
                }
                else
                {
                    List<int> eventIds = new List<int>();
                    eventIds.Add(x);
                    dictionary.Add(approvalName, eventIds);
                }
            });
            return dictionary;
        }

        public List<ApprovalTransactionDetails> GetApprovalTransactionDetails()
        {
            return _context.ApprovalTransactionDetails.ToList();
        }

        public List<ApprovalTransactionDetailsDTO> ApprovalTransactionDetails(Guid entityId)
        {
            return (from ApprovalTransactionDetail in _context.ApprovalTransactionDetails

                    join ApprovalTran in _context.ApprovalTransactions on ApprovalTransactionDetail.ApprovalTransactionId equals ApprovalTran.ApprovalTransactionId
                    join ApprovalAction in _context.ApprovalActions on ApprovalTransactionDetail.ApprovalActionId equals ApprovalAction.ApprovalActionId
                    join users in _context.Users on ApprovalTransactionDetail.CreatedBy equals users.UserId

                    where (ApprovalTran.EntityId == entityId)

                    select new ApprovalTransactionDetailsDTO
                    {
                        ApprovalActionName = ApprovalAction.ApprovalActionName,
                        FirstName = users.FirstName,
                        LastName = users.LastName,
                        ActionPerformedDate = ApprovalTransactionDetail.ModifiedDate.Value,


                    }).ToList();
        }


        public int GetApprovalEventOrderNumber(ApprovalEventViewModel approvalEventViewModel)
        {
            return _context.ApprovalEvents.FirstOrDefault(x => (x.ApprovalEventName == approvalEventViewModel.ApprovalEventName) && (x.ApprovalId == approvalEventViewModel.ApprovalId)).ApprovalEventOrder;
        }

        public void CreateApprovalTransaction(ApprovalTransactions approvalTransaction)
        {
            _context.ApprovalTransactions.Add(approvalTransaction);
            _context.SaveChanges();
        }

        public ApprovalTransactions GetApprovalTransactionByEntity(Guid entityId)
        {
            return _context.ApprovalTransactions.Include(t => t.ApprovalAction).FirstOrDefault(x => x.EntityId == entityId && x.IsActive && !x.IsDeleted);
        }

        public void UpdateApprovalTransaction(ApprovalTransactions approvalTransaction, ApprovalTransactionDetails approvalTransactionDetail)
        {
            _context.ApprovalTransactionDetails.Add(approvalTransactionDetail);
            _context.Entry(approvalTransaction).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void AddApprovalTransactionDetails(ApprovalTransactionDetails approvalTransactionDetail)
        {
            _context.ApprovalTransactionDetails.Add(approvalTransactionDetail);
            _context.SaveChanges();
        }

        public List<Users> GetApprovedUsersByRole(int roleId, int approvalEventId)
        {
            return _context.ApprovalEventRoles.Include(t => t.User).Where(x => x.ApprovalEventId == approvalEventId
             && x.RoleId == roleId && x.IsActive && !x.IsDeleted).Select(x => x.User).ToList();
        }

        public List<Users> GetApprovedUsers(int approvalEventId)
        {
            return _context.ApprovalEventRoles.Where(x => x.ApprovalEventId == approvalEventId
             && x.IsActive && !x.IsDeleted).Select(x => x.User).ToList();
        }

        public List<ApprovalTransactions> GetAllApprovalTransactions(List<Guid> openingIds)
        {
            return _context.ApprovalTransactions.Include(t => t.ApprovalAction).Where(x => openingIds.Contains(x.EntityId) && (x.IsActive && !x.IsDeleted)).ToList();
        }

        public int GetApprovalEventOrderOfUser(Guid entityId, Guid userId, int approvalId)
        {
            if (approvalId == (int)Approval.Opening)
            {
                var scheduledUser = _context.ApprovalEventRoles.Include(x => x.ApprovalEvent).FirstOrDefault(x => x.UserId == userId && x.IsActive && !x.IsDeleted);
                return scheduledUser.ApprovalEvent.ApprovalEventOrder;
            }
            else
            {
                var scheduledUser = _context.ScheduleUserForCandidate.Include(x => x.ApprovalEvent).Where(x => x.UserId == userId && x.CandidateId == entityId && x.IsActive && !x.IsDeleted).OrderByDescending(x => x.ApprovalEventId).First();
                return scheduledUser.ApprovalEvent.ApprovalEventOrder;
            }
        }

        public int GetTotalOpenOpenings()
        {
            return _context.ApprovalTransactions.Where(x => x.IsApproved && x.ApprovalId == (int)Approval.Opening && DateTime.Now.Month == x.ModifiedDate.Value.Month && x.IsActive && !x.IsDeleted).Count();
        }

        public int GetTotalCloseOpenings()
        {
            return 0;
        }

        public int GetTotalCandidatesHired()
        {
            return _context.ApprovalTransactions.Where(x => x.IsApproved && x.ApprovalId == (int)Approval.Candidate && DateTime.Now.Month == x.ModifiedDate.Value.Month && x.IsActive && !x.IsDeleted).Count();
        }

        public Users GetUserForCandidateApproval(Guid entityId, int nextEventOrderNumber)
        {
            return _context.CandidateAssignedUser.Include(s => s.ApprovalEvent).Include(t => t.User).FirstOrDefault(x => x.CandidateId == entityId && x.ApprovalEvent.ApprovalEventOrder == nextEventOrderNumber && x.IsActive && !x.IsDeleted).User;
        }

        public int GetTotalCandidatesAttendedInterview(int month)
        {
            return _context.ApprovalTransactions.Where(x => x.ApprovalId == (int)Approval.Candidate && x.ModifiedDate.Value.Month == month && x.IsActive && !x.IsDeleted).Count();
        }

        public List<SeriesModel> GetSeriesDetail(int showType)
        {
            List<SeriesModel> series = new List<SeriesModel>();
            if (showType == (int)ShowType.All)
            {
                var approvedOpenings = _context.ApprovalTransactions.Where(x => x.ApprovalId == (int)Approval.Opening && x.IsApproved && x.IsActive && !x.IsDeleted).Select(x => x.EntityId).ToList();
                var openings = new List<String>();
                approvedOpenings.ForEach(x =>
                {
                    openings.Add(_context.Openings.FirstOrDefault(s => s.OpeningId == x && s.IsActive && !s.IsDeleted).Title);
                });
                openings.ForEach(opening =>
                {
                    SeriesModel seriesModel = new SeriesModel();
                    seriesModel.Data = new List<int>();
                    seriesModel.Name = opening;
                    for (int i = 1; i <= 12; i++)
                    {
                        seriesModel.Data.Add(GetCandidatesHired(i, opening));
                    }
                    series.Add(seriesModel);
                });
            }
            else
            {
                var gender = _context.Candidates.Select(x => x.Gender).Distinct().ToList();
                gender.ForEach(x =>
                {
                    SeriesModel seriesModel = new SeriesModel();
                    seriesModel.Data = new List<int>();
                    seriesModel.Name = x == 1 ? "Male" : "Female";
                    for (int i = 1; i <= 12; i++)
                    {
                        seriesModel.Data.Add(GetCandidatesHiredBasedOnGender(i, x));
                    }
                    series.Add(seriesModel);
                });
            }
            SeriesModel totalCandidateInterviewed = new SeriesModel();
            totalCandidateInterviewed.Name = "Total Candidate Interviewed";
            totalCandidateInterviewed.Data = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                totalCandidateInterviewed.Data.Add(GetTotalCandidatesAttendedInterview(i));
            }
            series.Add(totalCandidateInterviewed);
            return series;
        }

        private int GetCandidatesHired(int i, string opening)
        {
            return _context.ApprovalTransactions.Where(x => x.IsApproved && GetOpening(x.EntityId) == opening && x.ApprovalId == (int)Approval.Candidate && x.ModifiedDate.Value.Month == i && x.IsActive && !x.IsDeleted).Count();
        }

        private string GetOpening(Guid candidateId)
        {
            var openingCandidate = _context.OpeningCandidates.Include(t => t.Opening).FirstOrDefault(x => x.CandidateId == candidateId && x.IsActive && !x.IsDeleted);
            if (openingCandidate != null)
            {
                return openingCandidate.Opening.Title;
            }
            return null;
        }

        private int GetCandidatesHiredBasedOnGender(int i, int gender)
        {
            return _context.ApprovalTransactions.Where(x => x.IsApproved && GetGender(x.EntityId) == gender && x.ApprovalId == (int)Approval.Candidate && x.ModifiedDate.Value.Month == i && x.IsActive && !x.IsDeleted).Count();
        }

        private int GetGender(Guid candidateId)
        {
            var candidate = _context.Candidates.FirstOrDefault(x => x.CandidateId == candidateId && x.IsActive && !x.IsDeleted);
            if (candidate != null)
            {
                return candidate.Gender;
            }
            return 0;
        }

        public List<Users> GetUserForOpeningApproval(ApprovalTransactionViewModel approvalTransactionViewModel)
        {
            return _context.ApprovalEventRoles.Include(t => t.User).Where(x => x.ApprovalEventId == approvalTransactionViewModel.NextEventOrderNumber && x.IsActive && !x.IsDeleted).Select(x => x.User).ToList();
        }

        public int GetNextEventOrderForCandidate(Guid candidateId)
        {
            var eventOrder = 1;
            var entityList = _context.ApprovalTransactions.Where(x => x.IsActive && !x.IsDeleted).Select(x => x.EntityId).ToList();
            if (entityList.Contains(candidateId))
            {
                eventOrder = _context.ApprovalTransactions.FirstOrDefault(x => x.EntityId == candidateId && x.IsActive && !x.IsDeleted).NextEventOrderNumber;
                if (eventOrder == -1 || eventOrder == 0)
                {
                    return _context.ApprovalTransactions.FirstOrDefault(x => x.EntityId == candidateId && x.IsActive && !x.IsDeleted).EventOrderNumber;
                }
            }
            return eventOrder;
        }

        public bool CheckForStartInterview(Guid candidateId, int approvalEventId, Guid userId)
        {
            var interviewStarted = _context.ScheduleUserForCandidate.FirstOrDefault(x => x.ApprovalEventId == approvalEventId && x.CandidateId == candidateId && x.IsStarted && (x.IsActive && !x.IsDeleted));
            if (interviewStarted != null && interviewStarted.UserId != userId)
            {
                return true;
            }
            _context.ScheduleUserForCandidate.FirstOrDefault(x => x.ApprovalEventId == approvalEventId && x.UserId == userId && x.CandidateId == candidateId && (x.IsActive && !x.IsDeleted)).IsStarted = true;
            _context.SaveChanges();
            return false;
        }

        public Dictionary<string, List<int>> GetApprovalEventsOfUserForCandidate(Guid candidateId, Guid UserId)
        {
            Dictionary<string, List<int>> dictionary = new Dictionary<string, List<int>>();
            var approvalEvent = _context.ScheduleUserForCandidate.Where(x => x.UserId == UserId && x.CandidateId == candidateId && x.IsActive && !x.IsDeleted).Max(x => x.ApprovalEventId);

            string approvalName = _context.ApprovalEvents.Include(s => s.Approval).FirstOrDefault(t => t.ApprovalEventId == approvalEvent).Approval.ApprovalName;

            List<int> eventIds = new List<int>();
            eventIds.Add(approvalEvent);
            dictionary.Add(approvalName, eventIds);

            return dictionary;
        }
    }
}
