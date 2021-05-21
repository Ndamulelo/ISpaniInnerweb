using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerAddExperienceViewModel
    {
        public IList<JobCategory> JobCategories { set; get; }
        public JobSeekerExperienceViewModel JobSeekerExperienceViewModel { set; get; }
        
    }
}
