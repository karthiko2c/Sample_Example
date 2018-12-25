using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Common.CommonData
{
    public class SendGridSetting
    {
        public string ApiKey { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }
    }
}
