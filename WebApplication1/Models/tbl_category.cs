//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_category
    {
        public tbl_category()
        {
            this.tbl_product = new HashSet<tbl_product>();
        }
    
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public string cat_image { get; set; }
        public int cat_status { get; set; }
        public Nullable<int> cat_fk_ad { get; set; }
    
        public virtual tbl_admin tbl_admin { get; set; }
        public virtual tbl_admin tbl_admin1 { get; set; }
        public virtual ICollection<tbl_product> tbl_product { get; set; }
    }
}
