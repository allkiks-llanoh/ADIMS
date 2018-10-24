using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddFoodStocksDataViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public int season { get; set; }

        public string commodity { get; set; }

        public int total_sampled_farmers { get; set; }
        public int total_sampled_traders { get; set; }
        public int total_sampled_millers { get; set; }
        public int total_ncpb_depots { get; set; }
        public int total_food_aid_agencies { get; set; }

        public int farmer_bags { get; set; }
        public int trader_bags { get; set; }
        public int miller_bags { get; set; }
        public int ncpb_bags { get; set; }
        public int food_aid_bags { get; set; }
    }
}