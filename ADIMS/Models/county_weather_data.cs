//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ADIMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class county_weather_data
    {
        public int id { get; set; }
        public Nullable<int> county { get; set; }
        public Nullable<int> sub_county { get; set; }
        public Nullable<int> month { get; set; }
        public Nullable<int> season { get; set; }
        public string station_name { get; set; }
        public Nullable<double> total_rainfall { get; set; }
        public Nullable<int> total_no_of_rainy_days { get; set; }
        public Nullable<double> spatial_distribution { get; set; }
        public string remarks { get; set; }
    
        public virtual county county1 { get; set; }
        public virtual season season1 { get; set; }
        public virtual subcounty subcounty { get; set; }
    }
}
