using RS.Common.Enums;

namespace RS.Common.CommonData
{
    public class MailDetailModel
    {
        public string Subject { get; set; }
        public string EmailId { get; set; }
        public dynamic MessageBody { get; set; }
        public TemplateType Template { get; set; }
    }
}

