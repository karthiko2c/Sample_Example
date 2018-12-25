using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.Enums;

namespace SubQuip.ViewModel.BillOfMaterial
{
    public class BomItemViewModel
    {
        /// <summary>
        /// Equipment or material type
        /// </summary>
        public BomItemType ItemType { get; set; }

        /// <summary>
        /// Equipment or material
        /// </summary>
        public dynamic Item { get; set; }
    }
}
