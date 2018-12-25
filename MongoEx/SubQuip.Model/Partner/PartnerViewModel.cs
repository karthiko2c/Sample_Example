using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.ViewModel.Partner
{
    public class PartnerViewModel
    {
        public string PartnerId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
