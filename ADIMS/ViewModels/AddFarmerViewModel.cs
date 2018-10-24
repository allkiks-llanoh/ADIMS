using ADIMS.Models;
using System;

namespace ADIMS.ViewModels
{
    public class AddFarmerViewModel
    {
        public string adimsid { get; set; }
        public int? idnumber { get; set; }
        public string passportno { get; set; }
        public string krapin { get; set; }
        public int? phoneno { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string dob { get; set; }
        public int? photo { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public string maritalstatus { get; set; }
        public string nextofkinname { get; set; }
        public string nexofkintype { get; set; }
        public string educationLevel { get; set; }
        public int? county { get; set; }
        public int? ward { get; set; }

        //Farmer insurance policies
        public bool hasInsurance { get; set; }
        public string insurance_type { get; set; }
        public string other_insurance { get; set; }
        public string insurer { get; set; }
        public int crop { get; set; }
        public string policy_number { get; set; }

        public string progresskey { get; set; }

    }
}