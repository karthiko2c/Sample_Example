using RS.Service.Interfaces;
using RS.Common.Enums;
using RS.ViewModel.User;
using System;
using RS.Data.Interfaces;
using RS.Common.CommonData;
using RS.ViewModel.Candidate;
using RS.Entity.Models;
using System.Security.Claims;
using System.Security.Principal;
using RS.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using RS.ViewModel.Organization;
using RS.ViewModel.Approval;
using Microsoft.AspNetCore.Hosting;

namespace RS.Service.Logic
{
    public class CandidateManagerService : ICandidateManagerService
    {
        #region Global Variables
        private readonly ClaimsPrincipal _principal;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IApprovalRepository _approvalRepository;
        private readonly IOpeningRepository _openingRepository;
        private readonly IQualificationRepository _qualificationRepository;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        #endregion
        public CandidateManagerService(IPrincipal principal, ICandidateRepository candidateRepository,
            IOpeningRepository openingRepository, IQualificationRepository qualificationRepository, IApprovalRepository approvalRepository,
            IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _principal = principal as ClaimsPrincipal;
            _candidateRepository = candidateRepository;
            _openingRepository = openingRepository;
            _qualificationRepository = qualificationRepository;
            _approvalRepository = approvalRepository;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public IResult AddCandidate(CandidateViewModel candidateViewModel, CandidateDocumentViewModel candidateDocumentViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var candidateModel = new Candidates();
                candidateModel.MapFromViewModel(candidateViewModel, (ClaimsIdentity)_principal.Identity);
                candidateModel.QualificationId = candidateViewModel.Qualification;

                #region Insert OpeningCandidate
                var openingCandidate = new OpeningCandidates();
                openingCandidate.OpeningId = candidateViewModel.OpeningId;
                openingCandidate.Candidate = candidateModel;
                openingCandidate.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                #endregion

                #region Insert Organization
                Organizations organization = null;
                if (candidateViewModel.OrganizationId != 0)
                {
                    candidateModel.OrganizationId = candidateViewModel.OrganizationId;
                }
                else
                {
                    organization = new Organizations
                    {
                        Name = candidateViewModel.OrganizationName
                    };
                    organization.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                    candidateModel.Organisation = organization;
                }
                #endregion

                #region Insert Candidate Document 
                var candidateDocumentModel = new CandidateDocuments();
                candidateDocumentModel.MapFromViewModel(candidateDocumentViewModel, (ClaimsIdentity)_principal.Identity);
                candidateModel.CandidateDocuments.Add(candidateDocumentModel);
                #endregion

                _candidateRepository.AddCandidate(candidateModel, openingCandidate);
                result.Body = candidateModel.CandidateId;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult AssignUserForCandidate(List<CandidateAssignedUserModel> candidateAssignedUserList)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var assignedUserList = _candidateRepository.GetAssignedUsersByID(candidateAssignedUserList[0].CandidateId);
                var approvalEvents = _approvalRepository.GetApprovalEvents((int)Approval.Candidate);
                var addingAssignedUsers = new List<CandidateAssignedUserModel>();
                if (assignedUserList.Any())
                {
                    foreach (var approvalEvent in approvalEvents)
                    {
                        var candidateAssignedUserModelsList = candidateAssignedUserList.Where(x => x.ApprovalEventId == approvalEvent.ApprovalEventId).Select(y => y.UserId).ToList();
                        var candidateAssignedUserModels = assignedUserList.Where(x => x.ApprovalEventId == approvalEvent.ApprovalEventId).Select(y => y.UserId).ToList();

                        if (candidateAssignedUserModelsList.Any() && candidateAssignedUserModels.Any())
                        {
                            var existingAssignedUsers = candidateAssignedUserModelsList.Intersect(candidateAssignedUserModels).ToList();
                            var addAssignedUsers = candidateAssignedUserModelsList.Except(existingAssignedUsers).ToList();
                            var removingAssignedusers = candidateAssignedUserModels.Except(existingAssignedUsers).ToList();

                            if (existingAssignedUsers.Any())
                            {
                                var assignedUsers = assignedUserList.Where(x => existingAssignedUsers.Contains(x.UserId) && x.ApprovalEventId == approvalEvent.ApprovalEventId).ToList();
                                assignedUsers.ForEach(x => x.MapAuditColumns((ClaimsIdentity)_principal.Identity));
                            }

                            if (removingAssignedusers.Any())
                            {
                                var assignedUsers = assignedUserList.Where(x => removingAssignedusers.Contains(x.UserId) && x.ApprovalEventId == approvalEvent.ApprovalEventId).ToList();
                                assignedUsers.ForEach(x => x.MapDeleteColumns((ClaimsIdentity)_principal.Identity));
                            }

                            if (addAssignedUsers.Any())
                            {
                                addingAssignedUsers = candidateAssignedUserList.Where(x => addAssignedUsers.Contains(x.UserId) && x.ApprovalEventId == approvalEvent.ApprovalEventId).ToList();
                            }
                        }
                    }
                }
                else
                {
                    addingAssignedUsers = candidateAssignedUserList;
                }

                if (addingAssignedUsers.Any())
                {
                    foreach (var assignedUser in addingAssignedUsers)
                    {
                        var assignedUserForCandidate = new CandidateAssignedUser();
                        assignedUserForCandidate.MapFromViewModel(assignedUser, (ClaimsIdentity)_principal.Identity);
                        _candidateRepository.AssignUserForCandidate(assignedUserForCandidate);

                        MailDetailModel mailDetail = new MailDetailModel();
                        mailDetail.EmailId = assignedUser.User.Email;
                        mailDetail.Subject = "Registration Confirmation";
                        mailDetail.Template = TemplateType.ScheduleUserForInterview;
                        mailDetail.MessageBody = assignedUser;
                        GenericHelper.Send(mailDetail, _configuration, _hostingEnvironment);
                    }
                }

                result.Body = candidateAssignedUserList;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult ApprovedForInterview(Guid candidateId)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                var candidateModel = _candidateRepository.GetByID(candidateId);
                if (candidateModel != null)
                {
                    candidateModel.IsReadyForInterview = true;
                    _candidateRepository.ApprovedForInterview(candidateModel);
                }
                var candidateViewModel = new CandidateViewModel();
                candidateViewModel.MapFromModel(candidateModel);
                //if (candidateViewModel.IsReadyForInterview)
                //{
                //    var user = _approvalRepository.GetUserForCandidateApproval(candidateViewModel.CandidateId, Constants.FirstApproval);
                //    MailDetailModel mailDetail = new MailDetailModel();
                //    mailDetail.EmailId = user.Email;
                //    mailDetail.Subject = "Ready For Interview";
                //    mailDetail.Template = TemplateType.Appoval;
                //    mailDetail.MessageBody = user;
                //    GenericHelper.Send(mailDetail, _configuration);
                //}
                result.Body = candidateViewModel.CandidateId;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult DeleteCandidate(Guid id)
        {
            throw new NotImplementedException();
        }

        public IResult GetAllCandidate()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var allCandidates = _candidateRepository.GetAll();
                var candidateList = allCandidates.Select(candidate =>
                {
                    var openingCandidate = _candidateRepository.GetOpeningCandidate(candidate.CandidateId);
                    var candidateListModel = new CandidateListModel();
                    if (openingCandidate != null)
                    {
                        candidateListModel.Opening = openingCandidate.Opening.Title;
                        candidateListModel.ModifiedDate = openingCandidate.Opening.ModifiedDate;
                    }

                    var assignedUsers = _candidateRepository.GetAssignedUsersByID(candidate.CandidateId);
                    candidateListModel.AssignedUsers = assignedUsers.Count;
                    candidateListModel.Documents = candidate.CandidateDocuments.Count;
                    var approvalTransaction = _approvalRepository.GetApprovalTransactionByEntity(candidate.CandidateId);
                    candidateListModel.Status = approvalTransaction == null ? "Created" : approvalTransaction.ApprovalAction.ApprovalActionName;

                    return candidateListModel.MapFromModel(candidate);

                }).ToList();
                List<CandidateListModel> candidateListViewModel = candidateList.Cast<CandidateListModel>().ToList();
                result.Body = candidateListViewModel.OrderByDescending(x => x.ModifiedDate);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetAssignedUsersById(Guid candidateId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var assignedUsersList = new List<CandidateAssignedUser>();
                var assignedUsers = _candidateRepository.GetAssignedUsersByID(candidateId);
                result.Body = assignedUsersList.MapFromModel<CandidateAssignedUser, CandidateAssignedUserModel>(assignedUsers);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetCandidateById(Guid id)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var candidateViewModel = new CandidateViewModel();
                var getCandidate = _candidateRepository.GetByID(id);
                var openingCandidate = _candidateRepository.GetOpeningCandidate(getCandidate.CandidateId);
                candidateViewModel.OpeningId = openingCandidate.Opening.OpeningId;
                candidateViewModel.OpeningTitle = openingCandidate.Opening.Title;
                candidateViewModel.Qualification = getCandidate.Qualification.QualificationId;
                candidateViewModel.QualificationName = getCandidate.Qualification.Name;
                candidateViewModel.OrganizationName = getCandidate.Organisation.Name;
                var candidateDocument = new CandidateDocumentViewModel();
                candidateViewModel.CandidateDocument = (CandidateDocumentViewModel)candidateDocument.MapFromModel(getCandidate.CandidateDocuments.FirstOrDefault());
                result.Body = candidateViewModel.MapFromModel(getCandidate);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetCandidatesCorrespondingToLoggedUser(Guid userId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var allScheduleUserForCandiadate = _candidateRepository.GetCandidatesCorrespondingToLoggedUser(userId);
                var candidateList = allScheduleUserForCandiadate.Select(scheduleUser =>
                {
                    var openingCandidate = _candidateRepository.GetOpeningCandidate(scheduleUser.CandidateId);
                    var candidateListModel = new CandidateListModel();
                    if (openingCandidate != null)
                    {
                        candidateListModel.Opening = openingCandidate.Opening.Title;
                        candidateListModel.ModifiedDate = openingCandidate.Opening.ModifiedDate;
                    }

                    var approvalTransaction = _approvalRepository.GetApprovalTransactionByEntity(scheduleUser.CandidateId);

                    candidateListModel.Status = approvalTransaction == null ? "Created" : approvalTransaction.ApprovalAction.ApprovalActionName;

                    candidateListModel.ApprovalEventId = scheduleUser.ApprovalEventId;
                    candidateListModel.IsFinished = _candidateRepository.CheckForInterviewCompletion(scheduleUser);
                    return candidateListModel.MapFromModel(scheduleUser.Candidate);
                }).ToList();
                List<CandidateListModel> candidateListViewModel = candidateList.Cast<CandidateListModel>().ToList();
                result.Body = candidateListViewModel.OrderByDescending(x => x.ModifiedDate);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult UpdateCandidate(CandidateViewModel candidateViewModel, CandidateDocumentViewModel candidateDocumentViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                var candidateModel = _candidateRepository.GetByID(candidateViewModel.CandidateId);
                candidateModel.MapFromViewModel(candidateViewModel, (ClaimsIdentity)_principal.Identity);
                candidateModel.QualificationId = candidateViewModel.Qualification;

                #region Organization insert update
                Organizations organization = null;
                if (candidateModel.OrganizationId != candidateViewModel.OrganizationId)
                {
                    if (candidateViewModel.OrganizationId != 0)
                    {
                        candidateModel.OrganizationId = candidateViewModel.OrganizationId;
                    }
                    else
                    {
                        organization = new Organizations
                        {
                            Name = candidateViewModel.OrganizationName
                        };
                        organization.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                        candidateModel.Organisation = organization;
                    }
                }
                #endregion

                #region OpeningCandidate Update
                var openingCandidateModel = _candidateRepository.GetOpeningCandidate(candidateViewModel.CandidateId);
                if ((openingCandidateModel != null) && (openingCandidateModel.Opening.OpeningId != candidateViewModel.OpeningId))
                {
                    openingCandidateModel.OpeningId = candidateViewModel.OpeningId;
                    openingCandidateModel.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                }
                #endregion

                #region Candidate Document Insert Update
                var candidateDocumentModel = candidateModel.CandidateDocuments.FirstOrDefault();
                if (candidateDocumentViewModel != null)
                {
                    candidateDocumentModel.MapFromViewModel(candidateDocumentViewModel, (ClaimsIdentity)_principal.Identity);
                }
                candidateModel.CandidateDocuments.Add(candidateDocumentModel);
                #endregion

                _candidateRepository.UpdateCandidate(candidateModel, openingCandidateModel);
                result.Body = candidateModel.CandidateId;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetOrganizationsOnInputChanged(string input)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                List<OrganizationViewModel> organizationModels = new List<OrganizationViewModel>();
                var organizations = _candidateRepository.GetOrganizationsOnInputChanged(input);
                result.Body = organizationModels.MapFromModel<Organizations, OrganizationViewModel>(organizations);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult AddUsersToConductInterview(List<ScheduleUserForCandidateModel> scheduleUserForCandidateModelList)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                ScheduleUserForCandidate scheduleUser = new ScheduleUserForCandidate();
                scheduleUser.MapFromViewModel(scheduleUserForCandidateModelList.First());
                var scheduledUsersModelList = scheduleUserForCandidateModelList.Select(x => x.UserId).ToList();
                var scheduledUsers = _candidateRepository.GetScheduledUserByApprovalEvent(scheduleUser);
                var scheduledUsersList = scheduledUsers.Select(x => x.UserId).ToList();
                var addingScheduledUsers = new List<ScheduleUserForCandidateModel>();
                if (scheduledUsersList.Any())
                {
                    var existingScheduledUsers = scheduledUsersModelList.Intersect(scheduledUsersList).ToList();
                    var addScheduledUsers = scheduledUsersModelList.Except(existingScheduledUsers).ToList();
                    var removingScheduledusers = scheduledUsersList.Except(existingScheduledUsers).ToList();

                    if (existingScheduledUsers.Any())
                    {
                        var scheduledUserList = scheduledUsers.Where(x => existingScheduledUsers.Contains(x.UserId)).ToList();
                        scheduledUserList.ForEach(x => x.MapAuditColumns((ClaimsIdentity)_principal.Identity));
                    }

                    if (removingScheduledusers.Any())
                    {
                        var scheduledUsersModeList = new List<ScheduleUserForCandidateModel>();
                        var scheduledUserList = scheduledUsers.Where(x => removingScheduledusers.Contains(x.UserId)).ToList();
                        scheduledUserList.ForEach(x => x.MapDeleteColumns((ClaimsIdentity)_principal.Identity));

                        foreach (var scheduledUser in scheduledUserList)
                        {
                            var scheduleUserModel = new ScheduleUserForCandidateModel();
                            scheduleUserModel.MapFromModel(scheduledUser);

                            var approvalEvent = new ApprovalEventViewModel();
                            scheduleUserModel.ApprovalEvent = (ApprovalEventViewModel)approvalEvent.MapFromModel(scheduledUser.ApprovalEvent);

                            var user = new UserViewModel();
                            scheduleUserModel.User = (UserViewModel)user.MapFromModel(scheduledUser.User);

                            CandidateViewModel candidateView = new CandidateViewModel();
                            candidateView.MapFromModel(_candidateRepository.GetByID(scheduledUser.CandidateId));

                            scheduleUserModel.Candidate = candidateView;
                            scheduleUserModel.InterviewScheduledDate = scheduledUser.ScheduledOn.ToString("yyyy-MM-dd");
                            scheduleUserModel.InterviewScheduledTime = scheduledUser.ScheduledOn.ToString("hh:mm a");
                            MailDetailModel mailDetail = new MailDetailModel();
                            mailDetail.EmailId = scheduleUserModel.User.Email;
                            mailDetail.Subject = "Interview Cancelled";
                            mailDetail.Template = TemplateType.InterviewCancelled;
                            mailDetail.MessageBody = scheduleUserModel;
                            GenericHelper.Send(mailDetail, _configuration, _hostingEnvironment);
                        }

                    }

                    if (addScheduledUsers.Any())
                    {
                        addingScheduledUsers = scheduleUserForCandidateModelList.Where(x => addScheduledUsers.Contains(x.UserId)).ToList();
                    }
                }
                else
                {
                    addingScheduledUsers = scheduleUserForCandidateModelList;
                }
                if (addingScheduledUsers.Any())
                {
                    foreach (var scheduledUser in addingScheduledUsers)
                    {
                        var scheduledUserForCandidate = new ScheduleUserForCandidate();
                        scheduledUserForCandidate.MapFromViewModel(scheduledUser, (ClaimsIdentity)_principal.Identity);
                        _candidateRepository.AddScheduledUsers(scheduledUserForCandidate);
                        CandidateViewModel candidateView = new CandidateViewModel();
                        candidateView.MapFromModel(_candidateRepository.GetByID(scheduledUser.CandidateId));
                        scheduledUser.Candidate = candidateView;
                        scheduledUser.InterviewScheduledDate = scheduledUser.ScheduledOn.ToString("yyyy-MM-dd");
                        scheduledUser.InterviewScheduledTime = scheduledUser.ScheduledOn.ToString("hh:mm tt");
                        MailDetailModel mailDetail = new MailDetailModel();
                        mailDetail.EmailId = scheduledUser.User.Email;
                        mailDetail.Subject = "Interview Scheduled";
                        mailDetail.Template = TemplateType.ScheduleUserForInterview;
                        mailDetail.MessageBody = scheduledUser;
                        GenericHelper.Send(mailDetail, _configuration, _hostingEnvironment);
                    }
                }
                _candidateRepository.SaveChanges();
                result.Body = scheduleUserForCandidateModelList;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetScheduledUsersById(Guid candidateId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                ScheduleInterviewModel scheduleInterviewModel = new ScheduleInterviewModel();
                var usersList = new List<UserViewModel>();
                var scheduledUsersList = new List<ScheduleUserForCandidateModel>();
                var scheduledUsers = _candidateRepository.GetScheduledUsersById(candidateId);
                var scheduledInterviewsCompleted = scheduledUsers.Where(x => x.CandidateId == candidateId && x.IsFinished && (x.IsActive && !x.IsDeleted)).ToList();
                var approvalEventOrderList = scheduledInterviewsCompleted.Select(x => x.ApprovalEvent.ApprovalEventOrder).ToList();
                var usersCompletedInterview = scheduledInterviewsCompleted.Select(x => x.User).ToList();
                scheduleInterviewModel.InterviewScheduledTime = scheduledInterviewsCompleted.Select(x => x.ScheduledOn).ToList();
                scheduleInterviewModel.Users = usersList.MapFromModel<Users, UserViewModel>(usersCompletedInterview);
                scheduleInterviewModel.FinishedEventOrder = approvalEventOrderList.DefaultIfEmpty(0).Max();
                scheduleInterviewModel.ScheduledDate = GetScheduledDate(scheduledUsers, candidateId);
                scheduleInterviewModel.NextApprovalEvent = scheduledInterviewsCompleted.Any() ? scheduledInterviewsCompleted[0].ApprovalEventId + 1 : 4;
                scheduleInterviewModel.ScheduleUserForCandidateModelList = scheduledUsersList.MapFromModel<ScheduleUserForCandidate, ScheduleUserForCandidateModel>(scheduledUsers);
                result.Body = scheduleInterviewModel;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        DateTime GetScheduledDate(List<ScheduleUserForCandidate> scheduledUsers, Guid candidateId)
        {
            var date = DateTime.Now;
            var approvalEventIds = scheduledUsers.Where(x => x.CandidateId == candidateId).Select(x => x.ApprovalEventId).Distinct().ToList();
            foreach (var approvalEventId in approvalEventIds)
            {
                var isFinished = scheduledUsers.Where(x => x.ApprovalEventId == approvalEventId).Select(x => x.IsFinished).ToList();
                if (!isFinished.Contains(true))
                {
                    date = scheduledUsers.FirstOrDefault(x => x.ApprovalEventId == approvalEventId).ScheduledOn;
                }
            }
            return date;
        }

    }
}
