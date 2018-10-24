namespace ADIMS.ViewModels
{
    public class PreliminaryCropCuttingViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public int ward { get; set; }
        public string ward_code { get; set; }
        public string enumeration_area_code { get; set; }
        public string village_name { get; set; }

        public string respondent_name { get; set; }
        public string respondent_telephone { get; set; }
        public string respondent_relation { get; set; }

        public int? crop { get; set; }
        public string variety { get; set; }

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
        public decimal total_maize_plants { get; set; }
        public decimal total_cobs_count { get; set; }
        public string photo_of_crop { get; set; }
        public string agreed_date_cutting { get; set; }

        public int queue { get; set; }
        public int cropacerage { get; set; }
        public string progresskey { get; set; }
    }


}