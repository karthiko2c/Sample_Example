using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RS.Service.Interfaces;
using RS.Common.CommonData;
using RS.ViewModel.Qualification;
using RS.Common.Enums;
using RS.ViewModel.SearchAndSortModel;

namespace RS.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Qualificaton/[Action]")]
    [ValidateModel]
    [Authorize]
    public class QualificatonController : Controller
    {
        private readonly IQualificationManagerService _qualificationService;
        public QualificatonController(IQualificationManagerService qualificationService)
        {
            this._qualificationService = qualificationService;
        }

        [HttpPost]
        public IResult CreateQualification([FromBody]QualificationViewModel qualificationView)
        {
            if (ModelState.IsValid)
            {
                var createdQualification = _qualificationService.CreateQualification(qualificationView);
                return createdQualification;
            }
            return new Result
            {
                Operation = Operation.Create,
                Status = Status.Fail,
                Message = CommonErrorMessages.BadRequest,
                Body = null
            };
        }

        [HttpPut]
        public IResult UpdateQualification([FromBody]QualificationViewModel qualificationView)
        {
            if (ModelState.IsValid)
            {
                var updatedQualification = _qualificationService.UpdateQualification(qualificationView);
                return updatedQualification;
            }
            return new Result
            {
                Operation = Operation.Update,
                Status = Status.Fail,
                Message = CommonErrorMessages.BadRequest,
                Body = null
            };
        }
        [HttpPut]
        public IResult DeleteQualification([FromBody]QualificationViewModel qualificationView)
        {
            if (ModelState.IsValid)
            {
                var deleteQualification = _qualificationService.DeleteQualification(qualificationView);
                return deleteQualification;
            }
            return new Result
            {
                Operation = Operation.Delete,
                Status = Status.Fail,
                Message = CommonErrorMessages.BadRequest,
                Body = null
            };
        }

        [HttpGet]
        public IResult GetAllQualification()
        {
            var qualificationList = _qualificationService.GetAllQualification();
            return qualificationList;
        }

        [HttpGet]
        public IResult GetQualificationlById(int id)
        {
            var QualificationRecord = _qualificationService.GetQualificationById(id);
            return QualificationRecord;
        }

        [HttpPost]
        public IResult GetQualificationsResults([FromBody]SearchAndSortModel searchAndSortModel)
        {
            var quaificationList = _qualificationService.GetQualificationsResults(searchAndSortModel);
            return quaificationList;
        }
    }
}