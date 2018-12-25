using RS.Entity.Models;
using RS.ViewModel.SearchAndSortModel;
using RS.ViewModel.Skill;
using System;
using System.Collections.Generic;
using System.Text;

namespace RS.Data.Interfaces
{
    public interface ISkillRepository : IRepository<Skills>
    {
        List<Skills> GetAll(SearchAndSortModel searchAndSortModel);
    }
}
