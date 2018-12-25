using RS.Common.CommonData;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using RS.Common.Enums;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace RS.Common.Extensions
{
    public static class GenericHelper
    {

        public static UserClaim GetUserClaimDetails(ClaimsIdentity identity)
        {
            UserClaim userClaim = null;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                userClaim = new UserClaim
                {
                    Name = identity.FindFirst(ClaimTypes.Name).Value,
                    FirstName = identity.FindFirst(ClaimTypes.GivenName).Value,
                    LastName = identity.FindFirst(ClaimTypes.NameIdentifier).Value,
                    UserName = identity.FindFirst(ClaimTypes.Surname).Value,
                    Email = identity.FindFirst(ClaimTypes.Email).Value,
                    Role = identity.FindFirst(ClaimTypes.Role).Value,
                    UserId = new Guid(identity.FindFirst(ClaimTypes.Sid).Value)
                };
            }
            return userClaim;
        }

        /// <summary>
        /// Maps the audit columns.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="identity"></param>
        public static void MapAuditColumns(this object model, ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var authorizedInfo = GenericHelper.GetUserClaimDetails(identity);
                if (model != null && authorizedInfo != null)
                {
                    if (Convert.ToDateTime(GetColumnValue(Constants.CreatedDate, model)) == default(DateTime))
                    {
                        SetColumnValue(Constants.IsActiveColumn, model, true);
                        SetColumnValue(Constants.CreatedDate, model, DateTime.Now);
                        SetColumnValue(Constants.CreatedBy, model, authorizedInfo.UserId);
                    }
                    SetColumnValue(Constants.ModifiedDate, model, DateTime.Now);
                    SetColumnValue(Constants.ModifiedBy, model, authorizedInfo.UserId);
                }
            }
        }

        public static void MapDeleteColumns(this object model, ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var authorizedInfo = GenericHelper.GetUserClaimDetails(identity);
                if (model != null && authorizedInfo != null)
                {
                    SetColumnValue(Constants.IsActiveColumn, model, false);
                    SetColumnValue(Constants.IsDeletedColumn, model, true);
                    SetColumnValue(Constants.DeletedDate, model, DateTime.Now);
                    SetColumnValue(Constants.DeletedBy, model, authorizedInfo.UserId);
                }
            }
        }

        /// <summary>
        /// Gets the column value.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static object GetColumnValue(String columnName, object entity)
        {
            var pinfo = entity.GetType().GetProperty(columnName);
            if (pinfo == null) { return null; }
            return pinfo.GetValue(entity, null);
        }

        /// <summary>
        /// Sets the column value.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="value">The value.</param>
        public static void SetColumnValue(String columnName, object entity, object value)
        {
            var pinfo = entity.GetType().GetProperty(columnName);
            if (pinfo != null) { pinfo.SetValue(entity, value, null); }
        }

        public static async void SendMail(string email, string subject, string msgBody, IConfiguration configuration)
        {
            try
            {
                SmtpClient client = new SmtpClient(configuration["Host"]);
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(configuration["EmailId"], configuration["Password"]);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(configuration["EmailId"], configuration["Title"]);
                mailMessage.To.Add(email);
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = msgBody;
                mailMessage.Subject = subject;
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Send(MailDetailModel mailDetail, IConfiguration config, IHostingEnvironment hostingEnvironment)
        {
            var msgBody = "";
            if (mailDetail.Template == TemplateType.UserRegistration)
            {
                var file = config["UserRegistrationTemplatePath"];
                var path = Path.Combine(hostingEnvironment.ContentRootPath, file);
                using (StreamReader reader = new StreamReader(path))
                {
                    msgBody = reader.ReadToEnd();
                }
                var userViewModel = mailDetail.MessageBody;
                msgBody = msgBody.Replace("{FirstName}", userViewModel.FirstName);
                msgBody = msgBody.Replace("{Password}", userViewModel.Password);
                msgBody = msgBody.Replace("{UserName}", userViewModel.UserName);
            }
            else if (mailDetail.Template == TemplateType.ScheduleUserForInterview)
            {
                var file = config["ScheduleInterviewTemplatePath"];
                var path = Path.Combine(hostingEnvironment.ContentRootPath, file);
                using (StreamReader reader = new StreamReader(path))
                {
                    msgBody = reader.ReadToEnd();
                }
                var scheduledUser = mailDetail.MessageBody;
                msgBody = msgBody.Replace("{FirstName}", scheduledUser.User.FirstName);
                msgBody = msgBody.Replace("{ApprovalEvent}", scheduledUser.ApprovalEvent.ApprovalEventName);
                msgBody = msgBody.Replace("{Date}", scheduledUser.InterviewScheduledDate);
                msgBody = msgBody.Replace("{Time}", scheduledUser.InterviewScheduledTime);
                msgBody = msgBody.Replace("{Candidate}", scheduledUser.Candidate.FirstName + " " + scheduledUser.Candidate.LastName);
            }
            else if (mailDetail.Template == TemplateType.InterviewCancelled)
            {
                var file = config["InterviewCancelledTemplatePath"];
                var path = Path.Combine(hostingEnvironment.ContentRootPath, file);
                using (StreamReader reader = new StreamReader(path))
                {
                    msgBody = reader.ReadToEnd();
                }
                var scheduledUser = mailDetail.MessageBody;
                msgBody = msgBody.Replace("{FirstName}", scheduledUser.User.FirstName);
                msgBody = msgBody.Replace("{ApprovalEvent}", scheduledUser.ApprovalEvent.ApprovalEventName);
                msgBody = msgBody.Replace("{Date}", scheduledUser.InterviewScheduledDate);
                msgBody = msgBody.Replace("{Time}", scheduledUser.InterviewScheduledTime);
                msgBody = msgBody.Replace("{Candidate}", scheduledUser.Candidate.FirstName + " " + scheduledUser.Candidate.LastName);
            }
            else
            {
                msgBody = "";
            }
            SendMail(mailDetail.EmailId, mailDetail.Subject, msgBody, config);
        }

        public static string DecryptPassword(string encryptedString)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.BlockSize = 128;
            myRijndael.KeySize = 128;
            myRijndael.IV = HexStringToByteArray("abcdef9876543210abcdef9876543210");

            myRijndael.Padding = PaddingMode.None;
            myRijndael.Mode = CipherMode.ECB;
            myRijndael.Key = HexStringToByteArray("0123456789abcdef0123456789abcdef");
            byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
            var decryptor = myRijndael.CreateDecryptor(myRijndael.Key, myRijndael.IV);
            byte[] originalBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(originalBytes).Trim();
        }

        private static byte[] HexStringToByteArray(string strHex)
        {
            dynamic r = new byte[strHex.Length / 2];
            for (int i = 0; i <= strHex.Length - 1; i += 2)
            {
                r[i / 2] = Convert.ToByte(Convert.ToInt32(strHex.Substring(i, 2), 16));
            }
            return r;
        }
    }
}
