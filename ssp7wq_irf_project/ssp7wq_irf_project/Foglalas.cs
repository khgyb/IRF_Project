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
    
    public partial class Foglalas
    {
        public int Id_Foglalas { get; set; }
        public int Musor_Id { get; set; }
        public Nullable<int> Szek_Id { get; set; }
        public bool Foglalt { get; set; }
    
        public virtual Musor Musor { get; set; }
        public virtual Terem Terem { get; set; }
    }
}