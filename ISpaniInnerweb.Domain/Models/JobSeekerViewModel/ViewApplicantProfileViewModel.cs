using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class ViewApplicantProfileViewModel
    {
        public JobSeekerPersonalDetailsViewModel JobSeekerPersonalDetailsViewModel { set; get; }
        public IList<JobSeekerExperienceViewModel> JobSeekerExperienceViewModel { set; get; }
        public IList<JobSeekerEducationViewModel> JobSeekerEducationViewModel { set; get; }
        public IList<JobSeekerSkillsViewModel> JobSeekerSkillsViewModel { set; get; }
        public IList<JobSeekerLanguagesViewModel> JobSeekerLanguagesViewModel { set; get; }
        public IList<LanguageLevel> LanguageLevel { set; get; }
        public IList<SkillLevel> SkillLevel { set; get; }
        public IList<JobSeekerAttachmentsViewModel> JobSeekerAttachmentsViewModel { set; get; }
        public IList<Gender> Genders { set; get; }
        public IList<Ethnicity> Ethnicities { set; get; }
        public string JobAdvertId { set; get; }
        public IList<City> Cities
        {
            set; get;
        }

    }
}
