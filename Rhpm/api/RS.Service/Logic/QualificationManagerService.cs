using RS.Service.Interfaces;
using System;
using System.Collections.Generic;
using RS.Common.CommonData;
using RS.ViewModel.Qualification;
using RS.Data.Interfaces;
using RS.Common.Enums;
using RS.Entity.Models;
using RS.Common.Extensions;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using RS.ViewModel.SearchAndSortModel;

namespace RS.Service.Logic
{
    public class QualificationManagerService : IQualificationManagerService
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IQualificationRepository _qualificationRepository;

        public QualificationManagerService(IPrincipal principal, IQualificationRepository qualificationRepository)
        {
            _qualificationRepository = qualificationRepository;
            _principal = principal as ClaimsPrincipal;
        }
        public IResult CreateQualification(QualificationViewModel qualification)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var duplicatequalification = _qualificationRepository.GetFirstOrDefault(x => x.Name == qualification.Name);
                if (duplicatequalification != null)
                {
                    result.Status = Status.Fail;
                    result.Message = QualificationStatusNotification.DuplicateQualification;
                    result.Body = null;
                }
                else
                {
                    var qualificationModel = new Qualifications();
                    qualificationModel.MapFromViewModel(qualification, (ClaimsIdentity)_principal.Identity);
                    _qualificationRepository.Create(qualificationModel);
                    _qualificationRepository.SaveChanges();
                    result.Body = qualificationModel;
                }   
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult DeleteQualification(QualificationViewModel qualification)
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success   
            };
            try
            {
                var qualificationObj = _qualificationRepository.GetFirstOrDefault(x => x.QualificationId == qualification.QualificationId);
                if (qualificationObj != null)
                {
                    qualificationObj.MapDeleteColumns((ClaimsIdentity)_principal.Identity);
                    _qualificationRepository.Update(qualificationObj);
                    _qualificationRepository.SaveChanges();
                    result.Message = QualificationStatusNotification.QualificationDeleted;
                }
                result.Body = qualificationObj;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetAllQualification()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var qualifications = new List<QualificationViewModel>();
                var allQualifications = _qualificationRepository.GetAll().ToList();
                result.Body = qualifications.MapFromModel<Qualifications, QualificationViewModel>(allQualifications);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetQualificationById(int id)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var qualification = new QualificationViewModel();
                var getQualification = _qualificationRepository.GetByID(id);
                result.Body = qualification.MapFromModel(getQualification);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult UpdateQualification(QualificationViewModel qualification)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                var duplicatequalification = _qualificationRepository.GetFirstOrDefault(x => x.Name == qualification.Name && x.QualificationId != qualification.QualificationId);
                if (duplicatequalification != null)
                {
                    result.Status = Status.Fail;
                    result.Message = QualificationStatusNotification.DuplicateQualification;
                    result.Body = null;
                }
                else
                {
                    var qualificationModel = new Qualifications();
                    qualificationModel.MapFromViewModel(qualification, (ClaimsIdentity)_principal.Identity);
                    _qualificationRepository.Update(qualificationModel);
                    _qualificationRepository.SaveChanges();
                    result.Body = qualificationModel;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Error;
            }
            return result;
        }

        public IResult GetQualificationsResults(SearchAndSortModel searchAndSortModel)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                List<QualificationViewModel> qualificationModelList = new List<QualificationViewModel>();
                var qualificationList = _qualificationRepository.GetAll(searchAndSortModel).ToList();

                var qualificationViewModelLists = qualificationModelList.MapFromModel<Qualifications, QualificationViewModel>(qualificationList);

                result.Body = qualificationViewModelLists;
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
