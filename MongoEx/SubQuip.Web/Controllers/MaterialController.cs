using Microsoft.AspNetCore.Mvc;
using SubQuip.Common.CommonData;
using Microsoft.AspNetCore.Authorization;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.PartProperties;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Linq;
using SubQuip.Common.Extensions;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SubQuip.ViewModel.Material;
using SubQuip.ViewModel.TechSpecs;
using SubQuip.Common;
using SubQuip.Common.Enums;
using SubQuip.ViewModel.Request;
using SubQuip.ViewModel.User;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Material controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Material/[Action]")]
    [ValidateModel]
    [Authorize]
    public class MaterialController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMaterialManagerService _materialManager;

        /// <summary>
        /// Initializes a new instance of Material controller
        /// </summary>
        /// <param name="materialManager">Material manager.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="hostingEnvironment">Hosting environment.</param>
        public MaterialController(IMaterialManagerService materialManager, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _materialManager = materialManager;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Search for materials.
        /// </summary>
        /// <returns>List of found materials.</returns>
        /// <param name="search">Search parameters.</param>
        [HttpGet]
        public IResult Materials(SearchSortModel search)
        {
            var materialList = _materialManager.GetAllMaterials(search);
            return materialList;
        }

        /// <summary>
        /// Get a spesific material.
        /// </summary>
        /// <returns>Record of that material.</returns>
        /// <param name="id">Identifier of the material.</param>
        [HttpGet]
        public IResult Details(string id)
        {
            var materialRecord = _materialManager.GetMaterialById(id);
            return materialRecord;
        }

        /// <summary>
        /// Get Equipments For Material
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IResult EquipmentsForMaterial(string id)
        {
            var materialRecord = _materialManager.GetEquipmentsForMaterial(id);
            return materialRecord;
        }

        /// <summary>
        /// Create a new material.
        /// </summary>
        /// <returns>The created material.</returns>
        /// <param name="materialViewModel">Material view model.</param>
        [HttpPost]
        public IResult Create([FromBody]MaterialViewModel materialViewModel)
        {
            var result = _materialManager.InsertMaterial(materialViewModel);
            return result;
        }

        /// <summary>
        /// Upload the material document.
        /// </summary>
        /// <returns>The material document.</returns>
        [HttpPost]
        public IResult InsertMaterialDocument()
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
                result = _materialManager.SaveMaterialDocument(fileList, documentViewModel);
            }
            return result;
        }

        /// <summary>
        /// Update a material.
        /// </summary>
        /// <returns>The updated material model.</returns>
        /// <param name="materialViewModel">Material view model.</param>
        [HttpPut]
        public IResult Update([FromBody]MaterialViewModel materialViewModel)
        {
            var result = _materialManager.UpdateMaterial(materialViewModel);
            return result;
        }

        /// <summary>
        /// Delete a spesific material by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the material.</param>
        [HttpDelete]
        public void Delete(string id)
        {
            _materialManager.DeleteMaterial(id);
        }

        /// <summary>
        /// Manages the material part properties.
        /// </summary>
        /// <returns>The material part properties.</returns>
        /// <param name="partPropertyViewModel">Part property view model.</param>
        [HttpPut]
        public IResult ManageMaterialPartProperties([FromBody] PartPropertyViewModel partPropertyViewModel)
        {
            var result = _materialManager.ManageMaterialPartProperties(partPropertyViewModel);
            return result;
        }

        /// <summary>
        /// Manages the material tech specs.
        /// </summary>
        /// <returns>The material tech specs.</returns>
        /// <param name="techSpecsViewModel">Tech specs view model.</param>
        [HttpPut]
        public IResult ManageMaterialTechSpecs([FromBody] TechSpecsViewModel techSpecsViewModel)
        {
            var result = _materialManager.ManageMaterialTechnicalSpecs(techSpecsViewModel);
            return result;
        }

        /// <summary>
        /// Delete All Materials.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpDelete]
        public IResult DeleteAll([FromBody]UserLoginViewModel loginModel)
        {
            if (loginModel.UserName.Equals("test") && loginModel.UserPassword.Equals("test"))
            {
                return _materialManager.DeleteAllMaterial();
            }
            return null;
        }

        /// <summary>
        /// Insert Material Request
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResult> RequestMaterial()
        {
            IResult result = null;
            var requestViewModel = JsonConvert.DeserializeObject<MaterialRequestViewModel>(Request.Form["model"]);
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
                
                result = _materialManager.InsertMaterialRequest(fileList, requestViewModel);
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
        /// Prepare Email options for material requests
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        private EmailOptions PrepareEmailOptions(MaterialRequestViewModel requestViewModel)
        {
            var emailOptions = new EmailOptions
            {
                Template = MailTemplate.Material,
                PlainBody = string.Empty,
                Attachments = new List<Attachment>()
            };
            var msgBody = SendGridMailHelper.MailBody(_hostingEnvironment, emailOptions.Template);
            if (!string.IsNullOrEmpty(msgBody))
            {
                emailOptions.Subject = "Request for Material/Part Received - SubQuip";
                emailOptions.CcMail = new MailUser { Name = "SubQuip", Email = _configuration["requestEmail"] };
                emailOptions.ToMailsList = requestViewModel.MailUsers;
                emailOptions.HtmlBody = msgBody.Replace("{MaterialNumber}", requestViewModel.MaterialNumber).Replace("{FromDate}", requestViewModel.FromDate.ToString("dd-MM-yyyy")).Replace("{ToDate}", requestViewModel.ToDate.ToString("dd-MM-yyyy"));
            }
            return emailOptions;
        }

        #endregion
    }
}
