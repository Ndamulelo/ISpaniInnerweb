using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("education")]
    //Change graduation year to int
    public class Education : HustlersEntity
    {
        public string JobSeekerId { set; get; }
        public int GraduationYear { set; get; }
        public string Institution { set; get; }
        public string FieldOfStudy { set; get; }
        public string QualificationeId { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
    }
}
