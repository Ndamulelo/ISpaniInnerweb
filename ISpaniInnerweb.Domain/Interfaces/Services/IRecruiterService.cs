using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models.JobAdvertViewModel;
using ISpaniInnerweb.Domain.Models.RecruiterViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IRecruiterService
    {
        bool Create(CreateRecruiterViewModel createRecruiterViewModel);
        void Update(EditRecruiterViewModel editRecruiterViewModel);
        RecruiterProfileViewModel GetProfileData(string id);
        Recruiter Get(string id);
        IList<Recruiter> Get();
        //void DeleteByCompany(string id);
        IList<RecruiterAppliedJobs> GetAppliedJobs(string id);
        IList<ViewRecruiterViewModel> GetAll(string id);
        IQueryable<Recruiter> GetByCompanyId(string companyId);
        bool Delete(string id);
    }
}
