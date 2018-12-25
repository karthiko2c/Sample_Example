using System;
using System.Threading.Tasks;
using SubQuip.Entity.Models.Graph;

namespace SubQuip.Data.Interfaces
{
    public interface IGraphRepository
    {
        /// <summary>
        /// Get External User Details.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
		Task<Users> FindExternalUser(string email);

        /// <summary>
        /// Request For User Details.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User> RequestUserDetails(string email);

        /// <summary>
        /// Get list of application users
        /// </summary>
        /// <returns></returns>
        Task<Users> ApplicationUsers();

    }
}
