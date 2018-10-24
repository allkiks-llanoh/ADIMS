using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddDisasterWarningDataViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public int season { get; set; }

        public string disaster_type { get; set; }
        public int crop_damaged { get; set; }
        public double hectared_destroyed { get; set; }
        public double production_loss { get; set; }

        public double estimated_loss_value { get; set; }
        public string remarks { get; set; }
    }
}