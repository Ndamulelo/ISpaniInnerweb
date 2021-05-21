using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.EducationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class EducationService : IEducationService
    {
        IRepository<Education> educationRepository; 

        public EducationService(IRepository<Education> educationRepository)
        {
            this.educationRepository = educationRepository;
        }
        public void Create(JobSeekerEducationViewModel jobSeekerEducationViewModel)
        {
            var education = new Education() 
            {
                GraduationYear = jobSeekerEducationViewModel.GraduationYear,
                Institution = jobSeekerEducationViewModel.Institution,
                QualificationeId = jobSeekerEducationViewModel.QualificationId,
                IsActive = true,
                JobSeekerId = jobSeekerEducationViewModel.JobSeekerId,
                FieldOfStudy = jobSeekerEducationViewModel.FieldOfStudy,
                Id = Guid.NewGuid().ToString()
            };

            educationRepository.Insert(education);
        }

        public void Delete(string id, string seekerId)
        {
            var educationToDelete = educationRepository.
                                FindByConditionAsNoTracking(s => s.Id.Equals(id) && s.JobSeekerId.Equals(seekerId)).
                                FirstOrDefault();

            educationRepository.Delete(educationToDelete.Id);
        }

        public void Edit(JobSeekerEducationViewModel jobSeekerEducationViewModel)
        {
            var education = educationRepository.Get(jobSeekerEducationViewModel.EducationId);

            education.GraduationYear = jobSeekerEducationViewModel.GraduationYear;
            education.Institution = jobSeekerEducationViewModel.Institution;
            education.FieldOfStudy = jobSeekerEducationViewModel.FieldOfStudy;
            education.QualificationeId = jobSeekerEducationViewModel.QualificationId;

            educationRepository.Update(education);
        }
    }
}
