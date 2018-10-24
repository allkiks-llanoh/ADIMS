using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class EditUserViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }

        public bool Administrator { get; set; }
        public bool ExtensionOfficer { get; set; }
        public bool MinistryOfficial { get; set; }
    }
}