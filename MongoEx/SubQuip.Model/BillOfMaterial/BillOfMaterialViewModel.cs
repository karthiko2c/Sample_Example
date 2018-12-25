using System.Collections.Generic;
using System;
using SubQuip.Common.Enums;
using SubQuip.ViewModel.Material;
using SubQuip.ViewModel.Equipment;

namespace SubQuip.ViewModel.BillOfMaterial
{
    public class BillOfMaterialViewModel
    {
        public string BomId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Image { get; set; }

        public string ImageContent { get; set; }

        public BomStatus Status { get; set; }

        public BomUserViewModel BomUser { get; set; }
      
        public BomType Type { get; set; }

        /// <summary>
        /// template id itself a bom id, when used as template for creating a new bom.
        /// </summary>
        public string TemplateId { get; set; }

        public BomOptionViewModel Option { get; set; }

        public List<BomGroupViewModel> Groups { get; set; }

        public List<BomCommentViewModel> Comments { get; set; }
    }

    public class Item
    {
        public List<MaterialViewModel> Materials { get; set; }

        public List<EquipmentViewModel> Equipments { get; set; }
    }

    public class BomsForTemplateViewModel
    {
        public string BomId { get; set; }

        public string BomName { get; set; }

        public string BomTemplateName { get; set; }

        public BomUserViewModel BomUser { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }

    public class BomTemplateViewModel
    {
        public string BomTemplateId { get; set; }

        public string BomTemplateName { get; set; }

        public int Count { get; set; } 

    }
}