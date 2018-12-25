using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.Enums;

namespace SubQuip.ViewModel.BillOfMaterial
{
    public class BomCommentViewModel
    {
        /// <summary>
        /// unique id for BOM
        /// </summary>
        public string RegardingId { get; set; }

        /// <summary>
        /// Comment added
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Name of user added the comment
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// Comment creation date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Comment creation date
        /// </summary>
        public string CreatedBy { get; set; }
    }
   
}
