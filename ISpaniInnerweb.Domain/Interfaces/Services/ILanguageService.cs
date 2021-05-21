using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models.EducationViewModel;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface ILanguageService
    {
        void Edit(JobSeekerLanguagesViewModel jobSeekerLanguagesViewModel);
        void Create(JobSeekerLanguagesViewModel jobSeekerLanguagesViewModel );
        void Delete(string id,string seekerId);
        IList<Language> GetAll();
    }
}
