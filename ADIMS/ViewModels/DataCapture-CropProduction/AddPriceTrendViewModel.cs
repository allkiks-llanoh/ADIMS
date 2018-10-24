using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.DataCapture_CropProduction
{
    public class AddPriceTrendViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public string month { get; set; }
        public int season { get; set; }

        public string commodity { get; set; }

        public int total_sampled_farmers { get; set; }
        public int total_sampled_markets { get; set; }

        public double wholesale_price { get; set; }
        public double farmgate_price { get; set; }
    }
}