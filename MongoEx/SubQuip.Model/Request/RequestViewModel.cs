using System;
using System.Collections.Generic;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;

namespace SubQuip.ViewModel.Request
{

    public class RequestViewModel
    {
        public string RequestId { get; set; }

        public RequestFormType Type { get; set; }

        public string Description { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string RequestedBy { get; set; }

        public string Company { get; set; }

        public string PhoneNumber { get; set; }

        public List<MailUser> MailUsers { get; set; }

        public List<string> Files { get; set; }
    }

    public class BomRequestViewModel : RequestViewModel
    {
        /// <summary>
        /// Bom identifier
        /// </summary>
        public string RegardingId { get; set; }

        public List<BomRequestNodeViewModel> BomRequestNodes { get; set; }
    }

    public class EquipmentRequestViewModel : RequestViewModel
    {
        /// <summary>
        /// Equipment Identifier
        /// </summary>
        public string RegardingId { get; set; }

        public string EquipmentNumber { get; set; }
    }

    public class MaterialRequestViewModel : RequestViewModel
    {
        /// <summary>
        /// Material Identifier
        /// </summary>
        public string RegardingId { get; set; }

        public string MaterialNumber { get; set; }

    }
}


