//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ADIMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class cce_queue_farms
    {
        public int id { get; set; }
        public Nullable<int> queue { get; set; }
        public Nullable<int> cropacreage { get; set; }
        public string preliminary_status { get; set; }
        public Nullable<int> preliminary_cce { get; set; }
        public string actual_status { get; set; }
        public Nullable<int> actual_cce { get; set; }
        public Nullable<System.DateTime> datecreated { get; set; }
        public Nullable<int> createdby { get; set; }
    
        public virtual actual_cropcutting_exercise actual_cropcutting_exercise { get; set; }
        public virtual crop_cutting_queue crop_cutting_queue { get; set; }
        public virtual cropacreage cropacreage1 { get; set; }
        public virtual preliminary_cropcutting_exercise preliminary_cropcutting_exercise { get; set; }
    }
}
