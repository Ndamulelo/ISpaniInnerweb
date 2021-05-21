using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.SkillsViewModel
{
    public class AddSkillViewModel
    {
        public IList<Skill> Skills { set; get; }
        public IList<SkillLevel> SkillLevels { set; get; }
        public IList<JobSeekerSkillsViewModel> JobSeekerSkills { set; get; }
    }
}
