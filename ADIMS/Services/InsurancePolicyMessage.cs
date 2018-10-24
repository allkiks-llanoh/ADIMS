namespace ADIMS.Services
{
    public class InsurancePolicyMessage
    {
        public string farmer { get; set; }
        public string id_number { get; set; }
        public string farm { get; set; }
        public double? average_yield { get; set; }
        public double? insured_yield { get; set; }
        public double? sum_insured { get; set; }
        public double? area_insured { get; set; }
        public double? total_insured { get; set; }
        public double? premium_rate { get; set; }
        public double? total_premium { get; set; }
        public double? subsidy_amount { get; set; }
        public double? final_premium { get; set; }
    }
}