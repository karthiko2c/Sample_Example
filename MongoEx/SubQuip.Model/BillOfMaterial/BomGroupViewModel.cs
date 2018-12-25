using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.Enums;

namespace SubQuip.ViewModel.BillOfMaterial
{
    public class BomGroupViewModel
    {
        public string Name { get; set; }

        /// <summary>
        /// Bill of material
        /// </summary>
       public List<BillOfMaterialViewModel> BillOfMaterials { get; set; }

        /// <summary>
        /// Items can be equipment or material
        /// </summary>
        public List<BomItemViewModel> BomItems { get; set; }
    }
   
}
