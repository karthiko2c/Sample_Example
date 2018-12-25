using SubQuip.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.ViewModel.BillOfMaterial
{
    public class AddItemToBom
    {
        public string BomId { get; set; }

        public string RegardingId { get; set; }

        public BomItemType ItemType { get; set; }
    }
}
