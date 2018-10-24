using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class AddFarmPolicyViewModel
    {
        public Nullable<int> farmid { get; set; }
        public Nullable<int> insurance_companyid { get; set; }
        public string uai_name { get; set; }
        public Nullable<int> cropinsured { get; set; }
        public Nullable<double> preferred_coverage { get; set; }
        public Nullable<System.DateTime> dateofpolicysale { get; set; }
        public Nullable<double> average_yield { get; set; }
        public Nullable<double> insured_yield { get; set; }
        public Nullable<double> sum_insured { get; set; }
        public Nullable<double> area_insured { get; set; }
        public Nullable<double> premium_rate { get; set; }
        public Nullable<double> subsidy_amount { get; set; }
        public Nullable<double> total_insured { get; set; }
        public Nullable<double> total_premium { get; set; }
        public Nullable<double> final_premium { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
    }
}