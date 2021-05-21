using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerEducationViewModel
    {
        [Required]
        [StringLength(50)]
        public string Insitution { set; get; }
        [Required]
        [StringLength(50)]
        public string FieldOfStudy { set; get; }
        public int GraduationYear { set; get; }
        public string QualificationId { set; get; }
        public string Qualification { set; get; }
        public string EducationId { set; get; }
        public string JobSeekerId { set; get; }

    }
}
