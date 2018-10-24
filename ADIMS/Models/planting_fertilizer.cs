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
    
    public partial class planting_fertilizer
    {
        public int id { get; set; }
        public Nullable<int> county { get; set; }
        public Nullable<int> sub_county { get; set; }
        public Nullable<int> month { get; set; }
        public Nullable<int> season { get; set; }
        public Nullable<int> core_crop { get; set; }
        public Nullable<double> hectares_planted { get; set; }
        public string planting_fertilizer_1 { get; set; }
        public Nullable<double> estimated_ha_applied_fertilizer_1 { get; set; }
        public Nullable<double> fertilizer_use_rate_1 { get; set; }
        public Nullable<double> total_fertilizer_used_1 { get; set; }
        public string planting_fertilizer_2 { get; set; }
        public Nullable<double> estimated_ha_applied_fertilizer_2 { get; set; }
        public Nullable<double> fertilizer_use_rate_2 { get; set; }
        public Nullable<double> total_fertilizer_used_2 { get; set; }
    
        public virtual county county1 { get; set; }
        public virtual season season1 { get; set; }
        public virtual subcounty subcounty { get; set; }
    }
}
