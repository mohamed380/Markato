//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Markato.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FeedBack
    {
        public Nullable<int> MTID { get; set; }
        public int Rate { get; set; }
        public string FeedBack1 { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public int id { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Project Project { get; set; }
    }
}
