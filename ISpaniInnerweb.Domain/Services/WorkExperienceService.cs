using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class WorkExperienceService : IWorkExperienceService
    {
        IRepository<WorkExperience> workExperienceRepository;

        public WorkExperienceService(IRepository<WorkExperience> workExperienceRepository)
        {
            this.workExperienceRepository = workExperienceRepository;
        }

        public bool IsJobSeekerEmployed(string jobSeekerId)
        {
            var jobSeeker = workExperienceRepository.FindByCondition(c => c.JobSeekerId.Equals(jobSeekerId));
            
            if(jobSeeker != null)
            {
                foreach (var item in jobSeeker)
                {
                    if(item.IsCurrentCompany == true)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Create(JobSeekerExperienceViewModel jobSeekerExperienceViewModel)
        {
            var experienceToCreate = new WorkExperience
            {
                Id = Guid.NewGuid().ToString(),
                CompanyName = jobSeekerExperienceViewModel.CompanyName,
                JobSeekerId = jobSeekerExperienceViewModel.JobSeekerId,
                JobCategoryId = jobSeekerExperienceViewModel.JobCategoryId,
                JobTitle = jobSeekerExperienceViewModel.JobTitle,
                IsActive = true,
                IsCurrentCompany = true
            };

            if(jobSeekerExperienceViewModel.IsCurrentCompany == false)
            {
                experienceToCreate.StartDate = jobSeekerExperienceViewModel.StartDate;
                experienceToCreate.EndDate = jobSeekerExperienceViewModel.EndDate;
                experienceToCreate.IsCurrentCompany = false;
            }
            workExperienceRepository.Insert(experienceToCreate);
        }

        public void Update(JobSeekerExperienceViewModel jobSeekerExperienceViewModel)
        {
            var seekerExperience = workExperienceRepository.Get(jobSeekerExperienceViewModel.WorkExperienceId);

            if (!jobSeekerExperienceViewModel.IsCurrentCompany.Equals("value"))
            {

                seekerExperience.CompanyName = jobSeekerExperienceViewModel.CompanyName;
                seekerExperience.EndDate = jobSeekerExperienceViewModel.EndDate;
                seekerExperience.StartDate = jobSeekerExperienceViewModel.StartDate;
                seekerExperience.JobTitle = jobSeekerExperienceViewModel.JobTitle;
                seekerExperience.JobCategoryId = jobSeekerExperienceViewModel.JobCategoryId;
            }
            else
            {
                seekerExperience.JobTitle = jobSeekerExperienceViewModel.JobTitle;
                seekerExperience.JobCategoryId = jobSeekerExperienceViewModel.JobCategoryId;
                seekerExperience.CompanyName = jobSeekerExperienceViewModel.CompanyName;
            }

            workExperienceRepository.Update(seekerExperience);
        }

        public void Delete(string id, string seekerId)
        {
            var workExperienceToDelete = workExperienceRepository.
                                FindByConditionAsNoTracking(s => s.Id.Equals(id) && s.JobSeekerId.Equals(seekerId)).
                                FirstOrDefault();

            workExperienceRepository.Delete(workExperienceToDelete.Id);
        }
    }
}
