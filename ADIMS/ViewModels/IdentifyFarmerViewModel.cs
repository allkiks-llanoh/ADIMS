using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class IdentifyFarmerViewModel
    {
        public IdentificationOption identificationoption { get; set; }
        public int id_number { get; set; }
        public string adims_id { get; set; }
        public string phone_number { get; set; }
    }

    public enum IdentificationOption
    {
        ID_Number = 1,
        ADIMS_ID = 2,
        PHONE_NUMBER = 3
    }
}