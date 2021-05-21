using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.EducationViewModel
{
    public class JobSeekerEducationViewModel
    {
        public string JobSeekerId { set; get; }
        [Required]
        [StringLength(50)]
        public string Institution { set; get; }
        [Required]
        [StringLength(50)]
        public string FieldOfStudy { set; get; }
        //[]
        public int GraduationYear { set; get; }
        public string EducationId { set; get; }
        public string QualificationId { set; get; }
    }
}
