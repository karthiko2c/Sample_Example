using RS.Common.CommonData;
using RS.Common.Enums;
using RS.ViewModel.SearchAndSortModel;
using RS.ViewModel.User;
using System;

namespace RS.Service.Interfaces
{
    public interface IUserManagerService
    {
        IResult ChangePassword(string oldPassword, string newPassword);

        IResult LoginUser(string userName, string password);

        IResult ForgotPassword(String userName);

        IResult CreateUser(UserViewModel user);

        IResult UpdateUser(UserViewModel user);

      //  IResult UpdateUserProfile(UserViewModel user);

        IResult GetAllUser();

        IResult GetUserById(Guid id);

        IResult GetUsersByRole(int roleId);

        IResult GetUserDetail();
        IResult GetUsersResults(SearchAndSortModel searchAndSortModel);

    }
}
