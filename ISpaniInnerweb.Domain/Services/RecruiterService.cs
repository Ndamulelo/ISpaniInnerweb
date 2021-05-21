using System;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using ISpaniInnerweb.Domain.Models.RecruiterViewModel;
using ISpaniInnerweb.Domain.Models.JobAdvertViewModel;

namespace ISpaniInnerweb.Domain.Services
{
    public class RecruiterService : IRecruiterService
    {
        IRepository<Recruiter> recruiterRepository;
        IRepository<JobSeeker> jobSeekerRepository;
        IRepository<JobAdvert> jobAdvertRepository;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        IRepository<JobSeekerJobApplications> jobSeekerJobApplications;

        ILogger<RecruiterService> logger;

        public RecruiterService(ILogger<RecruiterService> logger, IRepository<User> userRepository, IRepository<Recruiter> recruiterRepository,
            IRepository<JobSeeker> jobSeekerRepository, IUserService userService, ICompanyService companyService, IRepository<JobSeekerJobApplications> jobSeekerJobApplications,
            IRepository<JobAdvert> jobAdvertRepository)
        {
            this.recruiterRepository = recruiterRepository;
            this.jobAdvertRepository = jobAdvertRepository;
            this.jobSeekerJobApplications = jobSeekerJobApplications;
            this.jobSeekerRepository = jobSeekerRepository;
            _userService = userService;
            _companyService = companyService;
            this.logger = logger;
        }
        public bool Create(CreateRecruiterViewModel createRecruiterViewModel)
        {
            var recruiterId = Guid.NewGuid().ToString();
            
            //Create User first

            if (_userService.IsUserRegistered(new User
            {
                Password = createRecruiterViewModel.Password,
                Email = createRecruiterViewModel.Email,
                UserId = recruiterId,
                RoleName = "Recruiter"
            }))
            {
                recruiterRepository.Insert(new Recruiter
                {
                    Id = recruiterId,
                    LastName = createRecruiterViewModel.LastName,
                    FirstName = createRecruiterViewModel.FirstName,
                    Phone = createRecruiterViewModel.Phone,
                    CompanyId = createRecruiterViewModel.CompanyId,
                    IsActive = true
                });

                return true;
            }

            return false;
        }
        //Add IsAssigned to job so that we unassociate it with a recruiter so that we can assign it to another recruiter
        public bool Delete(string id)
        {
            try
            {
                var recruiterJobs = jobAdvertRepository.FindByConditionAsNoTracking(r => r.RecruiterId.Equals(id)).ToList();

                if(recruiterJobs != null)
                {
                    foreach (var item in recruiterJobs)
                    {
                        jobAdvertRepository.Delete(item.Id);
                    }
                }
                //Delete all the links between 
                recruiterRepository.Delete(id);

                logger.LogInformation("Recruiter " + id + "deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return true;
        }
        //Admin
        public Recruiter Get(string id)
        {
            var recruiter = recruiterRepository.Get(id);
            return recruiter;
        }
        //JobAdvertCreate
        public IList<Recruiter> Get()
        {
            var recruiters = recruiterRepository.Get();
            return recruiters;
        }

        //Admin
        public IList<ViewRecruiterViewModel> GetAll(string companyId = null)
        {
            //var recruiter = recruiterRepository.Get();
            var recruiterListResult = new List<ViewRecruiterViewModel>();

            if (String.IsNullOrEmpty(companyId))
            {
                var recruiterList = recruiterRepository.Get();

                foreach (var item in recruiterList)
                {
                    recruiterListResult.Add(new ViewRecruiterViewModel
                    {
                        FullName = item.FirstName + " " + item.LastName,
                        Email = _userService.GetByUserId(item.Id).Email,
                        //Get company id from recruiter then get company then get company name
                        CompanyName = _companyService.Get(item.CompanyId).CompanyName,
                        RecruiterId = item.Id

                    });
                }

            }
            else
            {
                var recruiters = recruiterRepository.FindByCondition(c => c.IsActive == true && c.CompanyId.Equals(companyId));
                var company = _companyService.Get(companyId);

                foreach (var item in recruiters)
                {
                    recruiterListResult.Add(new ViewRecruiterViewModel
                    {
                        FullName = item.FirstName + " " + item.LastName,
                        Email = _userService.GetByUserId(item.Id).Email,
                        CompanyName = _companyService.Get(companyId).CompanyName,
                        RecruiterId = item.Id

                    });
                }
            }
            return recruiterListResult;
        }


        //Admin
        public IQueryable<Recruiter> GetByCompanyId(string companyId)
        {
            return recruiterRepository.FindByCondition(x => x.CompanyId.Equals(companyId));
        }

        public void Update(EditRecruiterViewModel editRecruiterViewModel)
        {
            var recruiter = recruiterRepository.Get(editRecruiterViewModel.RecruiterId);
            recruiter.Id = editRecruiterViewModel.RecruiterId;
            recruiter.Phone = editRecruiterViewModel.Phone;
            recruiter.FirstName = editRecruiterViewModel.FirstName;
            recruiter.LastName = editRecruiterViewModel.LastName;
            recruiter.CompanyId = editRecruiterViewModel.CompanyId;

            recruiterRepository.Update(recruiter);
        }

        public IList<RecruiterAppliedJobs> GetAppliedJobs(string id)
        {
            var appliedJob = jobSeekerJobApplications.FindByCondition(c => c.RecruiterId.Equals(id)).ToList();
            var recruiterAppliedJobs = new List<RecruiterAppliedJobs>();

            if (appliedJob != null)
            {
                foreach (var item in appliedJob)
                {
                    var applicant = jobSeekerRepository.Get(item.JobSeekerId);
                    var job = jobAdvertRepository.Get(item.JobAdvertId);
                    
                    recruiterAppliedJobs.Add(new RecruiterAppliedJobs {
                        DateCreated = (DateTime)item.CreatedDate,
                        ApplicantName = applicant.FirstName + " " + applicant.LastName,
                        ApplicantPhone = applicant.Phone,
                        JobSeekerId = item.JobSeekerId,
                        JobAdvertId = item.JobAdvertId,
                        JobCaption = job.Caption,
                        ApplicationStatus = item.Status,
                        Email = _userService.GetByUserId(item.JobSeekerId).Email
                    });

                }
                return recruiterAppliedJobs;
            }
            return null;
        }

        public RecruiterProfileViewModel GetProfileData(string id)
        {
            var recruiter = recruiterRepository.Get(id);

            var recruiterProfileData = new
                RecruiterProfileViewModel {
                CompanyId = recruiter.CompanyId,
                FisrtName = recruiter.FirstName,
                LastName = recruiter.LastName,
                Phone = recruiter.Phone,
                CompanyName = _companyService.Get(recruiter.CompanyId).CompanyName
            };

            return recruiterProfileData;
        }

    }
}