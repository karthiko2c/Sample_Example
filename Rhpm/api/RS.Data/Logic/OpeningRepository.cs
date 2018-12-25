using Microsoft.EntityFrameworkCore;
using RS.Common.Enums;
using RS.Data;
using RS.Data.Interfaces;
using RS.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RS.Data.Logic
{
    public class OpeningRepository : Repository<Openings>, IOpeningRepository
    {
        private readonly RSContext _context;
        public OpeningRepository(RSContext context) : base(context)
        {

            this._context = context;
        }

        public void CreateOpening(Openings opening, List<OpeningSkills> openingSkills)
        {
            opening.OpeningSkills = openingSkills;
            _context.Openings.Add(opening);
            _context.SaveChanges();
        }

        public Openings GetByID(Guid openingId)
        {
            return _context.Openings.Include(t => t.OpeningSkills).ThenInclude(r => r.Skill).FirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.OpeningId == openingId));
        }

        public List<Openings> GetOpeningsCorrespondingToLoggedUser(Guid userId)
        {
            var openings = new List<Openings>();
            var eventRole = _context.ApprovalEventRoles.Include(x => x.ApprovalEvent).FirstOrDefault(x => x.UserId == userId);
            if (eventRole != null && eventRole.ApprovalEvent.ApprovalId == (int)Approval.Opening)
            {
                openings = _context.Openings.Include(t => t.OpeningSkills).ThenInclude(r => r.Skill).Where(x => (x.IsActive && !x.IsDeleted)).OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.ModifiedDate).ToList();
            }
            return openings;
        }

        public void UpdateOpeningSkills(List<OpeningSkills> openingSkills)
        {
            openingSkills.ForEach(x => _context.OpeningSkills.Add(x));
        }

        List<Openings> IOpeningRepository.GetAll()
        {
            var openings = new List<Openings>();
            var openingIds = _context.ApprovalTransactions.Where(x => x.ApprovalId == (int)Approval.Opening && x.IsApproved && (x.IsActive && !x.IsDeleted)).Select(x => x.EntityId).ToList();
            if (openingIds.Any())
            {
                return _context.Openings.Include(t => t.OpeningSkills).ThenInclude(r => r.Skill).Where(x => openingIds.Contains(x.OpeningId) && (x.IsActive && !x.IsDeleted)).OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.ModifiedDate).ToList();
            }
            return openings;
        }
    }
}
