using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerSkillsViewModel
    {
        public string SkillId { set; get; }
        public string JobSeekerId { set; get; }
        public string SkillLevelId { set; get; }
        public string SkillDescription { set; get; }
        public string SkillLevelDescription { set; get; }
    }
}
