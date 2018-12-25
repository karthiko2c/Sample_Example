using System;
using System.Collections.Generic;
using SubQuip.ViewModel.Request;
using SubQuip.ViewModel.BillOfMaterial;

namespace SubQuip.ViewModel.Statistics
{
    public class Statistics
    {
		public int NumBoms { get; set; }
        public int NumBomsInProgress { get; set; }
		public int NumBomsCompleted { get; set; }
        
		public int NumEquipments { get; set; }
		public int NumEquipmentsWithDocumentation { get; set; }
		public List<PartnerStatistics> NumEquipmentForPartner { get; set; }
        public int NumRecentEquipments { get; set; }

        public int NumMaterials { get; set; }
        public int NumMaterialsWithDocumentation { get; set; }
        public int NumRecentMaterials { get; set; }

        public List<BillOfMaterialViewModel> LastFiveRequests { get; set; }
    }
}
