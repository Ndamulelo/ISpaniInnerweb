using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IWorkExperienceService
    {
        void Create(JobSeekerExperienceViewModel jobSeekerExperienceViewModel);
        void Update(JobSeekerExperienceViewModel jobSeekerExperienceViewModel);
        void Delete(string id, string seekerId);
        bool IsJobSeekerEmployed(string jobSeekerId);
    }
}
