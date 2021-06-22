using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models
{
    public class ViewInterviewViewModel
    {
        public DateTime? InterviewDate { set; get; }
        public string Time { set; get; }
        public string InterviewType { set; get; } // Virtial or Physical
        public string InterviewLink { set; get; } // For Virtual Interviews Only
        public string Applicant { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }
        public string JobAdvertId { set; get; }
        public string JobSeekerId { set; get; }
        public string Company { set; get; }
        public string Address { set; get; }
        public string Job { set; get; } // Android Developer
        public string Interviewer { set; get; } // Roshudufhadzwa
    }
}
