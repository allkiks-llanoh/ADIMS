using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class AddPolicyPremiumViewModel
    {
        public Nullable<int> farm_policy_id { get; set; }
        public Nullable<int> farmer { get; set; }
        public Nullable<double> amount { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
    }
}