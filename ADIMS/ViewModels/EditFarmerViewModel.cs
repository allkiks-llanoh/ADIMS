using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class EditFarmerViewModel
    {
        public int id { get; set; }
        public string adimsid { get; set; }
        public int? idnumber { get; set; }
        public string passportno { get; set; }
        public string krapin { get; set; }
        public int? phoneno { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public int dob { get; set; }
        public int? photo { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public string maritalstatus { get; set; }
        public string nextofkinname { get; set; }
        public string nexofkintype { get; set; }
        public int? county { get; set; }
        public int? ward { get; set; }
    }
}