using System.Collections.Generic;
using SubQuip.ViewModel.PartProperties;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.Material;
using SubQuip.ViewModel.TechSpecs;
using System;
using SubQuip.Common.CommonData;

namespace SubQuip.ViewModel.Equipment
{
    public class EquipmentViewModel
    {
        public string EquipmentId { get; set; }

        public string EquipmentNumber { get; set; }

        public string SerialNumber { get; set; }

        public string Owner { get; set; }

        public string License { get; set; }

        public string Location { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<PartPropertyViewModel> PartProperties { get; set; }

        public List<FileDetails> Documents { get; set; }

        public List<TechSpecsViewModel> TechnicalSpecifications { get; set; }

        public string Material { get; set; }

        public MaterialViewModel MaterialDetails { get; set; }
    }
}
