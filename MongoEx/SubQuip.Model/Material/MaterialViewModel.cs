using System.Collections.Generic;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.PartProperties;
using SubQuip.ViewModel.TechSpecs;
using System;
using SubQuip.Common.CommonData;

namespace SubQuip.ViewModel.Material
{
    public class MaterialViewModel
    {
        public string MaterialId { get; set; } //Should be unique, hidden from user

        public string Materialnumber { get; set; }

        public string Description { get; set; }

        public string ManufactorPartNumber { get; set; }

        public string ManufactorName { get; set; }

        public string Owner { get; set; }

        public string License { get; set; }

        public string Type { get; set; }

        public string GrossWeight { get; set; }

        public string SizeDimension { get; set; }

        public string Quantity { get; set; }

        public bool IsSparePart { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<PartPropertyViewModel> PartProperties { get; set; }

        public List<FileDetails> Documents { get; set; }

        public List<TechSpecsViewModel> TechnicalSpecifications { get; set; }
    }

    public class MaterialImportViewModel : MaterialViewModel
    {
        public bool IsValidRecord { get; set; }

        public string Message { get; set; }
    }

    public class MaterialImportResult
    {
        public int InsertedCount { get; set; }

        public int UpdatedCount { get; set; }
    }
}
