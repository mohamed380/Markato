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
    
    public partial class Request
    {
        public int id { get; set; }
        public int projectID { get; set; }
        public int employeeID { get; set; }
        public int sr { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Project Project { get; set; }
    }
}
