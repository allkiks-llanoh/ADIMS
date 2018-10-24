using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddGovtSubsidizedFertlizerViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public int season { get; set; }
        public string subsidized_fertilizer { get; set; }

        public double total_bags_availed_toDate { get; set; }
        public double total_bags_issued { get; set; }
        public int total_beneficiary_farmers { get; set; }
    }
}