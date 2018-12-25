using Microsoft.EntityFrameworkCore;
using RS.Data;
using RS.Data.Interfaces;
using RS.Entity.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RS.Data.Logic
{
    public class CandidateRepository : Repository<Candidates>, ICandidateRepository
    {
        private readonly RSContext _context;
        public CandidateRepository(RSContext context) : base(context)
        {
            this._context = context;
        }

        public void AddCandidate(Candidates candidate, OpeningCandidates openingCandidate)
        {
            _context.Candidates.Add(candidate);
            _context.OpeningCandidates.Add(openingCandidate);
            _context.SaveChanges();
        }

        public void UpdateCandidate(Candidates candidate, OpeningCandidates openingCandidate)
        {
            if (openingCandidate.Opening != null)
            {
                _context.Entry(openingCandidate).State = EntityState.Modified;
            }

            var candidateDocument = candidate.CandidateDocuments.FirstOrDefault();
            if (candidateDocument.CandidateDocumentId != Guid.Empty)
            {
                _context.Entry(candidateDocument).State = EntityState.Modified;
            }

            _context.Entry(candidate).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public Candidates GetByID(Guid candidateId)
        {
            return _context.Candidates.Include(s => s.CandidateDocuments).Include(t => t.Organisation).Include(r => r.Qualification).FirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.CandidateId == candidateId));
        }

        public OpeningCandidates GetOpeningCandidate(Guid candidateId)
        {
            return _context.OpeningCandidates.Include(t => t.Opening).FirstOrDefault(x => (x.CandidateId == candidateId) && (x.IsActive && !x.IsDeleted));
        }

        List<Candidates> ICandidateRepository.GetAll()
        {
            return _context.Candidates.Include(t => t.Organisation).Include(s => s.CandidateDocuments).Where(x => (x.IsActive && !x.IsDeleted)).ToList();
        }

        public void AssignUserForCandidate(CandidateAssignedUser candidateAssignedUser)
        {
            _context.CandidateAssignedUser.Add(candidateAssignedUser);
            _context.SaveChanges();
        }

        public List<CandidateAssignedUser> GetAssignedUsersByID(Guid candidateId)
        {
            return _context.CandidateAssignedUser.Where(x => x.CandidateId == candidateId && (x.IsActive && !x.IsDeleted)).ToList();
        }

        public List<ScheduleUserForCandidate> GetCandidatesCorrespondingToLoggedUser(Guid userId)
        {
            var ScheduleUserForCandidateList = _context.ScheduleUserForCandidate.Include(t => t.Candidate).Where(x => x.UserId == userId && (x.IsActive && !x.IsDeleted)).Select(x => x.CandidateId).Distinct().ToList();
            var scheduleUserList = new List<ScheduleUserForCandidate>();
            ScheduleUserForCandidateList.ForEach(x =>
            {
                var scheduleUser = _context.ScheduleUserForCandidate.Include(t => t.Candidate).Where(t => t.CandidateId == x).OrderByDescending(t => t.ApprovalEventId).First();
                scheduleUserList.Add(scheduleUser);
            });
            return scheduleUserList;
        }

        public void ApprovedForInterview(Candidates candidate)
        {
            _context.Entry(candidate).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<Organizations> GetOrganizationsOnInputChanged(string input)
        {
            return _context.Organizations.Where(x => x.Name.Substring(0, input.Length).Equals(input) && (x.IsActive && !x.IsDeleted)).ToList();
        }

        public void AddScheduledUsers(ScheduleUserForCandidate scheduleUser)
        {
            _context.ScheduleUserForCandidate.Add(scheduleUser);
        }

        public List<ScheduleUserForCandidate> GetScheduledUserByApprovalEvent(ScheduleUserForCandidate scheduleUser)
        {
            return _context.ScheduleUserForCandidate.Include(t => t.ApprovalEvent).Include(s => s.User).Where(x => x.CandidateId == scheduleUser.CandidateId && x.ApprovalEventId == scheduleUser.ApprovalEventId && (x.IsActive && !x.IsDeleted)).ToList();
        }

        public List<ScheduleUserForCandidate> GetScheduledUsersById(Guid candidateId)
        {
            return _context.ScheduleUserForCandidate.Include(t => t.ApprovalEvent).Include(s => s.User).Where(x => x.CandidateId == candidateId && (x.IsActive && !x.IsDeleted)).ToList();
        }

        public void OnInterviewFinished(ApprovalTransactions approvalTransaction, Guid userId)
        {
            _context.ScheduleUserForCandidate.FirstOrDefault(x => x.CandidateId == approvalTransaction.EntityId && x.ApprovalEvent.ApprovalEventOrder == approvalTransaction.EventOrderNumber && x.UserId == userId && (x.IsActive && !x.IsDeleted)).IsFinished = true;
            _context.SaveChanges();
        }

        public void OnCandidateApproved(ApprovalTransactions approvalTransaction)
        {
            _context.Candidates.FirstOrDefault(x => x.CandidateId == approvalTransaction.EntityId && (x.IsActive && !x.IsDeleted)).IsApproved = true;
            _context.SaveChanges();
        }

        public bool CheckForInterviewCompletion(ScheduleUserForCandidate scheduleUserForCandidate)
        {
            var interviewCompleted = _context.ScheduleUserForCandidate.Where(x => x.ApprovalEventId == scheduleUserForCandidate.ApprovalEventId && x.CandidateId == scheduleUserForCandidate.CandidateId && x.IsFinished && (x.IsActive && !x.IsDeleted)).ToList();
            if (interviewCompleted.Any())
            {
                return true;
            }
            return false;
        }
    }
}
