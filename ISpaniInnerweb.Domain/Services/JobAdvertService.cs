using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Helpers;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models;
using ISpaniInnerweb.Domain.Models.JobAdvertViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class JobAdvertService : IJobAdvertService
    {
        IRepository<User> userRepository;
        IRepository<Recruiter> recruiterRepository;
        IRepository<JobSeeker> jobSeekerRepository;
        IRepository<JobSeekerJobApplications> jobSeekerJobApplicationsRepository;
        IRepository<Attachment> attachmentRepository;
        IRepository<JobAdvert> jobAdvertRepository;
        IRepository<JobSeekerLanguages> jobSeekerLanguageRepository;
        IRepository<Education> educationRepository;
        IRepository<JobSeekerSkills> jobSeekerSkillsRepository;
        IStringManipulator _stringManipulator;
        ILogger<JobAdvertService> logger;

        private readonly ICompanyService _companyService;
        private readonly IExperienceLevelService _experienceLevelService;
        private readonly IProvinceService _provinceService;
        private readonly IJobTypeService _jobTypeService;
        private readonly IRecruiterService _recruiterService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ICityService _cityService;

        public JobAdvertService(ILogger<JobAdvertService> logger, IRepository<User> userRepository, IRepository<Recruiter> recruiterRepository,
            IRepository<JobSeeker> jobSeekerRepository, IRepository<JobAdvert> jobAdvertRepository, IStringManipulator stringManipulator,
            ICompanyService companyService, IExperienceLevelService experienceLevelService, IProvinceService provinceService, IRecruiterService recruiterService, 
            IJobCategoryService jobCategoryService, IJobTypeService jobTypeService, ICityService cityService, IRepository<JobSeekerJobApplications> jobSeekerJobApplicationsRepository
            , IRepository<Attachment> attachmentRepository, IRepository<Education> educationRepository, IRepository<JobSeekerLanguages> jobSeekerLanguageRepository,
            IRepository<JobSeekerSkills> jobSeekerSkillsRepository)
        {
            this.attachmentRepository = attachmentRepository;
            this.recruiterRepository = recruiterRepository;
            this.jobAdvertRepository = jobAdvertRepository;
            this.educationRepository = educationRepository;
            this.jobSeekerSkillsRepository = jobSeekerSkillsRepository;
            this.jobSeekerLanguageRepository = jobSeekerLanguageRepository;
            this.userRepository = userRepository;
            _companyService = companyService;
            _provinceService = provinceService;
            _recruiterService = recruiterService;
            _cityService = cityService;
            _experienceLevelService = experienceLevelService;
            _jobCategoryService = jobCategoryService;
            _jobTypeService = jobTypeService;
            _stringManipulator = stringManipulator;
            this.logger = logger;
            this.jobSeekerJobApplicationsRepository = jobSeekerJobApplicationsRepository;
        }
        public void Create(CreateJobAdvertViewModel createJobAdvertViewModel)
        {
            //Breakdown qualification and experience when we disiplay to users
            // var qualificationList = _stringManipulator.BreakDownText(createJobAdvertViewModel.Qualifications);
            //var experience = _stringManipulator.BreakDownText(createJobAdvertViewModel.Experience);

            //var duties = _stringManipulator.BreakDownText(createJobAdvertViewModel.Duties);

            var JobAdvert = new JobAdvert 
            {
                Id = Guid.NewGuid().ToString(), IsActive = true, Introduction = createJobAdvertViewModel.Introduction,
                Caption = createJobAdvertViewModel.Caption,Qualifications = createJobAdvertViewModel.Qualifications, 
                Experience = createJobAdvertViewModel.Experience, JobTypeId = createJobAdvertViewModel.JobTypeId,
                CityId = createJobAdvertViewModel.CityId, RecruiterId = createJobAdvertViewModel.RecruiterId,CompanyId = createJobAdvertViewModel.CompanyId, 
                ExperienceLevelId = createJobAdvertViewModel.ExperienceLevelId, JobCategoryId = createJobAdvertViewModel.JobCategoryId,
                CreatedDate = DateTime.Now, StartDate = createJobAdvertViewModel.StartDate, EndDate = createJobAdvertViewModel.EndDate,
                SalaryFrom = createJobAdvertViewModel.SalaryFrom,SalaryTo = createJobAdvertViewModel.SalaryTo,Duties = createJobAdvertViewModel.Duties
            };
           
            jobAdvertRepository.Insert(JobAdvert);
        }

        public void Delete(string id)
        {
            try
            {
                var advert = jobAdvertRepository.Get(id);
                advert.IsActive = false;
                jobAdvertRepository.Update(advert);
                logger.LogInformation("Job Advert " + advert.Id + "deleted");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        public JobAdvert Get(string id)
        {
            var jobAdvert = jobAdvertRepository.Get(id);
            return jobAdvert;
        }        

        public IList<JobAdvert> GetAll()
        {
            return jobAdvertRepository.Get();
        }

        public IQueryable<JobAdvert> GetByCompanyId(string companyId)
        {
            return jobAdvertRepository.FindByCondition(x => x.CompanyId.Equals(companyId));
        }

        public IQueryable<JobAdvert> GetByRecruiterId(string recruiterId)
        {
            return jobAdvertRepository.FindByCondition(x => x.RecruiterId.Equals(recruiterId) && x.IsActive == true);
        }
        //Jobv Seeker Priority
        public IQueryable<JobAdvert> GetJobAdvertsByFilter(JobAdvertSearchModel jobAdvertSearchModel)
        {
            var JobAdverts = jobAdvertRepository.Get();
            if (jobAdvertSearchModel != null)
            {
                if (!string.IsNullOrEmpty(jobAdvertSearchModel.JobCategoryId))
                    JobAdverts = JobAdverts.Where(x => x.JobCategoryId.Equals(jobAdvertSearchModel.JobCategoryId)).ToList(); 
                if (!string.IsNullOrEmpty(jobAdvertSearchModel.JobTypeId))
                    JobAdverts = JobAdverts.Where(x => x.JobTypeId.Equals(jobAdvertSearchModel.JobTypeId)).ToList();       
                if (!string.IsNullOrEmpty(jobAdvertSearchModel.ProvinceId))
                    JobAdverts = JobAdverts.Where(x => x.ProvinceId.Equals(jobAdvertSearchModel.ProvinceId)).ToList();
            }
            return (IQueryable<JobAdvert>)JobAdverts;
        }        
        
        public bool IsProfileComplete(string seekerId)
        {
            
            // var seeker = jobSeekerRepository.FindByCondition(s => s.Id.Equals(seekerId)).FirstOrDefault();
            var attachments = attachmentRepository.FindByCondition(x => x.JobSeekerId.Equals(seekerId)).ToList();
            var skills = jobSeekerSkillsRepository.FindByCondition(x => x.JobSeekerId.Equals(seekerId)).FirstOrDefault();
            var languages = jobSeekerLanguageRepository.FindByCondition(x => x.JobSeekerId.Equals(seekerId)).FirstOrDefault();
            var education = educationRepository.FindByCondition(x => x.JobSeekerId.Equals(seekerId)).FirstOrDefault();

            // if(String.IsNullOrEmpty(seeker.FirstName) || String.IsNullOrEmpty(seeker.PersonalProfile) || String.IsNullOrEmpty(seeker.AddressId)
            //   || String.IsNullOrEmpty(seeker.Phone))
           // {
                // return false;
            // }

            if (attachments.Count < 3)
            {
                return false;
            }

            if(skills == null)
            {
                return false;
            }            
            
            if(languages == null)
            {
                return false;
            }            
            
            if(education == null)
            {
                return false;
            }

            return true;
        }

        public IList<ViewJobAdvertViewModel> GetAllByAdminRecruiter(DateTime dateFrom, DateTime dateTo, string jobTypeId = "All", string companyId = "All", string jobCategoryId = "All")
        {
            var jobAdverts = jobAdvertRepository.Get();
            // Default Date {0001/01/01 00:00:00}
            //Loop through adverts and assign into ViewJobAdvertViewModel

            // JType | CId | CatId | SDate | EDate
            // All | All | All | All | All

            //First Time
            if (String.IsNullOrEmpty(jobTypeId) && String.IsNullOrEmpty(companyId) && String.IsNullOrEmpty(jobCategoryId))
            {
                jobAdverts = jobAdverts.OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);

            }

            if (jobTypeId.Equals("All") && companyId.Equals("All") & jobCategoryId.Equals("All"))
            {   
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo)
                                            .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);

            }//ALL | S | S
            else if(jobTypeId.Equals("All") && !companyId.Equals("All") && !jobCategoryId.Equals("All"))
            {
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo && x.CompanyId.Equals(companyId)
                                              && x.JobCategoryId.Equals(jobCategoryId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }            
            else if(jobTypeId.Equals("All") && companyId.Equals("All") && !jobCategoryId.Equals("All"))
            {   // ALL | ALL | S
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo
                                              && x.JobCategoryId.Equals(jobCategoryId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }            
            else if(jobTypeId.Equals("All") && !companyId.Equals("All") && jobCategoryId.Equals("All"))
            {   // ALL | S | ALL
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo
                                              && x.CompanyId.Equals(companyId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }           
            else if(!jobTypeId.Equals("All") && !companyId.Equals("All") && jobCategoryId.Equals("All"))
            {   // S | S | ALL
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo
                                              && x.JobTypeId.Equals(jobTypeId) && x.CompanyId.Equals(companyId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }            
            else if(!jobTypeId.Equals("All") && companyId.Equals("All") && jobCategoryId.Equals("All"))
            {   // S | ALL | ALL
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo
                                              && x.CompanyId.Equals(companyId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }            
            else if(jobTypeId.Equals("All") && !companyId.Equals("All") && jobCategoryId.Equals("All"))
            {   // ALL | S | ALL
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo
                                              && x.CompanyId.Equals(companyId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }            
            else if(!jobTypeId.Equals("All") && companyId.Equals("All") && !jobCategoryId.Equals("All"))
            {   // S | A | S
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo
                                              && x.JobTypeId.Equals(jobTypeId) && x.JobCategoryId.Equals(jobCategoryId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }            
            else if(jobTypeId.Equals("All") && companyId.Equals("All") && !jobCategoryId.Equals("All"))
            {   // ALL | S | ALL
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo
                                            && x.JobCategoryId.Equals(jobCategoryId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }            
            else if(!jobTypeId.Equals("All") && companyId.Equals("All") && jobCategoryId.Equals("All"))
            {   // ALL | S | ALL
                jobAdverts = jobAdverts.Where(x => x.CreatedDate >= dateFrom && x.CreatedDate <= dateTo
                                            && x.JobTypeId.Equals(jobTypeId))
                                        .OrderByDescending(x => x.CreatedDate).ToList();
                return ProcessAdvert(jobAdverts);
            }

            return ProcessAdvert(jobAdverts);

        }

        public IList<ViewJobAdvertViewModel> GetAllByRecruiter(string recruiterId,string jobTypeId,string jobCategoryId = null)
        {
            var jobAdverts = jobAdvertRepository.FindByCondition( r => r.RecruiterId.Equals(recruiterId)).ToList();

            //Loop through adverts and assign into ViewJobAdvertViewModel

            if (!String.IsNullOrEmpty(jobCategoryId))
            {
                if (jobTypeId.Equals("All") && jobCategoryId.Equals("All"))
                {
                    return ProcessAdvert(jobAdverts);
                }
                else if(jobTypeId.Equals("All") && !jobCategoryId.Equals("All"))
                {
                    //return  all for type and filter by category
                    jobAdverts = jobAdverts.Where(x => x.JobCategoryId.Equals(jobCategoryId)).ToList();
                    return ProcessAdvert(jobAdverts);
                }
                else if(!jobTypeId.Equals("All") && jobCategoryId.Equals("All"))
                {
                    jobAdverts = jobAdverts.Where(x => x.JobTypeId.Equals(jobTypeId)).ToList();
                    return ProcessAdvert(jobAdverts);
                }
                else if(!jobTypeId.Equals("All") && !jobCategoryId.Equals("All"))
                {
                    jobAdverts = jobAdverts.Where(x => x.JobTypeId.Equals(jobTypeId) && x.JobCategoryId.Equals(jobCategoryId)).ToList();
                    return ProcessAdvert(jobAdverts);
                }

                //Call process method here
                return ProcessAdvert(jobAdverts);
            }
            else
            {
                return ProcessAdvert(jobAdverts);
            }
        }

        public IList<ViewJobAdvertViewModel> GetAllByJobSeeker(string jobCategoryId = null,string jobCityId = null)
        {
            var jobAdverts = jobAdvertRepository.Get();
            //jobAdverts = jobAdverts.Where(x => x.EndDate.Value.Date
            //Loop through adverts and assign into ViewJobAdvertViewModel
            //var dateDiff = (jobAdverts[4].EndDate.Value.Date - DateTime.Now.Date).Days;

            if (!String.IsNullOrEmpty(jobCategoryId))
            {
                if(jobCategoryId.Equals("All") && jobCityId.Equals("All"))
                {
                    jobAdverts = jobAdverts.Where(x =>(x.EndDate.Value.Date - DateTime.Now.Date).Days > -1).OrderBy(x => x.EndDate).ToList();
                    return ProcessAdvert(jobAdverts);
                }
                else if(jobCategoryId.Equals("All") && !jobCityId.Equals("All"))
                {
                    jobAdverts = jobAdverts.Where(x=>
                                x.CityId.Equals(jobCityId) && (x.EndDate.Value.Date - DateTime.Now.Date).Days > -1).OrderBy(x => x.EndDate).ToList();
                    return ProcessAdvert(jobAdverts);
                }
                else if(!jobCategoryId.Equals("All") && jobCityId.Equals("All"))
                {
                    jobAdverts = jobAdverts.Where(
                                    x => x.JobCategoryId.Equals(jobCategoryId) && (x.EndDate.Value.Date - DateTime.Now.Date).Days > -1).OrderBy(x => x.EndDate).ToList();
                    return ProcessAdvert(jobAdverts);
                }
                //For the first time when a page loads
                jobAdverts = jobAdverts.Where(
                            x => x.JobCategoryId.Equals(jobCategoryId) &&
                            x.CityId.Equals(jobCityId) && (x.EndDate.Value.Date - DateTime.Now.Date).Days > -1 
                            ).OrderBy(x => x.EndDate).ToList();

                //Call process method here
                return ProcessAdvert(jobAdverts);
            }
            else
            {
                jobAdverts = jobAdverts.Where(x => (x.EndDate.Value.Date - DateTime.Now.Date).Days > -1).OrderBy(x => x.EndDate).ToList();
                return ProcessAdvert(jobAdverts);
            }
        }
        public JobSeekerJobDetailsViewModel GetDetailedJob(string jobId)
        {
            var advert = jobAdvertRepository.Get(jobId);
            var recruiter = recruiterRepository.Get(advert.RecruiterId);
            var company = _companyService.Get(advert.CompanyId);

            //Check if the applicant has already applied for the job and disable an application button on the UI

            JobSeekerJobDetailsViewModel jobSeekerJobDetailsViewModel = new JobSeekerJobDetailsViewModel
            {
                Qualifications = _stringManipulator.BreakDownText(advert.Qualifications),
                Duties = _stringManipulator.BreakDownText(advert.Duties),
                Introduction = advert.Introduction,
                Caption = advert.Caption,
                Experience = _stringManipulator.BreakDownText(advert.Experience),
                SalaryFrom = advert.SalaryFrom,
                SalaryTo = advert.SalaryTo,
                CreatedDate = advert.CreatedDate,
                JobAdvertId = advert.Id,
                City = _cityService.Get(advert.CityId).Name,
                Company = company.CompanyName,
                Recruiter = recruiter.FirstName + " " + recruiter.LastName,
                ExperienceLevel = _experienceLevelService.Get(advert.ExperienceLevelId).Description,
                JobType = _jobTypeService.Get(advert.JobTypeId).Description,
                RecruiterEmail = userRepository.FindByCondition(u => u.UserId.Equals(recruiter.Id)).FirstOrDefault().Email,
                RecruiterPhone = recruiter.Phone,
                RecruiterId = recruiter.Id,
                CompanyId = company.Id,
                IsAlreadyAppliedBySeeker = false,
                EndDate = advert.EndDate
            };

            return jobSeekerJobDetailsViewModel;
        }

        public bool IsAlreadyApplied(string jobId, string jobSeekerId)
        {
            var seekerJobApplication = jobSeekerJobApplicationsRepository.FindByCondition(s => s.JobAdvertId.Equals(jobId) &&
                s.JobSeekerId.Equals(jobSeekerId)).FirstOrDefault();

            if (seekerJobApplication != null)
            {
                return true;
            }
            return false;
        }
        //Apply for a job
        public void ApplyJob(string jobAdvertId, string recruiterId, string companyId, string jobSeekerId)
        {
            var seekerJobApplication = new JobSeekerJobApplications()
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = companyId,
                RecruiterId = recruiterId,
                JobAdvertId = jobAdvertId,
                JobSeekerId = jobSeekerId,
                Status = "Pending",
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            jobSeekerJobApplicationsRepository.Insert(seekerJobApplication);
        }

        //Reuse for all the search for JobAdverts for all Types of users
        private IList<ViewJobAdvertViewModel> ProcessAdvert(IList<JobAdvert> jobAdvertList)
        {
            var adverts = new List<ViewJobAdvertViewModel>();

            if(jobAdvertList == null)
            { return null; }

            foreach (var item in jobAdvertList)
            {
                adverts.Add(
                            new ViewJobAdvertViewModel
                            {
                                Caption = item.Caption,
                                Introduction = item.Introduction,
                                City = _cityService.Get(item.CityId).Name,
                                CompanyName = _companyService.Get(item.CompanyId).CompanyName,
                                JobType = _jobTypeService.Get(item.JobTypeId).Description,
                                ExperienceLevel = _experienceLevelService.Get(item.ExperienceLevelId).Description,
                                Category = _jobCategoryService.Get(item.JobCategoryId).Name,
                                CreatedDate = (DateTime)item.CreatedDate,
                                EndDate = (DateTime)item.EndDate,
                                JobAdvertId = item.Id
                            });
            }

            return adverts;
        }

        public void Update(EditJobAdvertViewModel editJobAdvertViewModel)
        {
            var advert = jobAdvertRepository.Get(editJobAdvertViewModel.JobAdvertId);

            advert.Caption = editJobAdvertViewModel.Caption;
            advert.CityId = editJobAdvertViewModel.CityId;
            //advert.CompanyId = editJobAdvertViewModel.CompanyId;
            advert.UpdatedDate = DateTime.Now;
            advert.StartDate = editJobAdvertViewModel.StartDate;
            advert.EndDate = editJobAdvertViewModel.EndDate;
            advert.SalaryTo = editJobAdvertViewModel.SalaryTo;
            advert.SalaryFrom = editJobAdvertViewModel.SalaryFrom;
            advert.Qualifications = editJobAdvertViewModel.Qualifications;
            advert.JobTypeId = editJobAdvertViewModel.JobTypeId;
            advert.JobCategoryId = editJobAdvertViewModel.JobCategoryId;
            advert.Duties = editJobAdvertViewModel.Duties;
            advert.ExperienceLevelId = editJobAdvertViewModel.ExperienceLevelId;
            advert.Introduction = editJobAdvertViewModel.Introduction;
            advert.Experience = editJobAdvertViewModel.Experience;

            jobAdvertRepository.Update(advert);
            
        }

        public void DeleteByCompany(string id)
        {
            var jobAdvert = jobAdvertRepository.FindByCondition(x => x.CompanyId.Equals(id));

            foreach (var item in jobAdvert)
            {
                item.IsActive = false;
                jobAdvertRepository.Update(item);
            }

        }

        public void DeclineApplication(string jobAdvertId, string jobSeekerId)
        {
            var seekerJobApplicationToUpdate = jobSeekerJobApplicationsRepository.
            FindByConditionAsNoTracking(j => j.JobSeekerId.Equals(jobSeekerId) && j.JobAdvertId.Equals(jobAdvertId)).FirstOrDefault();
            seekerJobApplicationToUpdate.Status = "Declined";
            jobSeekerJobApplicationsRepository.Update(seekerJobApplicationToUpdate);
        }

        public void InviteToInverView(string jobAdvertId, string jobSeekerId)
        {
            var seekerJobApplicationToUpdate = jobSeekerJobApplicationsRepository.
                FindByConditionAsNoTracking(j => j.JobSeekerId.Equals(jobSeekerId) && j.JobAdvertId.Equals(jobAdvertId)).FirstOrDefault();
            seekerJobApplicationToUpdate.Status = "Interview";
            jobSeekerJobApplicationsRepository.Update(seekerJobApplicationToUpdate);
        }
    }
}
