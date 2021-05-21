using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.EducationViewModel;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class LanguageService : ILanguageService
    {
        IRepository<JobSeekerLanguages> jobSeekerLangaugeRepository; 
        IRepository<Language> languageRepository; 

        public LanguageService(IRepository<JobSeekerLanguages> jobSeekerLangaugeRepository, IRepository<Language> languageRepository)
        {
            this.jobSeekerLangaugeRepository = jobSeekerLangaugeRepository;
            this.languageRepository = languageRepository;
        }
        public void Create(JobSeekerLanguagesViewModel jobSeekerLanguagesViewModel)
        {
            jobSeekerLangaugeRepository.Insert(new JobSeekerLanguages { 
                Id = Guid.NewGuid().ToString(),
                LanguageId = jobSeekerLanguagesViewModel.LanguageId,
                LanguageLevelId = jobSeekerLanguagesViewModel.LanguageLevelId,
                JobSeekerId = jobSeekerLanguagesViewModel.JobSeekerId,
                IsActive = true
            });
        }

        public void Edit(JobSeekerLanguagesViewModel jobSeekerLanguagesViewModel)
        {
            var seekerLanguageToUpdate = jobSeekerLangaugeRepository.FindByConditionAsNoTracking(c => c.LanguageId.Equals(jobSeekerLanguagesViewModel.LanguageId) &&
            c.JobSeekerId.Equals(jobSeekerLanguagesViewModel.JobSeekerId)).FirstOrDefault();

            seekerLanguageToUpdate.LanguageLevelId = jobSeekerLanguagesViewModel.LanguageLevelId;

            jobSeekerLangaugeRepository.Update(seekerLanguageToUpdate);
        }

        public IList<Language> GetAll()
        {
            return languageRepository.Get();
        }

        public void Delete(string id,string seekerId)
        {
            var LanguageToDelete = jobSeekerLangaugeRepository.
                FindByConditionAsNoTracking(l => l.LanguageId.Equals(id) && l.JobSeekerId.Equals(seekerId)).FirstOrDefault();

            jobSeekerLangaugeRepository.Delete(LanguageToDelete.Id);
        }
    }
}
