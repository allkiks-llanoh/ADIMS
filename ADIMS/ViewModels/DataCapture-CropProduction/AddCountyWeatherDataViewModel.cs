using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddCountyWeatherDataViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public string station_name { get; set; }
        public double total_rainfall { get; set; }
        public int total_no_of_rainy_days { get; set; }
        public double spatial_distribution { get; set; }
        public int season { get; set; }
        public string remarks { get; set; }
    }
}