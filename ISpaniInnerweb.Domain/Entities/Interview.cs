using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISpaniInnerweb.Domain.Entities
{
    public class Interview : HustlersEntity
    {
        public DateTime? InterviewDate { set; get; }
        public DateTime? DateCreated { set; get; }
        public string Time { set; get; }
        public string InterviewType { set; get; } // Virtial or Physical
        [NotMapped]
        public bool IsRead { set; get; } // 1 or 0
        public string InterviewLink { set; get; } // For Virtual Interviews Only
        public string JobAdvertId { set; get; }
        public string JobSeekerId { set; get; }
        public string RecruiterId { set; get; }
        public string CompanyId { set; get; }
        public string Job { set; get; } // Android Developer
        public string Company { set; get; } // Unique Geeks Technology
        public string Interviewer { set; get; } // Roshudufhadzwa
        
    }
}
