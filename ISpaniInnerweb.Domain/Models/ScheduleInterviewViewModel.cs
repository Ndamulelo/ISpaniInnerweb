using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models
{
    public class ScheduleInterviewViewModel
    {
        public Company Company { set; get; }
        public JobAdvert JobAdvert { set; get; }
        public Address Address { set; get; }
        public string RecruiterEmail { set; get; }
        public Recruiter Recruiter { set; get; }
        public JobSeeker JobSeeker { set; get; }
        public string JobSeekerEmail { set; get; }
    }
}
