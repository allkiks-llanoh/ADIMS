using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddEarlyWarningViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public int season { get; set; }

        public int crop { get; set; }

        public int crop_growth_stage { get; set; }
        public int rainfall_perfomance { get; set; }
        public int achieved_acreage { get; set; }
        public int time_of_planting { get; set; }
        public int crop_condition { get; set; }
        public int water_availability { get; set; }
        public int expected_yield_compared_to_LTA { get; set; }
        public int foodstocks_available_compared_toNormal { get;set; }
    }
}