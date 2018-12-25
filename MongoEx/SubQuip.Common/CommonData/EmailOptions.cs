using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.Enums;

namespace SubQuip.Common.CommonData
{
    public class EmailOptions
    {
        public List<MailUser> ToMailsList { get; set; }
		public MailUser CcMail { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string PlainBody { get; set; }
        public MailTemplate Template { get; set; }
		public List<Attachment> Attachments { get; set; }
    }

    public class MailUser
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
