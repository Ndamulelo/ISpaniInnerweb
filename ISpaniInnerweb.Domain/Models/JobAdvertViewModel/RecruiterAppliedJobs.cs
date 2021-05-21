using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobAdvertViewModel
{
    public class RecruiterAppliedJobs
    {
        public DateTime DateCreated { set; get; }
        public string ApplicantName { set; get; }
        public string ApplicantPhone { set; get; }
        public string JobCaption { set; get; }
        public string JobSeekerId { set; get; }
        public string JobAdvertId { set; get; }
        public string ApplicationStatus { set; get; }
        public string Email { set; get; }
    }
}
