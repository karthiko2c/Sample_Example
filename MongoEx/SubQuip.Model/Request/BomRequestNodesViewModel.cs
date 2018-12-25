using SubQuip.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.ViewModel.Request
{
    public class BomRequestNodeViewModel
    {
        /// <summary>
        /// Material or equipment Identifier
        /// </summary>
        public string RegardingId { get; set; }

        public string ItemNumber { get; set; }

        public BomItemType Type { get; set; }
    }
}
