using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("jobseekerskills")]
    public class JobSeekerSkills : HustlersEntity
    {
        public string JobSeekerId { set; get; }
        public string SkillId { set; get; }
        public string SkillLevelId { set; get; }
    }
}
