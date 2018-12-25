using Microsoft.AspNetCore.Mvc;
using SubQuip.Common.CommonData;
using Microsoft.AspNetCore.Authorization;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.Equipment;
using SubQuip.ViewModel.PartProperties;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Newtonsoft.Json;
using SubQuip.Common.Extensions;
using System;
using System.IO;
using System.Linq;
using SubQuip.ViewModel.TechSpecs;
using SubQuip.Common;
using SubQuip.ViewModel.User;
using System.Threading.Tasks;
using SubQuip.ViewModel.Request;
using SubQuip.Common.Enums;
using System.Net;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Equipment controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Equipment/[Action]")]
    [ValidateModel]
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEquipmentManagerService _equipmentManager;

        /// <summary>
        /// Initializes a new instance of the EquipmentController
        /// </summary>
        /// <param name="equipmentManager">Equipment manager.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="hostingEnvironment">Hosting environment.</param>
        public EquipmentController(IEquipmentManagerService equipmentManager, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _equipmentManager = equipmentManager;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Search for equipments.
        /// </summary>
        /// <returns>The equipments found by the search.</returns>
        /// <param name="search">Search parameters to find equipments.</param>
        [HttpGet]
        public IResult Equipments(SearchSortModel search)
        {
            var equipmentList = _equipmentManager.GetAllEquipments(search);
            return equipmentList;
        }

        /// <summary>
        /// Get a specific equipment by its identifier.
        /// </summary>
        /// <returns>The spesific equipment.</returns>
        /// <param name="id">Identifier of the equipment</param>
        [HttpGet]
        public IResult Details(string id)
        {
            var equipmentRecord = _equipmentManager.GetEquipmentById(id);
            return equipmentRecord;
        }

        /// <summary>
        /// Create a new equipment.
        /// </summary>
        /// <returns>The created equipment.</returns>
        /// <param name="equipmentViewModel">Equipment view model.</param>
        [HttpPost]
        public IResult Create([FromBody]EquipmentViewModel equipmentViewModel)
        {
            var result = _equipmentManager.InsertEquipment(equipmentViewModel);
            return result;
        }

        /// <summary>
        /// Uploads the equipment document.
        /// </summary>
        /// <returns>The equipment document.</returns>
        [HttpPost]
        public IResult InsertEquipmentDocument()
        {
            IResult result = null;
            var documentViewModel = JsonConvert.DeserializeObject<DocumentViewModel>(Request.Form["model"]);
            var fileList = new List<FileDetails>();
            if (documentViewModel != null)
            {
                var files = Request.Form.Files;
                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        var fileModel = FileHelper.GetFileDetails(file);
                        fileModel.Title = documentViewModel.Title;
                        fileModel.Description = documentViewModel.Description;
                        fileList.Add(fileModel);
                    }
                }
                result = _equipmentManager.SaveEquipmentDocument(fileList, documentViewModel);
            }
            return result;
        }

        /// <summary>
        /// Updated a spesific equipment.
        /// </summary>
        /// <returns>The updated equipment.</returns>
        /// <param name="equipmentViewModel">Equipment view model.</param>
        [HttpPut]
        public IResult Update([FromBody]EquipmentViewModel equipmentViewModel)
        {
            var result = _equipmentManager.UpdateEquipment(equipmentViewModel);
            return result;
        }

        /// <summary>
        /// Manages the equipment part properties.
        /// </summary>
        /// <returns>The equipment part properties.</returns>
        /// <param name="partPropertyViewModel">Part property view model.</param>
        [HttpPut]
        public IResult ManageEquipmentPartProperties([FromBody] PartPropertyViewModel partPropertyViewModel)
        {
            var result = _equipmentManager.ManageEquipmentPartProperties(partPropertyViewModel);
            return result;
        }

        /// <summary>
        /// Manages the equipment tech specs.
        /// </summary>
        /// <returns>The equipment tech specs.</returns>
        /// <param name="techSpecsViewModel">Tech specs view model.</param>
        [HttpPut]
        public IResult ManageEquipmentTechSpecs([FromBody] TechSpecsViewModel techSpecsViewModel)
        {
            var result = _equipmentManager.ManageEquipmentTechnicalSpecs(techSpecsViewModel);
            return result;
        }

        /// <summary>
        /// Delete the equipment.
        /// </summary>
        /// <returns>The deleted equipment.</returns>
        /// <param name="id">Identifier of the equipment.</param>
        [HttpDelete]
        public IResult Delete(string id)
        {
            return _equipmentManager.DeleteEquipment(id);
        }

        /// <summary>
        /// Delete All Equipments.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpDelete]
        public IResult DeleteAll([FromBody]UserLoginViewModel loginModel)
        {
            if (loginModel.UserName.Equals("test") && loginModel.UserPassword.Equals("test"))
            {
                return _equipmentManager.DeleteAllEquipment();
            }
            return null;
        }

        /// <summary>
        /// Insert Equipment Request
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResult> RequestEquipment()
        {
            IResult result = null;
            var requestViewModel = JsonConvert.DeserializeObject<EquipmentRequestViewModel>(Request.Form["model"]);
            if (requestViewModel != null)
            {
                var fileList = new List<FileDetails>();
                var files = Request.Form.Files;
                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        var fileModel = FileHelper.GetFileDetails(file);
                        fileList.Add(fileModel);
                    }
                }

                result = _equipmentManager.InsertEquipmentRequest(fileList, requestViewModel);
                if (result.Status == Status.Success)
                {
                    var emailOptions = PrepareEmailOptions(requestViewModel);
                    if (!string.IsNullOrEmpty(emailOptions.HtmlBody))
                    {
                        #region Append attach files
                        foreach (var file in files)
                        {
                            byte[] content = null;
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                content = ms.ToArray();
                            }
                            emailOptions.Attachments.Add(new Attachment
                            {
                                Name = file.FileName,
                                ContentType = file.ContentType,
                                Content = content
                            });
                        }
                        #endregion

                        var response = await SendGridMailHelper.SendSingleEmailToMultipleRecipients(_configuration, emailOptions);
                        if (response.StatusCode == HttpStatusCode.Accepted)
                        {
                            result.Message += "; Mail sent.";
                        }
                        else
                        {
                            result.Message += "; Mail not sent to provided email.";
                        }
                    }
                    else
                    {
                        result.Message += "; Mail not sent, as email template not found";
                    }
                }
            }
            return result;
        }

        #region Private methods

        /// <summary>
        /// Prepare Email options for equipment requests
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        private EmailOptions PrepareEmailOptions(EquipmentRequestViewModel requestViewModel)
        {
            var emailOptions = new EmailOptions
            {
                Template = MailTemplate.Equipment,
                PlainBody = string.Empty,
                Attachments = new List<Attachment>()
            };
            var msgBody = SendGridMailHelper.MailBody(_hostingEnvironment, emailOptions.Template);
            if (!string.IsNullOrEmpty(msgBody))
            {
                emailOptions.Subject = "Request for Equipment Received - SubQuip";
                emailOptions.CcMail = new MailUser { Name = "SubQuip", Email = _configuration["requestEmail"] };
                emailOptions.ToMailsList = requestViewModel.MailUsers;
                emailOptions.HtmlBody = msgBody.Replace("{EquipmentNumber}", requestViewModel.EquipmentNumber).Replace("{FromDate}", requestViewModel.FromDate.ToString("dd-MM-yyyy")).Replace("{ToDate}", requestViewModel.ToDate.ToString("dd-MM-yyyy"));
            }
            return emailOptions;
        }

    }

    #endregion
}