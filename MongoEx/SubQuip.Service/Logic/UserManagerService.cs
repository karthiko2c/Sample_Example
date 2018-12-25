using MongoDB.Bson;
using MongoDB.Driver;
using SubQuip.Business.Interfaces;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;
using SubQuip.Common.Extensions;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models.AppUser;
using SubQuip.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace SubQuip.Business.Logic
{
    public class UserManagerService : IUserManagerService
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the UserManagerService
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="userRepository"></param>
        public UserManagerService(IPrincipal principal, IUserRepository userRepository)
        {
            _principal = principal as ClaimsPrincipal;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get All Tabs For User.
        /// </summary>
        /// <returns></returns>
        public IResult GetTabsForUser()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
    
                var mail = ((ClaimsIdentity)_principal.Identity).GetActiveUserId();
                if (!string.IsNullOrEmpty(mail))
                {
                    var savedTabList = new List<SavedTabViewModel>();
                    var user = _userRepository.GetChildDocument(x => x.Mail.Equals(mail), "SavedTabs", "UserId");
                    if(user.SavedTabs != null && user.SavedTabs.Any())
                    {
                        user.SavedTabs = user.SavedTabs.OrderByDescending(x => x.CreatedDate).ToList();
                        savedTabList = savedTabList.MapFromModel<SavedTab, SavedTabViewModel>(user.SavedTabs);
                        result.Body = savedTabList;
                    }
                    else
                    {
                        result.Body = savedTabList;
                        result.Message = CommonErrorMessages.NoResultFound;
                    }
                }
                else
                {
                    result.Message = UserNotification.MailNotFound;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Save User and Tab Details.
        /// </summary>
        /// <param name="savedTabViewModel"></param>
        /// <returns></returns>
        public IResult SaveTabDetail(SavedTabViewModel savedTabViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                SavedTab savedTabDetail = null;
                if (savedTabViewModel != null)
                {
                    savedTabDetail = new SavedTab();
                    savedTabDetail.MapFromViewModel(savedTabViewModel, (ClaimsIdentity)_principal.Identity);
                    savedTabDetail.TabId = ObjectId.GenerateNewId();
                    var mail = ((ClaimsIdentity)_principal.Identity).GetActiveUserId();
                    if (!string.IsNullOrEmpty(mail))
                    {
                        var user = _userRepository.GetOne(x => x.Mail.Equals(mail));
                        if (user == null)
                        {
                            user = new AppUser();
                            user.Mail = mail;
                            user.SavedTabs = new List<SavedTab>();
                            _userRepository.InsertOne(user);
                        }
                        var updateDefinition = Builders<AppUser>.Update.AddToSet(t => t.SavedTabs, savedTabDetail);
                        _userRepository.UpdateOne(t => t.UserId.Equals(user.UserId), updateDefinition);
                        result.Message = UserNotification.TabDetailsSaved;
                        result.Body = savedTabViewModel.MapFromModel(savedTabDetail);
                    }
                    else
                    {
                        result.Message = UserNotification.MailNotFound;
                    }
                }
                else
                {
                    result.Message = UserNotification.NoTabDetails;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }
    }
}
