using Microsoft.EntityFrameworkCore;
using RS.Data.Interfaces;
using RS.Entity.Models;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using RS.ViewModel.SearchAndSortModel;

namespace RS.Data.Logic
{
    public class QualificationRepository : Repository<Qualifications>, IQualificationRepository
    {
        private readonly RSContext _context;
        public QualificationRepository(RSContext context) : base(context) {

            this._context = context;
        }
        List<Qualifications> IQualificationRepository.GetAll(SearchAndSortModel searchAndSortModel)
        {
            var qualificationList = _context.Qualifications.Where(x => (x.IsActive && !x.IsDeleted));

            if(searchAndSortModel.SearchString != null)
                {
                     qualificationList = qualificationList.Where(x => x.Name.ToLower().Contains(searchAndSortModel.SearchString.ToLower()));
                }
            if(searchAndSortModel.Property != null)
            {
                if(searchAndSortModel.Direction == 1)
                {
                    qualificationList = qualificationList.OrderBy(x => x.GetType().GetProperty(searchAndSortModel.Property).GetValue(x, null));
                }
                else
                {
                    qualificationList = qualificationList.OrderByDescending(x => x.GetType().GetProperty(searchAndSortModel.Property).GetValue(x, null));
                }
            }
            return qualificationList.ToList();
        }
    }
}
