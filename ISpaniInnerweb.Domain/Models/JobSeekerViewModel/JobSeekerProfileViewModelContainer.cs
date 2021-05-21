using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerProfileViewModelContainer
    {
        public JobSeekerPersonalDetailsViewModel JobSeekerPersonalDetailsViewModel { set; get; }
        public IList<JobSeekerExperienceViewModel> JobSeekerExperienceViewModel { set; get; }
        public IList<JobSeekerEducationViewModel> JobSeekerEducationViewModel { set; get; }
        public IList<JobSeekerSkillsViewModel> JobSeekerSkillsViewModel { set; get; }
        public IList<JobSeekerLanguagesViewModel> JobSeekerLanguagesViewModel { set; get; }
        public IList<JobSeekerAttachmentsViewModel> JobSeekerAttachmentsViewModel { set; get; }
        public IList<Gender> Genders { set; get; }
        public IList<Title> Titles { set; get; }
        public IList<Ethnicity> Ethnicities { set; get; }
        public IList<City> Cities { set; get; }
        public IList<Province> Provinces { set; get; }
        public IList<JobCategory> JobCategories { set; get; }
        public IList<Qualification> Qualifications { set; get; }
        public IList<SkillLevel> SkillLevels { set; get; }
        public IList<LanguageLevel> LanguageLevels { set; get; }
        public IList<Language> Languages { set; get; }
        public IList<Attachment> Attachments { set; get; }
        public JobSeekerCVCreateViewModel JobSeekerCVCreateViewModel { set; get; }
        public JobSeekerCreateIdViewModel JobSeekerCreateIdViewModel { set; get; }
        public JobSeekerCreateAcademicRecordViewModel JobSeekerCreateAcademicRecordViewModel { set; get; }

    }
}
