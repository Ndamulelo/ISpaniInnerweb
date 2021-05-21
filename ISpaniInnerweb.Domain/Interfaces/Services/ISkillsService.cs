using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models.EducationViewModel;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface ISkillsService
    {

        void Create(JobSeekerSkillsViewModel jobSeekerSkillsViewModel);
        void Edit(JobSeekerSkillsViewModel jobSeekerSkillsViewModel);
        void Delete(string id, string seekerId);
        //void Create(JobSeekerEducationViewModel jobSeekerEducationViewModel);
        IList<Skill> GetAll();
    }
}
