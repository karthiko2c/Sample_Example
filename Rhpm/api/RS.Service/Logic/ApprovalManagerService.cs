using Microsoft.Extensions.Configuration;
using RS.Common.CommonData;
using RS.Common.Enums;
using RS.Common.Extensions;
using RS.Data.Interfaces;
using RS.Entity.Models;
using RS.Service.Interfaces;
using RS.ViewModel.Approval;
using RS.ViewModel.ChartViewModel;
using RS.ViewModel.Dashboard;
using RS.ViewModel.Opening;
using RS.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using RS.ViewModel.Candidate;

namespace RS.Service.Logic
{
    public class ApprovalManagerService : IApprovalManagerService
    {
        #region Global Variables
        private readonly ClaimsPrincipal _principal;
        private readonly IApprovalRepository _approvalRepository;
        private readonly IOpeningRepository _openingRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IConfiguration _configuration;
        #endregion
        public ApprovalManagerService(IPrincipal principal, IApprovalRepository approvalRepository, IOpeningRepository openingRepository, ICandidateRepository candidateRepository,
            IConfiguration configuration)
        {
            _principal = principal as ClaimsPrincipal;
            _approvalRepository = approvalRepository;
            _openingRepository = openingRepository;
            _candidateRepository = candidateRepository;
            _configuration = configuration;
        }

        public IResult GetApprovalEvents(int approvalId, Guid entityId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var approvalEventAndTransactionDetail = new ApprovalEventAndTransactionDetail();
                var data = _approvalRepository.GetApprovalEvents(approvalId);
                if (data.Any())
                {
                    approvalEventAndTransactionDetail.approvalEventViewModel = data.Select(t =>
                    {
                        var eventViewModel = new ApprovalEventViewModel();
                        eventViewModel.MapFromModel(t);
                        if (t.ApprovalActions.Any())
                        {
                            var actionViewModel = new List<ApprovalActionViewModel>();
                            eventViewModel.ApprovalActions = actionViewModel
                                    .MapFromModel<ApprovalActions, ApprovalActionViewModel>(t.ApprovalActions.ToList());
                            var users = _approvalRepository.GetApprovedUsers(t.ApprovalEventId);
                            var userModels = new List<UserViewModel>();
                            eventViewModel.Users = userModels.MapFromModel<Users, UserViewModel>(users);
                        }
                        return eventViewModel;
                    }).ToList();
                }

                var userId = GetUserId();
                var permissibleEvent = 0;
                if (entityId != Guid.Empty)
                {
                    var approvalTransactionViewModel = new ApprovalTransactionViewModel();
                    var approvalTransactionModel = _approvalRepository.GetApprovalTransactionByEntity(entityId);
                    if (approvalTransactionModel != null)
                    {
                        approvalEventAndTransactionDetail.approvalTransactionViewModel = (ApprovalTransactionViewModel)approvalTransactionViewModel.MapFromModel(approvalTransactionModel);
                    }
                    permissibleEvent = _approvalRepository.GetApprovalEventOrderOfUser(entityId, userId, approvalId);
                }

                if (permissibleEvent > 0 && approvalEventAndTransactionDetail.approvalTransactionViewModel != null)
                {
                    approvalEventAndTransactionDetail.approvalTransactionViewModel.PermissibleEvent = permissibleEvent;
                }

                result.Body = approvalEventAndTransactionDetail;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult ManageApprovalEventRole(ApprovalEventRoleViewModel approvalEventRoleViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var eventRole = _approvalRepository.GetAllApprovalEventRole().FirstOrDefault(x => x.ApprovalEventId == approvalEventRoleViewModel.ApprovalEventId); ;
                var existingRole = new Roles();
                if (eventRole != null)
                {
                    existingRole = eventRole.Role;
                }
                if (existingRole.RoleId == 0)
                {
                    var users = approvalEventRoleViewModel.Users;
                    foreach (var user in users)
                    {
                        var approvalEventRoleModel = new ApprovalEventRoles();
                        approvalEventRoleModel.MapFromViewModel(approvalEventRoleViewModel, (ClaimsIdentity)_principal.Identity);
                        approvalEventRoleModel.UserId = user.UserId;
                        _approvalRepository.AddApprovalEventRole(approvalEventRoleModel);
                    }
                    result.Body = approvalEventRoleViewModel.ApprovalEventId;
                }
                else
                {
                    if (existingRole.RoleId == approvalEventRoleViewModel.RoleId)
                    {
                        result.Operation = Operation.Update;
                        UpdateApprovalEventRole(approvalEventRoleViewModel);
                        result.Body = approvalEventRoleViewModel.ApprovalEventId;
                    }
                    else
                    {
                        result.Message = "";
                        result.Status = Status.Fail;
                    }
                }
                _approvalRepository.SaveChanges();

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        void UpdateApprovalEventRole(ApprovalEventRoleViewModel approvalEventRoleViewModel)
        {
            var eventRoleList = _approvalRepository.GetAllApprovalEventRole().Where(x => x.ApprovalEventId == approvalEventRoleViewModel.ApprovalEventId && x.RoleId == approvalEventRoleViewModel.RoleId).ToList();
            var usersList = eventRoleList.Select(x => x.User).ToList();
            var usersViewList = approvalEventRoleViewModel.Users;

            var existingUsers = usersList.Select(x => x.UserId).Intersect(usersViewList.Select(x => x.UserId)).ToList();
            var addingUsers = usersViewList.Select(x => x.UserId).Except(existingUsers).ToList();
            var removingUsers = usersList.Select(x => x.UserId).Except(existingUsers).ToList();

            if (existingUsers.Any())
            {
                var eventRoles = eventRoleList.Where(x => existingUsers.Contains(x.UserId)).ToList();
                eventRoles.ForEach(x => x.MapAuditColumns((ClaimsIdentity)_principal.Identity));
            }

            if (removingUsers.Any())
            {
                var eventRoles = eventRoleList.Where(x => removingUsers.Contains(x.UserId)).ToList();
                eventRoles.ForEach(x => x.MapDeleteColumns((ClaimsIdentity)_principal.Identity));
            }

            if (addingUsers.Any())
            {
                foreach (var user in addingUsers)
                {
                    var approvalEventRoleModel = new ApprovalEventRoles();
                    approvalEventRoleModel.MapFromViewModel(approvalEventRoleViewModel, (ClaimsIdentity)_principal.Identity);
                    approvalEventRoleModel.UserId = user;
                    _approvalRepository.AddApprovalEventRole(approvalEventRoleModel);
                }
            }
        }

        public IResult GetAllApprovalEventRoles()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var approvalEventRoles = _approvalRepository.GetAllApprovalEventRole().ToList();
                if (approvalEventRoles.Any())
                {
                    result.Body = approvalEventRoles.Select(t =>
                    {
                        var eventRoleViewModel = new ApprovalEventRoleViewModel();
                        eventRoleViewModel.MapFromModel(t);
                        eventRoleViewModel.ApprovalEvent = t.ApprovalEvent.ApprovalEventName;
                        eventRoleViewModel.Role = t.Role.Name;
                        eventRoleViewModel.User = t.User.FirstName + " " + t.User.LastName;
                        return eventRoleViewModel;
                    }).ToList();
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetAllApprovals()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                result.Body = _approvalRepository.GetAllApprovals();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetApprovalTransactionByEntity(Guid openingId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var approvalTransactionViewModel = new ApprovalTransactionViewModel();
                var approvalTransactionModel = _approvalRepository.GetApprovalTransactionByEntity(openingId);
                result.Body = approvalTransactionViewModel.MapFromModel(approvalTransactionModel);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult ManageApprovalTransaction(EntityAndApprovalViewModel entityAndApprovalViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var approvalTransactionViewModel = entityAndApprovalViewModel.approvalTransactionViewModel;
                if (approvalTransactionViewModel.ApprovalTransactionId == 0)
                {
                    result.Body = AddApprovalTransaction(entityAndApprovalViewModel);
                }
                else
                {
                    var approvalTransactionModel = _approvalRepository.GetApprovalTransactionByEntity(approvalTransactionViewModel.EntityId);

                    if (approvalTransactionViewModel.NextEventOrderNumber == 0)
                    {
                        approvalTransactionModel.IsApproved = true;
                        if (approvalTransactionViewModel.ApprovalId == (int)Approval.Candidate)
                        {
                            _candidateRepository.OnCandidateApproved(approvalTransactionModel);
                        }
                    }

                    if (approvalTransactionModel.NextEventOrderNumber == approvalTransactionViewModel.NextEventOrderNumber)
                    {
                        approvalTransactionModel.IsFurtherActionRequired = true;
                    }
                    else
                    {
                        approvalTransactionModel.IsFurtherActionRequired = false;
                    }

                    approvalTransactionModel.MapFromViewModel(approvalTransactionViewModel, (ClaimsIdentity)_principal.Identity);
                    approvalTransactionViewModel.Action = approvalTransactionModel.ApprovalAction.ApprovalActionName;
                    var approvalTransactionDetail = new ApprovalTransactionDetails();
                    approvalTransactionDetail.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                    approvalTransactionDetail.ApprovalTransactionId = approvalTransactionViewModel.ApprovalTransactionId;
                    approvalTransactionDetail.ApprovalActionId = approvalTransactionViewModel.ApprovalActionId;
                    approvalTransactionDetail.EventOrderNumber = approvalTransactionViewModel.EventOrderNumber;
                    approvalTransactionDetail.Comments = approvalTransactionViewModel.Comments;
                    _approvalRepository.UpdateApprovalTransaction(approvalTransactionModel, approvalTransactionDetail);
                    var userId = GenericHelper.GetUserClaimDetails((ClaimsIdentity)_principal.Identity).UserId;
                    if (approvalTransactionViewModel.ApprovalId == (int)Approval.Candidate)
                    {
                        _candidateRepository.OnInterviewFinished(approvalTransactionModel, userId);
                    }

                    //if (approvalTransactionViewModel.ApprovalTransactionId != 0)
                    //{
                    //    if (approvalTransactionViewModel.ApprovalId == (int)Approval.Candidate)
                    //    {
                    //        UserViewModel userViewModel = new UserViewModel();
                    //        var user = _approvalRepository.GetUserForCandidateApproval(approvalTransactionViewModel.EntityId, approvalTransactionViewModel.NextEventOrderNumber);
                    //        userViewModel.MapFromModel(user);
                    //        approvalTransactionViewModel.User = userViewModel;
                    //        MailDetailModel mailDetail = new MailDetailModel();
                    //        mailDetail.EmailId = user.Email;
                    //        mailDetail.Subject = approvalTransactionViewModel.Action + " Candidate";
                    //        mailDetail.Template = TemplateType.Appoval;
                    //        mailDetail.MessageBody = approvalTransactionViewModel;
                    //        GenericHelper.Send(mailDetail, _configuration);
                    //    }
                    //    else
                    //    {
                    //        var users = _approvalRepository.GetUserForOpeningApproval(approvalTransactionViewModel);
                    //        users.ForEach(user =>
                    //        {
                    //            UserViewModel userViewModel = new UserViewModel();
                    //            MailDetailModel mailDetail = new MailDetailModel();
                    //            userViewModel.MapFromModel(user);
                    //            approvalTransactionViewModel.User = userViewModel;
                    //            mailDetail.EmailId = user.Email;
                    //            mailDetail.Subject = approvalTransactionViewModel.Action + " Opening";
                    //            mailDetail.Template = TemplateType.Appoval;
                    //            mailDetail.MessageBody = approvalTransactionViewModel;
                    //            GenericHelper.Send(mailDetail, _configuration);
                    //        });
                    //    }
                    //}
                    result.Body = approvalTransactionViewModel;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetApprovedUsersByRole(int roleId, int approvalEventId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                result.Body = _approvalRepository.GetApprovedUsersByRole(roleId, approvalEventId);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public ApprovalTransactionViewModel AddApprovalTransaction(EntityAndApprovalViewModel entityAndApprovalViewModel)
        {
            var approvalTransaction = new ApprovalTransactions();
            approvalTransaction.MapAuditColumns((ClaimsIdentity)_principal.Identity);

            var approvalTransactionViewModel = entityAndApprovalViewModel.approvalTransactionViewModel;
            approvalTransaction.ApprovalId = approvalTransactionViewModel.ApprovalId;
            approvalTransaction.ApprovalActionId = approvalTransactionViewModel.ApprovalActionId;
            if (entityAndApprovalViewModel.openingViewModel != null)
            {
                approvalTransaction.EntityId = entityAndApprovalViewModel.openingViewModel.OpeningId;
            }
            else
            {
                approvalTransaction.EntityId = entityAndApprovalViewModel.candidateViewModel.CandidateId;
            }
            approvalTransaction.EntityType = 0;
            if (approvalTransactionViewModel.NextEventOrderNumber < 0)
            {
                approvalTransaction.NextEventOrderNumber = approvalTransactionViewModel.NextEventOrderNumber;
            }
            else
            {
                approvalTransaction.EventOrderNumber = approvalTransactionViewModel.ApprovalEventOrder;
                approvalTransaction.NextEventOrderNumber = approvalTransaction.EventOrderNumber + 1;
            }

            var approvalTransactionDetail = new ApprovalTransactionDetails();
            approvalTransactionDetail.MapAuditColumns((ClaimsIdentity)_principal.Identity);
            approvalTransactionDetail.Comments = approvalTransactionViewModel.Comments;
            approvalTransactionDetail.ApprovalActionId = approvalTransaction.ApprovalActionId;
            approvalTransactionDetail.EventOrderNumber = approvalTransaction.EventOrderNumber;
            approvalTransaction.ApprovalTransactionDetails.Add(approvalTransactionDetail);
            _approvalRepository.CreateApprovalTransaction(approvalTransaction);
            approvalTransactionViewModel.MapFromModel(approvalTransaction);
            var userId = GenericHelper.GetUserClaimDetails((ClaimsIdentity)_principal.Identity).UserId;
            if (entityAndApprovalViewModel.candidateViewModel != null)
            {
                _candidateRepository.OnInterviewFinished(approvalTransaction, userId);
            }

            return approvalTransactionViewModel;
        }

        Guid GetUserId()
        {
            var identity = (ClaimsIdentity)_principal.Identity;
            var User = GenericHelper.GetUserClaimDetails(identity);
            return User.UserId;
        }

        public IResult GetDashboardDetails()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                DashboardViewModel dashboardViewModel = new DashboardViewModel();
                dashboardViewModel.TotalOpenOpenings = _approvalRepository.GetTotalOpenOpenings();
                dashboardViewModel.TotalCloseOpenings = _approvalRepository.GetTotalCloseOpenings();
                dashboardViewModel.TotalCandidateHired = _approvalRepository.GetTotalCandidatesHired();
                dashboardViewModel.TotalCandidateInterviewed = _approvalRepository.GetTotalCandidatesAttendedInterview(DateTime.Now.Month);
                result.Body = dashboardViewModel;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetChartDetails(int showType)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                ChartViewModel chartViewModel = new ChartViewModel();
                chartViewModel.Series = _approvalRepository.GetSeriesDetail(showType);
                result.Body = chartViewModel;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetUsersToScheduleInterview(int approvalEvent)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                result.Body = _approvalRepository.GetSeriesDetail(approvalEvent);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult ApprovalTransactionDetails(Guid entityId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                result.Body = _approvalRepository.ApprovalTransactionDetails(entityId);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetNextEventOrderForCandidate(Guid candidateId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                result.Body = _approvalRepository.GetNextEventOrderForCandidate(candidateId);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult CheckForStartInterview(CandidateListModel candidate)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var userId = GenericHelper.GetUserClaimDetails((ClaimsIdentity)_principal.Identity).UserId;
                result.Body = _approvalRepository.CheckForStartInterview(candidate.CandidateId, candidate.ApprovalEventId, userId);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetApprovalEventsOfUserForCandidate(Guid candidateId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var userId = GenericHelper.GetUserClaimDetails((ClaimsIdentity)_principal.Identity).UserId;
                result.Body = _approvalRepository.GetApprovalEventsOfUserForCandidate(candidateId, userId);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }
    }
}
