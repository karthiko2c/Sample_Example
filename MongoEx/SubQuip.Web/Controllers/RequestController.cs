using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SubQuip.Common.CommonData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.Request;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SubQuip.Common.Enums;
using SubQuip.Common.Extensions;
using SubQuip.Common;
using SubQuip.ViewModel.User;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Request controller for sending a request to SubQuip
    /// </summary>
    [Produces("application/json")]
    [Route("api/Request/[Action]")]
    [ValidateModel]
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IRequestManagerService _requestManager;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance the controller
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="hostingEnvironment">Hosting environment.</param>
        /// <param name="requestManager">Request manager.</param>
        public RequestController(IConfiguration configuration, IHostingEnvironment hostingEnvironment, IRequestManagerService requestManager)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _requestManager = requestManager;
        }

        /// <summary>
        /// Search for submitted requests.
        /// </summary>
        /// <returns>The requests.</returns>
        /// <param name="search">Search.</param>
        [HttpGet]
        public IResult Requests(SearchSortModel search)
        {
            var requestList = _requestManager.GetRequests(search);
            return requestList;
        }

        /// <summary>
        /// Get a submitted request by its identifier.
        /// </summary>
        /// <returns>The details of the request.</returns>
        /// <param name="id">Identifier of the request.</param>
        [HttpGet]
        public IResult Details(string id)
        {
            var requestRecord = _requestManager.GetRequestById(id);
            return requestRecord;
        }

        /// <summary>
        /// Create and submit a new request.
        /// </summary>
        /// <returns>Result model.</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IResult> Create()
        {
            IResult result = null;
            var requestViewModel = JsonConvert.DeserializeObject<RequestViewModel>(Request.Form["model"]);
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
                result = _requestManager.InsertRequest(fileList, requestViewModel);
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

                        var response = await SendGridMailHelper.SendEmail(_configuration, emailOptions);
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
        /// Update a request.
        /// </summary>
        /// <returns>The updated request.</returns>
        [HttpPut]
        public IResult Update()
        {
            IResult result = null;
            var requestViewModel = JsonConvert.DeserializeObject<RequestViewModel>(Request.Form["model"]);
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
                result = _requestManager.UpdateRequest(fileList, requestViewModel);
            }
            return result;
        }

        /// <summary>
        /// Delete a request by the its identifier.
        /// </summary>
        /// <returns>The request to delete.</returns>
        /// <param name="id">Identifier ofthe request to delete.</param>
        [HttpDelete]
        public IResult Delete(string id)
        {
            return _requestManager.DeleteRequest(id);
        }

        /// <summary>
        /// Delete All Requests.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpDelete]
        public IResult DeleteAll([FromBody]UserLoginViewModel loginModel)
        {
            if (loginModel.UserName.Equals("test") && loginModel.UserPassword.Equals("test"))
            {
                return _requestManager.DeleteAllRequests();
            }
            return null;
        }

        #region Private methods

        /// <summary>
        /// Prepare Email options for various system requests
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        private EmailOptions PrepareEmailOptions(RequestViewModel requestViewModel)
        {
            var emailOptions = new EmailOptions();
            var msgBody = SendGridMailHelper.MailBody(_hostingEnvironment, emailOptions.Template);
            if (!string.IsNullOrEmpty(msgBody))
            {             
                emailOptions.Subject = "Request Received - SubQuip";
                emailOptions.CcMail = new MailUser { Name = requestViewModel.MailUsers[0].Name, Email = requestViewModel.MailUsers[0].Email };
                emailOptions.HtmlBody = msgBody.Replace("{Name}", emailOptions.CcMail.Name).Replace("{Email}", emailOptions.CcMail.Email).Replace("{Company}", requestViewModel.Company).Replace("{PhoneNumber}", requestViewModel.PhoneNumber).Replace("{FromDate}", requestViewModel.FromDate.ToString("dd-MM-yyyy")).Replace("{ToDate}", requestViewModel.ToDate.ToString("dd-MM-yyyy")).Replace("{Description}", requestViewModel.Description).Replace("{Type}", requestViewModel.Type.ToString());
                emailOptions.PlainBody = string.Empty;
                emailOptions.Template = MailTemplate.Request;
                emailOptions.ToMailsList = new List<MailUser> { new MailUser { Name = "SubQuip", Email = _configuration["requestEmail"] } };
                emailOptions.Attachments = new List<Attachment>
                    {
                        new Attachment()
                        {
                            Content = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestViewModel)),
                            Name = "Request.json",
                            ContentType = "text/json"
                        }
                    };
            }
            return emailOptions;
        }

        #endregion
    }
}