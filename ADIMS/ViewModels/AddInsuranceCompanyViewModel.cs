using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class AddInsuranceCompanyViewModel
    {
        [Required]
        public string name { get; set; }
        public string initials { get; set; }
        public string krapin { get; set; }
        public string distribution_channel { get; set; }
        public string channel_name { get; set; }
        public string phoneno { get; set; }
        public string email { get; set; }
        public string postal_address { get; set; }
        public string town { get; set; }
        public string physical_address { get; set; }
        public DateTime? date_created { get; set; }
    }
}