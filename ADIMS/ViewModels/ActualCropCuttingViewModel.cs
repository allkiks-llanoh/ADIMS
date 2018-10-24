using System.Web;

namespace ADIMS.ViewModels
{
    public class ActualCropCuttingViewModel
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

        public string date_of_crop_cutting { get; set; }
        public string time_of_cropcutting_start { get; set; }
        public string time_of_cropcutting_end { get; set; }

        public int total_count_maizeplants_inSS { get; set; }
        public int total_count_cobs_inSS { get; set; }
        public int total_count_plants_stage1 { get; set; }
        public int total_count_plants_stage2 { get; set; }
        public int total_cobs_stage1 { get; set; }
        public int total_cobs_stage2 { get; set; }
        public string photo_ofSS_before_harvesting { get; set; }

        public int cobs_harvested { get; set; }
        public decimal quantity_harvested_wetgrain { get; set; }
        public decimal quantity_harvested_kgpercob { get; set; }
        public string final_quantity_wetgrain { get; set; }
        public decimal moisture_reading { get; set; }
        public decimal average_moisture_content { get; set; }
        public string quantity_wet_sample { get; set; }

        public string video_taken_CCE { get; set; }
        public string drying_method { get; set; }
        public decimal weight_of_wetsample { get; set; }
        public decimal weight_of_driedsample { get; set; }
        public decimal total_weight { get; set; }
        public decimal moisture_percentagedried { get; set; }

        public int queue { get; set; }
        public int cropacerage { get; set; }
        public string progresskey { get; set; }
        public HttpPostedFileBase file { get; set; }
    }
}