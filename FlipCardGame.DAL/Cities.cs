//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XSS_Victim.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cities
    {
        public Cities()
        {
            this.Users = new HashSet<Users>();
        }
    
        public long CountryID { get; set; }
        public long ProvinceID { get; set; }
        public long IDCity { get; set; }
        public string CityName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    
        public virtual Provinces Provinces { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
