namespace ADIMS.ViewModels
{
    public class AddFarmerGroupViewModel
    {
        public string adimsid { get; set; }
        public string name { get; set; }

        public string contact_name { get; set; }
        public string contact_id { get; set; }

        public string date_of_creation { get; set; }
        public int? phoneno { get; set; }

        //the members
        public string firstmember { get; set; }
        public string secondmember { get; set; }
        public string thirdmember { get; set; }

        public int? county { get; set; }
        public int? ward { get; set; }
        public string address { get; set; }
        public int? photo { get; set; }

        //Farmer insurance policies
        public bool hasInsurance { get; set; }
        public string insurance_type { get; set; }
        public string other_insurance { get; set; }
        public string insurer { get; set; }
        public int crop { get; set; }
        public string policy_number { get; set; }

        public string progresskey { get; set; }

    }
}