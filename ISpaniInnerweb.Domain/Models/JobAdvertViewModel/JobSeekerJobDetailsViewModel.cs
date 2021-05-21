using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobAdvertViewModel
{
    public class JobSeekerJobDetailsViewModel
    {
        public string JobAdvertId { set; get; }
        public string CompanyId { set; get; }
        public string Caption { set; get; }
        public string Introduction { set; get; }
        public IList<string> Qualifications { set; get; }
        public IList<string> Duties { set; get; }
        public IList<string> Experience { set; get; }
        public long SalaryFrom { set; get; }
        public long SalaryTo { set; get; }
        public string Company { set; get; }
        public bool IsAlreadyAppliedBySeeker { set; get; }
        public string Recruiter { set; get; }
        public string RecruiterId { set; get; }
        public string RecruiterEmail { set; get; }
        public string RecruiterPhone { set; get; }
        public string City { set; get; }
        public string ExperienceLevel { set; get; } //Entry, Intermediate, Experienced
        public string JobType { set; get; } //Contract, Permenant , part-time
        public DateTime? CreatedDate { set; get; }
    }
}
