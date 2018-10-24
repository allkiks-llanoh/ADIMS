using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class CCEQueueReportViewModel
    {
        public string UAIName { get; set; }
        public int? farmsSampled { get; set; }
        public decimal? averageWetWeight { get; set; }
        public decimal? dryWeight { get; set; }
        public decimal? averageMoistureContent { get; set; }
        public double? averageFarmSize { get; set; }
    }
}