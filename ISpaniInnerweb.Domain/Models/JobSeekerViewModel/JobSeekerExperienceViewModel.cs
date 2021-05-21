using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerExperienceViewModel
    {
        public string JobSeekerId { set; get; }
        [Required]
        [StringLength(50)]
        public string JobTitle { set; get; }
        [Required]
        [StringLength(50)]
        public string CompanyName { set; get; }
        public string JobCategory { set; get; }
        public string WorkExperienceId { set; get; }
        public bool IsCurrentCompany { set; get; }
        public string JobCategoryId { set; get; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { set; get; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { set; get; }
    }
}
