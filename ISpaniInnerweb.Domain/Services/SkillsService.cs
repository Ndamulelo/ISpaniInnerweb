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
    public class SkillsService : ISkillsService
    {
        IRepository<Skill> skillRepository; 
        IRepository<JobSeekerSkills> jobSeekerSkillsRepository; 

        public SkillsService(IRepository<Skill> skillRepository, IRepository<JobSeekerSkills> jobSeekerSkills)
        {
            this.skillRepository = skillRepository;
            this.jobSeekerSkillsRepository = jobSeekerSkills;
        }

        public IList<Skill> GetAll()
        {
            return skillRepository.Get();
        }
        public void Create(JobSeekerSkillsViewModel jobSeekerSkillsViewModel)
        {
            jobSeekerSkillsRepository.Insert(new JobSeekerSkills { 
                
                Id = Guid.NewGuid().ToString(),
                SkillLevelId = jobSeekerSkillsViewModel.SkillLevelId,
                SkillId = jobSeekerSkillsViewModel.SkillId,
                JobSeekerId = jobSeekerSkillsViewModel.JobSeekerId,
                IsActive = true
            });
        }

        public void Edit(JobSeekerSkillsViewModel jobSeekerSkillsViewModel)
        {
            var skillToUpdate = jobSeekerSkillsRepository.FindByConditionAsNoTracking
                (c => c.JobSeekerId.Equals(jobSeekerSkillsViewModel.JobSeekerId) && 
                c.SkillId.Equals(jobSeekerSkillsViewModel.SkillId)).FirstOrDefault();

            skillToUpdate.SkillLevelId = jobSeekerSkillsViewModel.SkillLevelId;

            jobSeekerSkillsRepository.Update(skillToUpdate);
            
        }

        public void Delete(string id, string seekerId)
        {
            var skillToDelete = jobSeekerSkillsRepository.
                FindByConditionAsNoTracking(s => s.SkillId.Equals(id) && s.JobSeekerId.Equals(seekerId)).FirstOrDefault();

            jobSeekerSkillsRepository.Delete(skillToDelete.Id);
        }
    }
}
