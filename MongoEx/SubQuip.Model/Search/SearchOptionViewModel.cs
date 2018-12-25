using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.ViewModel.Search
{
    public class SearchOptionViewModel
    {

        public Object id { get; set; }

        public string name { get; set; }
        public string description { get; set; }

        public string material_number { get; set; }

        public string manufactor_part_number { get; set; }

        public string manufactor_name { get; set; }
        public string equipment_number { get; set; }
        public string serial_number { get; set; }

        public string license { get; set; }
        public string location { get; set; }
    }
}
