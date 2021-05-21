using ISpaniInnerweb.Domain.Models.EducationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IEducationService
    {
        void Edit(JobSeekerEducationViewModel jobSeekerEducationViewModel);
        void Create(JobSeekerEducationViewModel jobSeekerEducationViewModel);
        void Delete(string id, string seekerId);
    }
}
