using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels.AYII
{
    public class UpdateUAIVariablesViewModel
    {
        public int uai { get; set; }
        public float historical_average { get; set; }
        public float average_yield { get; set; }
        public float coverage { get; set; }
        public float unit_value { get; set; }
        public int season { get; set; }
        public int year { get; set; }
    }
}