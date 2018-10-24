using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADIMS.ViewModels
{
    public class CreateQueueViewModel
    {
        public int county { get; set; }
        public int sub_county { get; set; }
        public int ward { get; set; }
        public int uai { get; set; }
        public int season { get; set; }
        public string name { get; set; }
        public int year { get; set; }
    }

    public class EditQueueViewModel
    {
        public int id { get; set; }
        public int county { get; set; }
        public int sub_county { get; set; }
        public int ward { get; set; }
        public int uai { get; set; }
        public int season { get; set; }
        public string name { get; set; }
        public int year { get; set; }
    }
}