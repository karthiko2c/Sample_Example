using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Newtonsoft.Json;
using SubQuip.Common.Enums;

namespace SubQuip.Common.CommonData
{
    public static class SendGridMailHelper
    {
        /// <summary>
        /// Sendgrid api and other settings
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static SendGridSetting MailSettings(IConfiguration configuration)
        {
            return new SendGridSetting
            {
                ApiKey = configuration["SendGridApiKey"],
                FromName = configuration["FromName"],
                FromEmail = configuration["FromEmail"],
            };
        }

        public static async Task<Response> SendEmail(IConfiguration configuration, EmailOptions options)
        {
            var settings = MailSettings(configuration);
            var client = new SendGridClient(settings.ApiKey);
            var from = new EmailAddress(settings.FromEmail, settings.FromName);
            var to = new EmailAddress(options.ToMailsList[0].Email, options.ToMailsList[0].Name);
            var msg = MailHelper.CreateSingleEmail(from, to, options.Subject, options.PlainBody, options.HtmlBody);
            msg.AddCc(options.CcMail.Email, options.CcMail.Name);
            if (options.Attachments != null && options.Attachments.Any())
            {
                msg.Attachments = options.Attachments.Select(a => new SendGrid.Helpers.Mail.Attachment
                {
                    Content = Convert.ToBase64String(a.Content),
                    Filename = a.Name,
                    Type = a.ContentType
                }).ToList();
            }


            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public static async Task<Response> SendSingleEmailToMultipleRecipients(IConfiguration configuration, EmailOptions options)
        {
            var settings = MailSettings(configuration);
            var client = new SendGridClient(settings.ApiKey);
            var from = new EmailAddress(settings.FromEmail, settings.FromName);
            List<EmailAddress> tos = options.ToMailsList.Select(t => new EmailAddress(t.Email, t.Name)).ToList();

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, options.Subject, options.PlainBody, options.HtmlBody, false);
            msg.AddCc(options.CcMail.Email, options.CcMail.Name);
            if (options.Attachments != null && options.Attachments.Any())
            {
                msg.Attachments = options.Attachments.Select(a => new SendGrid.Helpers.Mail.Attachment
                {
                    Content = Convert.ToBase64String(a.Content),
                    Filename = a.Name,
                    Type = a.ContentType
                }).ToList();
            }


            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public static string MailBody(IHostingEnvironment hostingEnvironment, MailTemplate template)
        {
            var path = Path.Combine(hostingEnvironment.ContentRootPath, "MailTemplate");
            var msgBody = string.Empty;
            switch (template)
            {
                case MailTemplate.Request:
                    path += "/Request.html";
                    break;
                case MailTemplate.ContactUs:
                    path += "/ContactUs.html";
                    break;
                case MailTemplate.BillOfMaterial:
                    path += "/BomRequest.html";
                    break;
                case MailTemplate.Material:
                    path += "/MaterialRequest.html";
                    break;
                case MailTemplate.Equipment:
                    path += "/EquipmentRequest.html";
                    break;
            }
            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    msgBody = reader.ReadToEnd();
                }
            }
            return msgBody;
        }
    }


}


