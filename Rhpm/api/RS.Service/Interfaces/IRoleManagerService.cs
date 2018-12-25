using RS.Common.Enums;
using RS.ViewModel.Roles;
using System;
using RS.Common.CommonData;

namespace RS.Service.Interfaces
{
    public interface IRoleManagerService
    {
        /// <summary>
        /// Get Role Info
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        IResult GetAllRole();
        /// <summary>
        /// Delete a  Role
        /// </summary>
        /// <param name="id">role id</param>
        /// <returns></returns>
        IResult GetRoleById(int id);
    }
}
