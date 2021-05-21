using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models.AdminViewModel;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using ISpaniInnerweb.Domain.Models.RecruiterViewModel;
using System.Collections.Generic;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IJobSeekerService
    {
        JobSeeker Get(string id);
        IList<JobSeekerRecruiterSearch> GetJobSeekerBySkillAndCity(string skillId = null, string cityId = null);
        bool IsJobSeekerSkillExisting(string skillId, string jobSeekerId);
        bool IsJobSeekerLanguageExisting(string languageId, string jobSeekerId);
        RecruiterDashboardStats RecruiterDashboardStats(string recruiterId);
        AdminDashboardStats AdminDashboardStats();
        JobSeekerPersonalDetailsViewModel GetPersonalDetails(string id);
        IList<JobSeekerSkillsViewModel> GetJobSeekerSkills(string id);
        IList<JobSeekerLanguagesViewModel> GetJobSeekerLanguages(string id);
        IList<JobSeekerEducationViewModel> GetJobSeekerEducation(string id);
        IList<JobSeekerExperienceViewModel> GetJobSeekerWorkExperience(string id);
        IList<JobSeekerApplicationHistory> GetJobSeekerApplicationHistory(string id);
        void UpdatePersonalDetails(JobSeekerPersonalDetailsViewModel jobSeekerPersonalDetailsViewModel);

        //Breaking best practice, Single responsibility principle. Sorry.
        IList<Gender> GetGender();
        IList<Title>GetTitle();
        IList<SkillLevel> GetSkillLevel();
        IList<LanguageLevel> GetLanguageLevel();
        IList<Language> GetLanguages();
        IList<Ethnicity> GetEthnicity();
        IList<Qualification> GetQualification();

    }
}
