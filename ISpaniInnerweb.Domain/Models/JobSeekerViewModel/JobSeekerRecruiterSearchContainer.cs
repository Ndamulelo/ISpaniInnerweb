using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerRecruiterSearchContainer
    {
        public IList<City> Cities { set; get; } 
        public IList<Skill> Skills { set; get; }
        public IList<JobSeekerRecruiterSearch> JobSeekerRecruiterSearch { set; get; }
    }
}
