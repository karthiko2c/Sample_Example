using RS.Data;
using RS.Data.Interfaces;
using RS.Entity.Models;

namespace RS.Data.Logic
{
    public class RoleRepository : Repository<Roles>, IRoleRepository
    {
        private readonly RSContext _context;
        public RoleRepository(RSContext context) : base(context) {
          
                this._context = context;
        }

    }
}
