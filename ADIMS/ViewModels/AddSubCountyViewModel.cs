using System.ComponentModel.DataAnnotations;

namespace ADIMS.ViewModels
{
    public class AddSubCountyViewModel
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string code { get; set; }
        public int? countyid { get; set; }
    }
}