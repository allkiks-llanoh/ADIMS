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
    
    public partial class farmer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public farmer()
        {
            this.ayii_policy = new HashSet<ayii_policy>();
            this.farms = new HashSet<farm>();
            this.farm_policy_premium = new HashSet<farm_policy_premium>();
            this.farmer_insurance = new HashSet<farmer_insurance>();
            this.farmer_photo = new HashSet<farmer_photo>();
        }
    
        public int id { get; set; }
        public string adimsid { get; set; }
        public Nullable<int> idnumber { get; set; }
        public string passportno { get; set; }
        public string krapin { get; set; }
        public Nullable<int> phoneno { get; set; }
        public string entity_type { get; set; }
        public string name { get; set; }
        public string group_contact_name { get; set; }
        public string group_contact_id { get; set; }
        public string group_first_member { get; set; }
        public string group_second_member { get; set; }
        public string group_third_member { get; set; }
        public string company_incorporation_number { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public Nullable<int> photo { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public string maritalstatus { get; set; }
        public string nextofkinname { get; set; }
        public string nexofkintype { get; set; }
        public Nullable<int> county { get; set; }
        public Nullable<int> ward { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<int> createdby { get; set; }
        public string educationLevel { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ayii_policy> ayii_policy { get; set; }
        public virtual county county1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<farm> farms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<farm_policy_premium> farm_policy_premium { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<farmer_insurance> farmer_insurance { get; set; }
        public virtual ward ward1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<farmer_photo> farmer_photo { get; set; }
    }
}
