using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobAdvertViewModel
{
    public class ViewJobAdvertViewModel
    {
        public string Caption { set; get; }
        public string JobAdvertId { set; get; }
        public string CompanyName { set; get; }
        public string Category { set; get; }
        public string Introduction { set; get; }
        public string City { set; get; }
        public DateTime CreatedDate { set; get; }
        public string JobType { set; get; }
        public string ExperienceLevel { set; get; }
    }
}
