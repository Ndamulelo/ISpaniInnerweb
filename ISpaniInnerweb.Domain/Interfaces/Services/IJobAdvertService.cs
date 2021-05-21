using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models;
using ISpaniInnerweb.Domain.Models.JobAdvertViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IJobAdvertService
    {
        void Create(CreateJobAdvertViewModel createJobAdvertViewModel);
        void Update(EditJobAdvertViewModel editJobAdvertViewModel);
        JobAdvert Get(string id);
        void DeleteByCompany(string id);
        IList<ViewJobAdvertViewModel> GetAllByJobSeeker(string jobCategoryId, string jobCityId);
        JobSeekerJobDetailsViewModel GetDetailedJob(string jobId);
        void ApplyJob(string jobAdvertId, string recruiterId, string companyId, string jobSeekerId);
        void DeclineApplication(string jobAdvertId, string jobSeekerId);
        void InviteToInverView(string jobAdvertId, string jobSeekerId);
        bool IsAlreadyApplied(string jobId, string jobSeekerId);
        IList<JobAdvert> GetAll();
        //Filter with JobType, JobCategory and Province
        IQueryable<JobAdvert> GetJobAdvertsByFilter(JobAdvertSearchModel jobAdvertSearchModel);
        IList<ViewJobAdvertViewModel> GetAllByAdminRecruiter(string jobTypeId, string companyId,DateTime dateFrom,DateTime dateTo, string jobCategoryId);
        IList<ViewJobAdvertViewModel> GetAllByRecruiter(string recruiterId,string jobTypeId, string jobCategoryId = null);
        IQueryable<JobAdvert> GetByCompanyId(string companyId);
        IQueryable<JobAdvert> GetByRecruiterId(string recruiterId);
        void Delete(string id);
    }
}
