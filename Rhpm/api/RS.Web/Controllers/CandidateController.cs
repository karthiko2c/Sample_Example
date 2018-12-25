using Microsoft.AspNetCore.Mvc;
using RS.Common.CommonData;
using RS.Service.Interfaces;
using System;
using Microsoft.AspNetCore.Authorization;
using RS.ViewModel.Candidate;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace RS.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Candidate/[Action]")]
    [Authorize]
    public class CandidateController : Controller
    {
        private readonly ICandidateManagerService _candidateManagerService;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        
        public CandidateController(ICandidateManagerService candidateManager, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _candidateManagerService = candidateManager;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        [HttpPost]
        public IResult AddCandidate()
        {
            var candidateViewModel = JsonConvert.DeserializeObject<CandidateViewModel>(Request.Form["candidate"]);
            var file = Request.Form.Files["uploadFile"];
            var candidateDocumentViewModel = new CandidateDocumentViewModel();
            GetCandidateDocumentDetails(candidateDocumentViewModel, file);
            var addedCandidate = _candidateManagerService.AddCandidate(candidateViewModel, candidateDocumentViewModel);
            if (addedCandidate.Body != null)
            {
                var allowedExtensions = _configuration["ResumeExtension"].Split(',');
                FileHelper.SaveFile(file, candidateDocumentViewModel.UploadedDocument, allowedExtensions, _hostingEnvironment, "uploadFiles");
            }
            return addedCandidate;
        }

        [HttpPost]
        public IResult AddUserForCandidate([FromBody]List<CandidateAssignedUserModel> candidateAssignedUserList)
        {
            var addedCandidate = _candidateManagerService.AssignUserForCandidate(candidateAssignedUserList);
            return addedCandidate;
        }

        [HttpPut]
        public IResult UpdateCandidate()
        {
            var candidateViewModel = JsonConvert.DeserializeObject<CandidateViewModel>(Request.Form["candidate"]);
            var file = Request.Form.Files["uploadFile"];
            var candidateDocumentViewModel = candidateViewModel.CandidateDocument;
            if (file != null)
            {
                GetCandidateDocumentDetails(candidateDocumentViewModel, file);
            }
            var updatedCandidate = _candidateManagerService.UpdateCandidate(candidateViewModel, candidateDocumentViewModel);
            if (updatedCandidate.Body != null && file != null)
            {
                var allowedExtensions = _configuration["ResumeExtension"].Split(',');
                FileHelper.SaveFile(file, candidateDocumentViewModel.UploadedDocument, allowedExtensions, _hostingEnvironment, "uploadFiles");
            }
            return updatedCandidate;
        }

        [HttpGet]
        public IResult GetAllCandidate()
        {
            var allCandidates = _candidateManagerService.GetAllCandidate();
            return allCandidates;
        }

        [HttpGet]
        public IResult GetCandidateById(Guid id)
        {
            var candidateRecord = _candidateManagerService.GetCandidateById(id);
            return candidateRecord;
        }

        [HttpGet]
        public IResult GetAssignedUsersById(Guid candidateId)
        {
            var assignedUsers = _candidateManagerService.GetAssignedUsersById(candidateId);
            return assignedUsers;
        }

        [HttpGet]
        public IResult GetScheduledUsersById(Guid candidateId)
        {
            var assignedUsers = _candidateManagerService.GetScheduledUsersById(candidateId);
            return assignedUsers;
        }

        [HttpGet]
        public IResult GetCandidatesCorrespondingToLoggedUser(Guid userId)
        {
            var assignedUsers = _candidateManagerService.GetCandidatesCorrespondingToLoggedUser(userId);
            return assignedUsers;
        }

        [HttpPut]
        public IResult ApprovedForInterview(Guid candidateId)
        {
            var approvedCandidate = _candidateManagerService.ApprovedForInterview(candidateId);
            return approvedCandidate;
        }

        [HttpPost]
        public IResult AddUsersToConductInterview([FromBody]List<ScheduleUserForCandidateModel> scheduleUserForCandidateModelList)
        {
            var approvedCandidate = _candidateManagerService.AddUsersToConductInterview(scheduleUserForCandidateModelList);
            return approvedCandidate;
        }

        [HttpPost]
        public ActionResult DownloadFile(string file)
        {
            if (file == null)
            {
                return null;
            }
            var folder = _configuration["uploadFiles"];
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, folder + file);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, FileHelper.GetContentType(path), Path.GetFileName(path));

        }

        [HttpGet]
        public IResult GetOrganizationsOnInputChanged(string input)
        {
            var organizations = _candidateManagerService.GetOrganizationsOnInputChanged(input);
            return organizations;
        }

        private void GetCandidateDocumentDetails(CandidateDocumentViewModel candidateDocumentViewModel, IFormFile file)
        {
            candidateDocumentViewModel.DocumentName = file.FileName;
            var allowedExtensions = _configuration["ResumeExtension"].Split(',');           
            var extension = FileHelper.GetExtension(file, allowedExtensions);
            candidateDocumentViewModel.UploadedDocument = Guid.NewGuid().ToString() + extension;
        }

    }
}