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
    
    public partial class ward
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ward()
        {
            this.actual_cropcutting_exercise = new HashSet<actual_cropcutting_exercise>();
            this.crop_cutting_queue = new HashSet<crop_cutting_queue>();
            this.farms = new HashSet<farm>();
            this.farmers = new HashSet<farmer>();
            this.preliminary_cropcutting_exercise = new HashSet<preliminary_cropcutting_exercise>();
            this.uais = new HashSet<uai>();
            this.uai_yield = new HashSet<uai_yield>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public Nullable<int> subcounty { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<int> createdby { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<actual_cropcutting_exercise> actual_cropcutting_exercise { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<crop_cutting_queue> crop_cutting_queue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<farm> farms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<farmer> farmers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<preliminary_cropcutting_exercise> preliminary_cropcutting_exercise { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uai> uais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uai_yield> uai_yield { get; set; }
    }
}
