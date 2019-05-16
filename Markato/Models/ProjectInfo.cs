using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Markato.Models
{
    public class ProjectsInfo
    {
        public List<Project> projects { get; set; }
        public List<Team> Teams { get; set; }
        public Employee Emp { get; set; }
        public List<Project> Rprojects { get; set; }
        public List<List<Request>> Prequests { get; set; }
    }
}