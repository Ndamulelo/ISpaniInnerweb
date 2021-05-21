using System;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("workexperience")]
    public class WorkExperience : HustlersEntity
    {
        public string JobTitle { set; get; }
        public string JobSeekerId { set; get; }
        public string CompanyName { set; get; }
        public string JobCategory { set; get; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }
        public bool IsCurrentCompany { set; get; }
        public string JobCategoryId { set; get; }

    }
}
