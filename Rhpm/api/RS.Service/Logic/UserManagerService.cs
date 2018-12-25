using RS.Service.Interfaces;
using RS.Common.Enums;
using RS.Common.Extensions;
using RS.ViewModel.User;
using System;
using System.Linq;
using RS.Data.Interfaces;
using RS.Common.CommonData;
using RS.Entity.Models;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using RS.ViewModel.SearchAndSortModel;

namespace RS.Service.Logic
{
    public class UserManagerService : IUserManagerService
    {
        #region Global Variables
        private readonly ClaimsPrincipal _principal;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IApprovalRepository _approvalRepository;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        #endregion
        public UserManagerService(IPrincipal principal, IUserRepository userRepository, IRoleRepository roleRepository,
            IApprovalRepository approvalRepository, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._approvalRepository = approvalRepository;
            this._configuration = configuration;
            this._principal = principal as ClaimsPrincipal;
            this._hostingEnvironment = hostingEnvironment;
        }

        public IResult ChangePassword(string oldPassword, string newPassword)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var authorizedUser = GenericHelper.GetUserClaimDetails((ClaimsIdentity)_principal.Identity);
                var user = _userRepository.GetUser(authorizedUser.UserId, oldPassword);
                if (user.UserId != null)
                {
                    user.Password = newPassword;
                    _userRepository.Update(user);
                    _userRepository.SaveChanges();
                    result.Body = user.UserId;
                }
                else
                {
                    result.Body = null;
                    result.Status = Status.Fail;
                    result.Message = CommonErrorMessages.UserNotFound;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult CreateUser(UserViewModel userViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var duplicateUserName = _userRepository.GetFirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.UserName == userViewModel.UserName));
                var duplicateEmail = _userRepository.GetFirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.Email == userViewModel.Email));
                if (duplicateUserName != null)
                {
                    result.Status = Status.Fail;
                    result.Message = UserStatusNotification.DuplicateUserName;
                    result.Body = null;
                    return result;
                }
                else if (duplicateEmail != null)
                {
                    result.Status = Status.Fail;
                    result.Message = UserStatusNotification.DuplicateEmail;
                    result.Body = null;
                    return result;
                }
                else
                {
                    var userModel = new Users();
                    userModel.MapFromViewModel(userViewModel, (ClaimsIdentity)_principal.Identity);

                    UserRoles userRole = new UserRoles();
                    userRole.RoleId = userViewModel.RoleId;
                    userRole.Role = _roleRepository.GetByID(userViewModel.RoleId);
                    userRole.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                    _userRepository.CreateUser(userModel, userRole);
                    userViewModel.Password = GenericHelper.DecryptPassword(userViewModel.Password);
                    if (userModel.UserId != Guid.Empty)
                    {
                        MailDetailModel mailDetail = new MailDetailModel();
                        mailDetail.EmailId = userModel.Email;
                        mailDetail.Subject = "Registration Confirmation";
                        mailDetail.Template = TemplateType.UserRegistration;
                        mailDetail.MessageBody = userViewModel;
                        GenericHelper.Send(mailDetail, _configuration, _hostingEnvironment);
                    }
                    result.Body = userModel.UserId;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult ForgotPassword(string userName)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var user = _userRepository.GetEmailIdByUserName(userName);
                if (user.UserId != null)
                {
                    result.Body = user.Email;
                }
                else
                {
                    result.Body = null;
                    result.Status = Status.Fail;
                    result.Message = CommonErrorMessages.UserNotFound;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetAllUser()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var allUsers = _userRepository.GetAll().ToList();
                result.Body = allUsers.Select(user =>
                {
                    var getUserRole = user.UserRoles.FirstOrDefault(x => (x.IsActive && !x.IsDeleted));
                    var userViewModel = new UserViewModel();
                    if (getUserRole != null)
                    {
                        userViewModel.RoleId = getUserRole.RoleId;
                        userViewModel.Role = getUserRole.Role.Name;
                    }
                    return userViewModel.MapFromModel(user);
                }).ToList();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetUserById(Guid id)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var getUser = _userRepository.GetByID(id);
                var getUserRole = getUser.UserRoles.FirstOrDefault(x => (x.IsActive && !x.IsDeleted));
                var user = new UserViewModel();
                if (getUserRole != null)
                {
                    user.RoleId = getUserRole.RoleId;
                    user.Role = getUserRole.Role.Name;
                }
                result.Body = user.MapFromModel(getUser);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetUserDetail()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var userModel = new UserViewModel();
                var identity = (ClaimsIdentity)_principal.Identity;
                var user = GenericHelper.GetUserClaimDetails(identity);
                result.Body =  userModel.MapFromModel(user);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetUsersByRole(int roleId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var users = _userRepository.GetUsersByRole(roleId).ToList();
                result.Body = users.Select(user =>
                {
                    var getUserRole = user.UserRoles.FirstOrDefault(x => (x.IsActive && !x.IsDeleted));
                    var userViewModel = new UserViewModel();
                    userViewModel.FullName = user.FirstName + " " + user.LastName;
                    if (getUserRole != null)
                    {
                        userViewModel.RoleId = getUserRole.RoleId;
                        userViewModel.Role = getUserRole.Role.Name;
                    }
                    return userViewModel.MapFromModel(user);
                }).ToList();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        #region public methods

        public IResult LoginUser(string username, string password)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var user = _userRepository.LoginUser(username, password);
                if (user != null)
                {
                    var userView = new UserViewModel();
                    userView.MapFromModel(user, "UserName;");
                    userView.FullName = user.FirstName + " " + user.LastName;
                    var firstOrDefault = user.UserRoles.FirstOrDefault();
                    if (firstOrDefault != null) userView.Role = firstOrDefault.Role.Name;
                    userView.ApprovalDetail = _approvalRepository.GetApprovalEventsOfUserForOpening(user.UserId);
                    result.Body = userView;
                }
                else
                {
                    result.Message = UserStatusNotification.InValidUser;
                    result.Status = Status.Fail;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
                result.Status = Status.Error;

            }
            return result;
        }

        public IResult UpdateUser(UserViewModel user)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                var duplicateUserName = _userRepository.GetFirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.UserName == user.UserName) && (x.UserId != user.UserId));
                if (duplicateUserName != null)
                {
                    result.Status = Status.Fail;
                    result.Message = UserStatusNotification.DuplicateUserName;
                    result.Body = null;
                    return result;
                }

                var duplicateEmail = _userRepository.GetFirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.Email == user.Email) && (x.UserId != user.UserId));
                if (duplicateEmail != null)
                {
                    result.Status = Status.Fail;
                    result.Message = UserStatusNotification.DuplicateEmail;
                    result.Body = null;
                    return result;
                }

                var userModel = _userRepository.GetByID(user.UserId);
                var userRoleModel = userModel.UserRoles.Where(x => (x.IsActive && !x.IsDeleted) && x.RoleId != user.RoleId).ToList();
                userModel.MapFromViewModel(user, (ClaimsIdentity)_principal.Identity);

                if (userRoleModel.Any())
                {
                    userRoleModel.ForEach(x => x.MapDeleteColumns((ClaimsIdentity)_principal.Identity));
                    var userRole = new UserRoles();
                    userRole.UserId = user.UserId;
                    userRole.RoleId = user.RoleId;
                    userRole.Role = _roleRepository.GetByID(user.RoleId);
                    userRole.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                    _userRepository.UpdateUserRole(userRole);
                }
                _userRepository.UpdateUser(userModel);
                result.Body = userModel.UserId;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        //public IResult UpdateUserProfile(UserViewModel user)
        //{
        //    var result = new Result
        //    {
        //        Operation = Operation.Update,
        //        Status = Status.Success
        //    };
        //    try
        //    {
        //        var userId = GenericHelper.GetUserClaimDetails((ClaimsIdentity)_principal.Identity).UserId;
        //        var duplicateUserName = _userRepository.GetFirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.UserName == user.UserName) && (x.UserId != userId));
        //        if (duplicateUserName != null)
        //        {
        //            result.Status = Status.Fail;
        //            result.Message = UserStatusNotification.DuplicateUserName;
        //            result.Body = null;
        //            return result;
        //        }

        //        var duplicateEmail = _userRepository.GetFirstOrDefault(x => (x.IsActive && !x.IsDeleted) && (x.Email == user.Email) && (x.UserId != userId));
        //        if (duplicateEmail != null)
        //        {
        //            result.Status = Status.Fail;
        //            result.Message = UserStatusNotification.DuplicateEmail;
        //            result.Body = null;
        //            return result;
        //        }


        //        var userModel = _userRepository.GetByID(userId);
        //        userModel.UserId = userId;
        //        userModel.MapFromViewModel((ClaimsIdentity)_principal.Identity);
        //        userModel.FirstName = user.FirstName;
        //        userModel.LastName = user.LastName;
        //        userModel.Email = user.Email;
        //        userModel.UserName = user.UserName;
        //        _userRepository.UpdateUser(userModel);
        //        result.Body = userModel;
        //    }
        //    catch (Exception e)
        //    {
        //        result.Message = e.Message;
        //        result.Status = Status.Error;
        //    }
        //    return result;
        //}



        public IResult GetUsersResults(SearchAndSortModel searchAndSortModel)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var userList = _userRepository.GetAll(searchAndSortModel).ToList();

                result.Body = userList.Select(user =>
                {
                    var getUserRole = user.UserRoles.FirstOrDefault(x => (x.IsActive && !x.IsDeleted));
                    var userViewModel = new UserViewModel();
                    if (getUserRole != null)
                    {
                        userViewModel.RoleId = getUserRole.RoleId;
                        userViewModel.Role = getUserRole.Role.Name;
                    }
                    return userViewModel.MapFromModel(user);
                }).ToList();

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        #endregion
    }
}
