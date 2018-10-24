using System.ComponentModel.DataAnnotations;

namespace ADIMS.ViewModels
{
    public class AddVarietyViewModel
    {
        [Required]
        public string name { get; set; }
        public int? cropid { get; set; }
    }
}