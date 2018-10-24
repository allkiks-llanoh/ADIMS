using System;
using System.ComponentModel.DataAnnotations;

namespace ADIMS.ViewModels
{
    public class AddCropViewModel
    {
        [Required]
        public string name { get; set; }
        public string icon { get; set; }
        public string localnames { get; set; }
        public int? maturityperiod { get; set; }
        public int? unitmeasure { get; set; }
        public int? createdby { get; set; }
        public DateTime? createdon { get; set; }
        public int? status { get; set; }
        public int? plantingmethod { get; set; }
    }

    public class EditCropViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string localnames { get; set; }
        public int? maturityperiod { get; set; }
        public int? unitmeasure { get; set; }
        public int? createdby { get; set; }
        public DateTime? createdon { get; set; }
        public int? status { get; set; }
        public int? plantingmethod { get; set; }
    }
}