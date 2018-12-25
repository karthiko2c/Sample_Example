using RS.Service.Interfaces;
using RS.Common.Enums;
using System;
using RS.Data.Interfaces;
using RS.Common.CommonData;
using RS.ViewModel.Opening;
using RS.Entity.Models;
using System.Security.Claims;
using System.Security.Principal;
using RS.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using RS.ViewModel.Skill;
using RS.ViewModel.Approval;
using Microsoft.Extensions.Configuration;

namespace RS.Service.Logic
{
    public class OpeningManagerService : IOpeningManagerService
    {
        #region Global Variables
        private readonly IOpeningRepository _openingRepository;
        private readonly IApprovalRepository _approvalRepository;
        private readonly IApprovalManagerService _approvalManagerService;
        private readonly ClaimsPrincipal _principal;
        private readonly IConfiguration _configuration;
        #endregion
        public OpeningManagerService(IPrincipal principal, IApprovalManagerService approvalManagerService, IOpeningRepository openingRepository, IApprovalRepository approvalRepository,
            IConfiguration configuration)
        {
            _openingRepository = openingRepository;
            _approvalRepository = approvalRepository;
            _approvalManagerService = approvalManagerService;
            _principal = principal as ClaimsPrincipal;
            _configuration = configuration;
        }

        public IResult InsertOrUpdateOpening(EntityAndApprovalViewModel entityAndApprovalViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var openingViewModel = entityAndApprovalViewModel.openingViewModel;
                if (openingViewModel.OpeningId != Guid.Empty)
                {
                    result.Body = UpdateOpening(entityAndApprovalViewModel);
                }
                else
                {
                    var openingModel = new Openings();
                    openingModel.MapFromViewModel(openingViewModel, (ClaimsIdentity)_principal.Identity);
                    var openingSkillList = new List<OpeningSkills>();
                    var openingSkills = openingViewModel.PrimarySkillTypes.Union(openingViewModel.SecondarySkillTypes).ToList();

                    foreach (var item in openingSkills)
                    {
                        var openingSkill = new OpeningSkills
                        {
                            SkillId = item.SkillId,
                            SkillType = item.OpeningSkillType
                        };
                        openingSkill.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                        openingSkillList.Add(openingSkill);
                    }
                    _openingRepository.CreateOpening(openingModel, openingSkillList);

                    entityAndApprovalViewModel.openingViewModel.OpeningId = openingModel.OpeningId;
                    var approvalTransactionViewModel = _approvalManagerService.AddApprovalTransaction(entityAndApprovalViewModel);

                    // var users = _approvalRepository.GetUserForOpeningApproval(approvalTransactionViewModel);
                    //users.ForEach(user =>
                    //{
                    //    MailDetailModel mailDetail = new MailDetailModel();
                    //    mailDetail.EmailId = user.Email;
                    //    mailDetail.Subject = "Opening Approval";
                    //    mailDetail.Template = TemplateType.Appoval;
                    //    mailDetail.MessageBody = approvalTransactionViewModel;
                    //    GenericHelper.Send(mailDetail, _configuration);
                    //});
                    result.Body = approvalTransactionViewModel;
               
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        public IResult DeleteOpening(Guid id)
        {
            throw new NotImplementedException();
        }

        public IResult GetAllOpenings()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var allOpenings = _openingRepository.GetAll();
                var openingIds = allOpenings.Select(x => x.OpeningId).Distinct().ToList();
                var approvalTransactions = _approvalRepository.GetAllApprovalTransactions(openingIds).ToList();
                result.Body = allOpenings.Select(opening =>
                {
                    var approvalTransaction = approvalTransactions.FirstOrDefault(x => x.EntityId == opening.OpeningId);

                    var openingViewModel = new OpeningViewModel();
                    MapPrimaryandSecondarySkills(openingViewModel, opening);

                    if (approvalTransaction != null)
                    {
                        openingViewModel.IsApproved = approvalTransaction.IsApproved;
                        openingViewModel.Status = approvalTransaction.ApprovalAction.ApprovalActionName;
                    }
                    return openingViewModel.MapFromModel(opening);
                }).ToList();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        public IResult GetOpeningsCorrespondingToLoggedUser(Guid userId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var allOpenings = _openingRepository.GetOpeningsCorrespondingToLoggedUser(userId);
                var openingIds = allOpenings.Select(x => x.OpeningId).Distinct().ToList();
                var approvalTransactions = _approvalRepository.GetAllApprovalTransactions(openingIds).ToList();
                result.Body = allOpenings.Select(opening =>
                {
                    var approvalTransaction = approvalTransactions.FirstOrDefault(x => x.EntityId == opening.OpeningId);

                    var openingViewModel = new OpeningViewModel();
                    MapPrimaryandSecondarySkills(openingViewModel, opening);

                    if (approvalTransaction != null)
                    {
                        openingViewModel.IsApproved = approvalTransaction.IsApproved;
                        openingViewModel.Status = approvalTransaction.ApprovalAction.ApprovalActionName;
                    }
                    return openingViewModel.MapFromModel(opening);
                }).ToList();
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        public IResult GetOpeningById(Guid id)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var getOpening = _openingRepository.GetByID(id);
                var openingViewModel = new OpeningViewModel();
                MapPrimaryandSecondarySkills(openingViewModel, getOpening);
                result.Body = openingViewModel.MapFromModel(getOpening);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        public ApprovalTransactionViewModel UpdateOpening(EntityAndApprovalViewModel entityAndApprovalViewModel)
        {
            var openingViewModel = entityAndApprovalViewModel.openingViewModel;
            var openingModel = new Openings();
            openingModel.MapFromViewModel(openingViewModel, (ClaimsIdentity)_principal.Identity);
            var openingDetail = _openingRepository.GetByID(openingViewModel.OpeningId);
            var skillViewModelList = openingViewModel.PrimarySkillTypes.Union(openingViewModel.SecondarySkillTypes).ToList();
            var skillModelList = openingDetail.OpeningSkills.Where(x => (x.IsActive && !x.IsDeleted)).Select(x => x.Skill).ToList();

            var existingSkills = skillViewModelList.Select(x => x.SkillId).Intersect(skillModelList.Select(x => x.SkillId)).ToList();
            var addingSkills = skillViewModelList.Select(x => x.SkillId).Except(existingSkills).ToList();
            var removingSkills = skillModelList.Select(x => x.SkillId).Except(existingSkills).ToList();

            if (existingSkills.Any())
            {
                var openingSkills = openingDetail.OpeningSkills.Where(x => existingSkills.Contains(x.SkillId)).ToList();
                openingSkills.ForEach(x => x.MapAuditColumns((ClaimsIdentity)_principal.Identity));
            }

            if (removingSkills.Any())
            {
                var openingSkills = openingDetail.OpeningSkills.Where(x => removingSkills.Contains(x.SkillId)).ToList();
                openingSkills.ForEach(x => x.MapDeleteColumns((ClaimsIdentity)_principal.Identity));
            }

            var openingSkillList = new List<OpeningSkills>();
            if (addingSkills.Any())
            {
                var addingSkillList = skillViewModelList.Where(x => addingSkills.Contains(x.SkillId)).ToList();
                foreach (var item in addingSkillList)
                {
                    var openingSkill = new OpeningSkills()
                    {
                        OpeningId = openingDetail.OpeningId,
                        SkillId = item.SkillId,
                        SkillType = item.OpeningSkillType
                    };
                    openingSkill.MapAuditColumns((ClaimsIdentity)_principal.Identity);
                    openingSkillList.Add(openingSkill);
                }
            }

            if (openingSkillList.Any())
            {
                _openingRepository.UpdateOpeningSkills(openingSkillList);
            }
            _openingRepository.Update(openingModel);
            entityAndApprovalViewModel.approvalTransactionViewModel.NextEventOrderNumber += 1;
            _approvalManagerService.ManageApprovalTransaction(entityAndApprovalViewModel);
            _openingRepository.SaveChanges();
            return entityAndApprovalViewModel.approvalTransactionViewModel;
        }

        public void MapPrimaryandSecondarySkills(OpeningViewModel openingViewModel, Openings opening)
        {
            var primarySkillList = opening.OpeningSkills.Where(x => x.SkillType == OpeningSkillType.Primary && (x.IsActive && !x.IsDeleted)).Select(x => x.Skill).ToList();
            var secondarySkillList = opening.OpeningSkills.Where(x => x.SkillType == OpeningSkillType.Secondary && (x.IsActive && !x.IsDeleted)).Select(x => x.Skill).ToList();
            openingViewModel.PrimarySkillTypes = new List<SkillViewModel>();
            openingViewModel.SecondarySkillTypes = new List<SkillViewModel>();
            openingViewModel.PrimarySkills = string.Join(",", primarySkillList.Select(x => x.Name).ToList());
            openingViewModel.SecondarySkills = string.Join(",", secondarySkillList.Select(x => x.Name).ToList());
            if (primarySkillList.Any())
            {
                foreach (var skill in primarySkillList)
                {
                    var skillViewModel = new SkillViewModel();
                    skillViewModel.MapFromModel(skill);
                    skillViewModel.OpeningSkillType = OpeningSkillType.Primary;
                    openingViewModel.PrimarySkillTypes.Add(skillViewModel);
                }
            }
            if (secondarySkillList.Any())
            {
                foreach (var skill in secondarySkillList)
                {
                    var skillViewModel = new SkillViewModel();
                    skillViewModel.MapFromModel(skill);
                    skillViewModel.OpeningSkillType = OpeningSkillType.Secondary;
                    openingViewModel.SecondarySkillTypes.Add(skillViewModel);
                }
            }
        }
    }
}
