using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddCropProductionStatisticsViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public int season { get; set; }
        public int no_sampled_ls_farms { get; set; }
        public int no_sampled_ci_farms { get; set; }
        public int no_sampled_irrigation_schemes { get; set; }
        public int no_sampled_greenhouses { get; set; }
        public int no_ss_farms { get; set; }
        public int core_crop { get; set; }
        public double hectares_planted { get; set; }
        public int estimated_ha_certified_seeds { get; set; }
        public double estimated_seed_quantity { get; set; }
    }
}