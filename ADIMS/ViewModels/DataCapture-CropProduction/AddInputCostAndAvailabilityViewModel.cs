using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddInputCostAndAvailabilityViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public int season { get; set; }
        public int no_sampled_stocklist { get; set; }
        public int no_sampled_ncpb_stores { get; set; }
        public string type_of_input { get; set; }
        public string unit { get; set; }
        public int crop_condition { get; set; }
        public double lowest_retail_price { get; set; }
        public double highest_retail_price { get; set; }
        public int availability { get; set; }
    }
}