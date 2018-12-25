using System.Linq;
using SubQuip.Business.Interfaces;
using SubQuip.Common.CommonData;
using SubQuip.ViewModel.BillOfMaterial;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using SubQuip.Common.Extensions;
using SubQuip.Common;
using Microsoft.AspNetCore.Authorization;
using SubQuip.ViewModel.User;
using System.Threading.Tasks;
using SubQuip.ViewModel.Request;
using System.Collections.Generic;
using SubQuip.Common.Enums;
using System.IO;
using System.Net;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Bill of materials controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/BillOfMaterials/[Action]")]
    [Authorize]
    public class BillOfMaterialsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IBillOfMaterialsManagerService _billOfMaterialsManager;

        /// <summary>
        /// Initializes a new instance of the BillOfMaterialsController
        /// </summary>
        /// <param name="billOfMaterialsManager">Bill of materials manager.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="hostingEnvironment">Hosting environment.</param>
        public BillOfMaterialsController(IBillOfMaterialsManagerService billOfMaterialsManager, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _billOfMaterialsManager = billOfMaterialsManager;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Search for a Bill Of Material (BOM) owned by a user.
        /// </summary>
        /// <returns>The of materials for that spesific user.</returns>
        /// <param name="search">Search Model.</param>
        /// <param name="userid">Spesific Userid.</param>
        [HttpGet]
        public IResult BillOfMaterialsForUser(SearchSortModel search, string userid)
        {
            var boms = _billOfMaterialsManager.GetBillOfMaterialsForUser(search, userid);
            return boms;
        }

        /// <summary>
        /// Get a spesific Bill Of Material (BOM) by its identifier.
        /// </summary>
        /// <returns>The spesific material.</returns>
        /// <param name="id">Identifier of the material.</param>
        [HttpGet]
        public IResult Details(string id)
        {
            var bom = _billOfMaterialsManager.GetBillOfMaterialById(id);
            return bom;
        }

        /// <summary>
        /// Get Boms of Specific Template.
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResult BomsForTemplate(string templateId)
        {
            var bom = _billOfMaterialsManager.GetBomsForTemplate(templateId);
            return bom;
        }

        /// <summary>
        /// Get all Bom Templates.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public IResult BomTemplates(SearchSortModel search)
        {
            var bomTemplates = _billOfMaterialsManager.GetBomTemplates(search);
            return bomTemplates;
        }

        /// <summary>
        /// Create a Bill Of Material (BOM) / BOM Template.
        /// </summary>
        /// <returns>The created Bom Of Material / BOM Template.</returns>
        [HttpPost]
        public IResult Create()
        {
            IResult result = null;
            FileDetails fileModel = null;
            var bomViewModel = JsonConvert.DeserializeObject<BillOfMaterialViewModel>(Request.Form["model"]);
            if (bomViewModel != null)
            {
                var files = Request.Form.Files;
                if (files.Any())
                {
                    var file = files[0];
                    fileModel = FileHelper.GetFileDetails(file);
                }
            }
            result = _billOfMaterialsManager.CreateBillOfMaterial(bomViewModel, fileModel);
            return result;
        }

        /// <summary>
        /// Update a Bill Of Material (BOM)
        /// </summary>
        /// <returns>The update.</returns>
        [HttpPut]
        public IResult Update()
        {
            IResult result = null;
            FileDetails fileModel = null;
            var bomViewModel = JsonConvert.DeserializeObject<BillOfMaterialViewModel>(Request.Form["model"]);
            if (bomViewModel != null)
            {
                var files = Request.Form.Files;
                if (files.Any())
                {
                    var file = files[0];
                    fileModel = FileHelper.GetFileDetails(file);
                }
            }
            result = _billOfMaterialsManager.UpdateBillOfMaterial(bomViewModel, fileModel);
            return result;
        }

        /// <summary>
        /// Delete a Bill Of Material (BOM) by its identifier.
        /// </summary>
        /// <returns>The deleted Bill Of Material.</returns>
        /// <param name="id">Identifier of the Bill of Material (BOM).</param>
        [HttpDelete]
        public IResult Delete(string id)
        {
            return _billOfMaterialsManager.DeleteBillOfMaterial(id);
        }

        /// <summary>
        /// Delete All Bill Of Material (BOM). 
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpDelete]
        public IResult DeleteAll([FromBody]UserLoginViewModel loginModel)
        {
            if (loginModel.UserName.Equals("test") && loginModel.UserPassword.Equals("test"))
            {
                return _billOfMaterialsManager.DeleteAllBillOfMaterial();
            }
            return null;
        }

        /// <summary>
        /// Adds comment to existing BOM
        /// </summary>
        /// <returns>The created comment on Bom</returns>
        [HttpPost]
        public IResult AddCommentToBOM([FromBody]BomCommentViewModel bomCommentViewModel)
        {
            IResult result = null;
            result = _billOfMaterialsManager.SaveCommentForBOM(bomCommentViewModel);
            return result;
        }

        /// <summary>
        /// Add Option to BOM.
        /// </summary>
        /// <param name="bomOptionViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult AddOptionToBOM([FromBody]BomOptionViewModel bomOptionViewModel)
        {
            IResult result = null;
            result = _billOfMaterialsManager.SaveOptionForBOM(bomOptionViewModel);
            return result;
        }
        
        /// <summary>
        /// Retrieve comments related to BOM
        /// </summary>
        /// <returns>Comments for specific BOM.</returns>
        /// <param name="bomId">Specific BomId.</param>
        [HttpGet]
        public IResult CommentsForBom(string bomId)
        {
            var boms = _billOfMaterialsManager.GetCommentsForBom(bomId);
            return boms;
        }

        /// <summary>
        /// Create and submit a new request.
        /// </summary>
        /// <returns>Result model.</returns>
        [HttpPost]
        public async Task<IResult> CreateBOMRequest()
        {
            IResult result = null;
            var requestViewModel = JsonConvert.DeserializeObject<BomRequestViewModel>(Request.Form["model"]);
            if (requestViewModel != null)
            {
                requestViewModel.Type = RequestFormType.BillOfMaterial;
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
                result = _billOfMaterialsManager.InsertBOMRequest(fileList, requestViewModel);
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

        /// <summary>
        /// Add Equipment/Material To BOM.
        /// </summary>
        /// <param name="addItemToBom"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult AddItemToBom([FromBody]AddItemToBom addItemToBom)
        {
            var bom = _billOfMaterialsManager.AddItemToBom(addItemToBom);
            return bom;
        }

        /// <summary>
        /// Get Added Items of BOM.
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResult GetAddedItemsOfBom(string bomId)
        {
            var bom = _billOfMaterialsManager.GetAddedItemsOfBom(bomId);
            return bom;
        }

        /// <summary>
        /// Prepare Email options for various system requests
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        private EmailOptions PrepareEmailOptions(BomRequestViewModel requestViewModel)
        {
            var emailOptions = new EmailOptions
            {
                Template = MailTemplate.BillOfMaterial,
                PlainBody = string.Empty,
                Attachments = new List<Attachment>()
            };
            var msgBody = SendGridMailHelper.MailBody(_hostingEnvironment, emailOptions.Template);
            if (!string.IsNullOrEmpty(msgBody))
            {
                emailOptions.Subject = "Request for Bill Of Material Received - SubQuip";
                emailOptions.CcMail = new MailUser { Name = "SubQuip", Email = _configuration["requestEmail"] };
                emailOptions.ToMailsList = requestViewModel.MailUsers;
                emailOptions.HtmlBody = msgBody.Replace("{BomDescription}", requestViewModel.Description).Replace("{Description}", PrepareHtmlBody(requestViewModel)).Replace("{FromDate}", requestViewModel.FromDate.ToString("dd-MM-yyyy")).Replace("{ToDate}", requestViewModel.ToDate.ToString("dd-MM-yyyy"));
            }
            return emailOptions;
        }

        /// <summary>
        /// Prepare HTML Msg Body
        /// </summary>
        /// <returns></returns>
        private string PrepareHtmlBody(BomRequestViewModel requestViewModel)
        {
            if (requestViewModel.BomRequestNodes != null && requestViewModel.BomRequestNodes.Any())
            {
                StringBuilder sb = new StringBuilder();
                var materialItems = requestViewModel.BomRequestNodes.Where(x => x.Type == BomItemType.Material).ToList();
                if (materialItems.Any())
                {
                    var materialItemNumbers = string.Join(", ", materialItems.Select(x => x.ItemNumber));
                    sb.Append("<div class='description'>");
                    sb.Append("<div><span> I would like to order subsea " + BomItemType.Material.ToString() + "</span></div>");
                    sb.Append("<div><span class='form-header'>Part Name/Number: </span><span class='form-data'>" + materialItemNumbers + "</span></div>");
                    sb.Append("</div>");
                }

                var equipmentItems = requestViewModel.BomRequestNodes.Where(x => x.Type == BomItemType.Equipment).ToList();
                if (equipmentItems.Any())
                {
                    var equipmentItemNumbers = string.Join(", ", equipmentItems.Select(x => x.ItemNumber));
                    sb.Append("<div class='description'>");
                    sb.Append("<div><span> I would like to order subsea " + BomItemType.Equipment.ToString() + "</span></div>");
                    sb.Append("<div><span class='form-header'>Equipment Name/Number: </span><span class='form-data'>" + equipmentItemNumbers + "</span></div>");
                    sb.Append("</div>");
                }
                return sb.ToString();
            }
            return null;
        }
    }
}