using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddTopDressingFertilizerViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public int season { get; set; }
        public int core_crop { get; set; }
        public double hectares_planted { get; set; }

        public string top_dressing_fertilizer_1 { get; set; }
        public double estimated_ha_applied_fertilizer_1 { get; set; }
        public double fertilizer_use_rate_1 { get; set; }
        public double total_fertilizer_used_1 { get; set; }

        public string top_dressing_fertilizer_2 { get; set; }
        public double estimated_ha_applied_fertilizer_2 { get; set; }
        public double fertilizer_use_rate_2 { get; set; }
        public double total_fertilizer_used_2 { get; set; }
    }
}