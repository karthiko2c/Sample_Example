using Microsoft.AspNetCore.Mvc;
using RS.Common.CommonData;
using RS.Service.Interfaces;
using System;
using Microsoft.AspNetCore.Authorization;
using RS.ViewModel.Approval;
using RS.ViewModel.Opening;
using RS.ViewModel.Candidate;

namespace RS.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Approval/[Action]")]
    [Authorize]
    public class ApprovalController : Controller
    {
        private readonly IApprovalManagerService _approvalManager;

        public ApprovalController(IApprovalManagerService approvalManager)
        {
            _approvalManager = approvalManager;

        }

        /// <summary>
        /// Get approval events by approval Id
        /// </summary>
        /// <param name="approvalId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResult GetApprovalEvents(int approvalId, Guid entityId)
        {
            return _approvalManager.GetApprovalEvents(approvalId, entityId);
        }

        [HttpGet]
        public IResult GetApprovals()
        {
            return _approvalManager.GetAllApprovals();
        }

        [HttpPost]
        public IResult GetApprovalDetails(Guid entityId)
        {
            var app = _approvalManager.ApprovalTransactionDetails(entityId);
            return app;
        }

        [HttpGet]
        public IResult GetAllApprovalEventRoles()
        {
            return _approvalManager.GetAllApprovalEventRoles();
        }

        [HttpGet]
        public IResult GetApprovalTransactionByEntity(Guid openingId)
        {
            return _approvalManager.GetApprovalTransactionByEntity(openingId);
        }

        [HttpGet]
        public IResult GetApprovedUsersByRole(int roleId, int approvalEventId)
        {
            return _approvalManager.GetApprovedUsersByRole(roleId, approvalEventId);
        }

        [ValidateModel]
        [HttpPost]
        public IResult CreateEventRole([FromBody]ApprovalEventRoleViewModel approvalEventRoleViewModel)
        {
            var createdEventRole = _approvalManager.ManageApprovalEventRole(approvalEventRoleViewModel);
            return createdEventRole;
        }

        [ValidateModel]
        [HttpPut]
        public IResult ManageApprovalTransaction([FromBody]EntityAndApprovalViewModel entityAndApprovalViewModel)
        {
            var createdEventRole = _approvalManager.ManageApprovalTransaction(entityAndApprovalViewModel);
            return createdEventRole;
        }

        [HttpGet]
        public IResult GetDashboardDetails()
        {
            var DashboardDetails = _approvalManager.GetDashboardDetails();
            return DashboardDetails;
        }

        [HttpGet]
        public IResult GetChartDetails(int showType)
        {
            var ChartDetails = _approvalManager.GetChartDetails(showType);
            return ChartDetails;
        }

        [HttpGet]
        public IResult GetNextEventOrderForCandidate(Guid candidateId)
        {
            var NextEventOrder = _approvalManager.GetNextEventOrderForCandidate(candidateId);
            return NextEventOrder;
        }

        [HttpGet]
        public IResult GetApprovalEventsOfUserForCandidate(Guid candidateId)
        {
            var NextEventOrder = _approvalManager.GetApprovalEventsOfUserForCandidate(candidateId);
            return NextEventOrder;
        }

        [HttpPost]
        public IResult CheckForStartInterview([FromBody]CandidateListModel candidate)
        {
            var NextEventOrder = _approvalManager.CheckForStartInterview(candidate);
            return NextEventOrder;
        }
    }
}