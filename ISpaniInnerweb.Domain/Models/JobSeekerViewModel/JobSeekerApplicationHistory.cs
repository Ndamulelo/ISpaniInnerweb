using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerApplicationHistory
    {
        public DateTime CreatedDate { set; get; }
        public string JobTtitle { set; get; }
        public string Company { set; get; }
        public string Status { set; get; }
    }
}
