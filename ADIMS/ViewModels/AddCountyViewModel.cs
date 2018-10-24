using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class AddCountyViewModel
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string code { get; set; }
    }
}