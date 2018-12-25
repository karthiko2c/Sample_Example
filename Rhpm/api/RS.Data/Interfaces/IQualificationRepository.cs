using RS.Entity.Models;
using RS.ViewModel.SearchAndSortModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RS.Data.Interfaces
{
    public interface IQualificationRepository : IRepository<Qualifications>
    {
        List<Qualifications> GetAll(SearchAndSortModel searchAndSortModel);
    }
}
