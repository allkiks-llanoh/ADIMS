using System;

namespace ADIMS.ViewModels
{
    public class AddCropAcreageViewModel
    {
        public int? farmid { get; set; }
        public int? cropid { get; set; }
        public int? variety { get; set; }
        public double? acreage { get; set; }
        public int season { get; set; }
        public int year { get; set; }

        //insurance things
        public bool subscribeAyii { get; set; }
        public int uai { get; set; }

        public bool addToQueue { get; set; }
    }
}