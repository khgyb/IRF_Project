//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ssp7wq_irf_project
{
    using System;
    using System.Collections.Generic;
    
    public partial class Terem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Terem()
        {
            this.Foglalas = new HashSet<Foglalas>();
        }
    
        public int Id_Szek { get; set; }
        public Nullable<int> Sorszam { get; set; }
        public int Oszlopszam { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Foglalas> Foglalas { get; set; }
    }
}
