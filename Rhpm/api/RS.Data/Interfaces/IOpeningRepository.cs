using RS.Entity.Models;
using System;
using System.Collections.Generic;

namespace RS.Data.Interfaces
{
    public interface IOpeningRepository : IRepository<Openings>
    {
        void CreateOpening(Openings opening, List<OpeningSkills> openingSkills);

        void UpdateOpeningSkills(List<OpeningSkills> openingSkills);

        Openings GetByID(Guid openingId);

        new List<Openings> GetAll();

        List<Openings> GetOpeningsCorrespondingToLoggedUser(Guid userId);

    }
}
