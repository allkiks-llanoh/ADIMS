namespace ADIMS.ViewModels
{
    public class EditFarmViewModel
    {
        public int farmid { get; set; }
        public string name { get; set; }
        public int? farmerid { get; set; }
        public double? area { get; set; }
        public int? countyid { get; set; }
        public int? wardid { get; set; }
        public int? soiltype { get; set; }
        public string lrnumber { get; set; }
        public int? topography { get; set; }
        public int? farmingmethod { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public double? altitude { get; set; }
    }
}