using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.ViewModel.TechSpecs
{
    public class TechSpecsViewModel
    {
        public string TechSpecId { get; set; }
        public string TechSpecName { get; set; }
        public string Value { get; set; }
        public string RegardingId { get; set; }
        public bool IncludeInOverview { get; set; }
    }
}
