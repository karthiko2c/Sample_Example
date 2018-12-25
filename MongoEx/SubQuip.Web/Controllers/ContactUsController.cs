using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;
using SubQuip.ViewModel.ContactUs;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Contact us controller, to handle the "contact us" form
    /// </summary>
    [Produces("application/json")]
    [Route("api/ContactUs")]
    public class ContactUsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SubQuip.WebApi.Controllers.ContactUsController"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        /// <param name="hostingEnvironment">Hosting environment.</param>
        public ContactUsController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Used for sending emails directly to Subquip.
        /// </summary>
        /// <returns>Result model</returns>
        /// <param name="contactUsViewModel">Data needed to send a contact us email</param>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IResult> Create([FromBody] ContactUsViewModel contactUsViewModel)
        {
            var options = new EmailOptions
            {
                Subject = "Contact form received - SubQuip",
                CcMail = new MailUser { Name = contactUsViewModel.Name, Email = contactUsViewModel.Email },
                ToMailsList = new List<MailUser> { new MailUser { Name = "SubQuip", Email = _configuration["contactEmail"] } },
                PlainBody = string.Empty,
                Template = MailTemplate.ContactUs
            };
            var msgBody = SendGridMailHelper.MailBody(_hostingEnvironment, options.Template);
            if (!string.IsNullOrEmpty(msgBody))
            {
                var result = new Result { Status = Status.Success, Message = string.Empty, Body = string.Empty, Operation = Operation.Create };
                options.HtmlBody = msgBody.Replace("{Name}", contactUsViewModel.Name).Replace("{Email}", contactUsViewModel.Email).Replace("{Company}", contactUsViewModel.Company).Replace("{Message}", contactUsViewModel.Message).Replace("{PhoneNumber}", contactUsViewModel.PhoneNumber);
                var response = await SendGridMailHelper.SendEmail(_configuration, options);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    result.Message = "Mail sent.";
                    result.Body = "Mail Sent";
                }
                else
                {
                    result.Message = "Mail not sent to provided email.";
                }
                return result;
            }
            return new Result { Status = Status.Fail, Message = "Mail not sent, as email template not found", Body = "Mail not sent", Operation = Operation.Create };
        }
    }
}