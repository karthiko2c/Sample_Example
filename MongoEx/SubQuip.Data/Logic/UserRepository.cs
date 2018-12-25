using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SubQuip.Common.CommonData;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models.AppUser;
using System.Linq;

namespace SubQuip.Data.Logic
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        public IConfiguration Configuration;

        /// <summary>
        /// Initializes a new instance of the UserRepository
        /// </summary>
        /// <param name="configuration"></param>
        public UserRepository(IConfiguration configuration) : base(configuration, "user")
        {
            Configuration = configuration;
        }
    }
}
