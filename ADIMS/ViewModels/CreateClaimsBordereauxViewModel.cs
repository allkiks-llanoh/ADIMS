using System.ComponentModel.DataAnnotations;

namespace ADIMS.ViewModels
{
    public class CreateClaimsBordereauxViewModel
    {
        [Required(ErrorMessage = "Insurer is Required")]
        public int insurer { get; set; }

        [Required(ErrorMessage = "County is Required")]
        public int countyid { get; set; }

        [Required(ErrorMessage = "Sub County is Required")]
        public int subcounty { get; set; }

        [Required(ErrorMessage = "Ward is Required")]
        public int wardid { get; set; }

        [Required(ErrorMessage = "Year is Required")]
        public int year { get; set; }

        public int season { get; set; }
        [Required(ErrorMessage = "Crop is Required")]
        public int crop { get; set; }
        [Required(ErrorMessage = "Actual Yield is Required")]
        public float actual_yield { get; set; }
    }
}