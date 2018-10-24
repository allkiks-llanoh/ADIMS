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
    
    public partial class preliminary_cropcutting_exercise
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public preliminary_cropcutting_exercise()
        {
            this.cce_queue_farms = new HashSet<cce_queue_farms>();
            this.farm_preliminary_cropcutting_exercise = new HashSet<farm_preliminary_cropcutting_exercise>();
        }
    
        public int id { get; set; }
        public Nullable<int> season { get; set; }
        public Nullable<int> county { get; set; }
        public Nullable<int> sub_county { get; set; }
        public Nullable<int> ward { get; set; }
        public Nullable<int> farm { get; set; }
        public string enumeration_area_code { get; set; }
        public string village_name { get; set; }
        public string respondent_name { get; set; }
        public string respondent_telephone { get; set; }
        public string respondent_relation { get; set; }
        public Nullable<int> crop { get; set; }
        public Nullable<int> variety { get; set; }
        public string month_of_planting { get; set; }
        public string week_of_planting { get; set; }
        public string planting_season { get; set; }
        public string manure_use { get; set; }
        public string inorganic_fert_use { get; set; }
        public string fertilizer_type { get; set; }
        public string fertilizer_quantity { get; set; }
        public string top_fertilizer_type { get; set; }
        public string top_fertilizer_quantity { get; set; }
        public string mode_of_planting { get; set; }
        public string other_crops { get; set; }
        public string planted_holding_type { get; set; }
        public string planted_holding_area { get; set; }
        public string sampling_plot_or_parcel { get; set; }
        public string parcel_geo_ref_no { get; set; }
        public string ss_length_starting_corner { get; set; }
        public string ss_length_paces_random_number { get; set; }
        public string ss_width_starting_corner { get; set; }
        public string ss_width_paces_random_number { get; set; }
        public string sketch_map { get; set; }
        public string crop_stage { get; set; }
        public string overall_crop_condition { get; set; }
        public string crop_status { get; set; }
        public Nullable<decimal> total_maize_plants { get; set; }
        public Nullable<decimal> total_cobs_count { get; set; }
        public string photo_of_crop { get; set; }
        public string agreed_date_cutting { get; set; }
        public Nullable<int> createdby { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public string brief_description { get; set; }
        public string key_observations { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cce_queue_farms> cce_queue_farms { get; set; }
        public virtual county county1 { get; set; }
        public virtual crop crop1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<farm_preliminary_cropcutting_exercise> farm_preliminary_cropcutting_exercise { get; set; }
        public virtual subcounty subcounty { get; set; }
        public virtual ward ward1 { get; set; }
    }
}