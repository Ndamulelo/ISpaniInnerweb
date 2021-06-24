using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models;
using ISpaniInnerweb.Domain.Models.AdminViewModel;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using ISpaniInnerweb.Domain.Models.RecruiterViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        IRepository<User> userRepository;
        IRepository<Recruiter> recruiterRepository;
        ICompanyService companyService;
        IJobAdvertService jobAdvertService;
        IRepository<JobSeeker> jobSeekerRepository;
        IRepository<City> cityRepository;
        IRepository<Province> provinceRepository;
        IRepository<Ethnicity> ethnicityRepository;
        IRepository<Address> addressRepository;
        IRepository<Title> titleRepository;
        IRepository<Gender> genderRepository;
        IRepository<Skill> skillRepository;
        IRepository<SkillLevel> skillLevelRepository;
        IRepository<JobSeekerSkills> jobSeekerSkillsRepository;
        IRepository<Language> languageRepository;
        IRepository<LanguageLevel> languageLevelRepository;
        IRepository<JobSeekerLanguages> jobSeekerLanguagesRepository;
        IRepository<JobSeekerJobApplications> jobSeekerJobApplications;
        IRepository<Education> educationRepository;
        IRepository<Qualification> qualificationRepository;
        IRepository<WorkExperience> workExperienceRepository;
        IRepository<JobCategory> jobCategoryRepository;
        IRepository<Attachment> attachmentRepository;
        //IRepository<Interview> interviewRepository;
        IRepository<Interview> interviewRepository;
        IRepository<Interview> _interviewRepository;
        ILogger<JobSeekerService> logger;

        public JobSeekerService(ILogger<JobSeekerService> logger, IRepository<User> userRepository, IRepository<Recruiter> recruiterRepository,
            IRepository<JobSeeker> jobSeekerRepository, IRepository<Gender> genderRepository, IRepository<Title> titleRepository, IRepository<Address> addressRepository,
            IRepository<Ethnicity> ethnicityRepository, IRepository<Province> provinceRepository, IRepository<City> cityRepository, IRepository<Skill> skillRepository,
            IRepository<SkillLevel> skillLevelRepository, IRepository<Language> languageRepository, IRepository<LanguageLevel> languageLevelRepository,
            IRepository<JobSeekerLanguages> jobSeekerLanguagesRepository, IRepository<Education> educationRepository, IRepository<Qualification> qualificationRepository,
            IRepository<WorkExperience> workExperienceRepository, IRepository<JobCategory> jobCategoryRepository, IRepository<JobSeekerSkills> jobSeekerSkillsRepository,
            IRepository<JobSeekerJobApplications> jobSeekerJobApplications, ICompanyService companyService,IJobAdvertService jobAdvertService, IRepository<Attachment> attachmentRepository,
             IRepository<Interview> interviewRepo)
        {
            //this.interviewRepository = interviewRepository;
            _interviewRepository = interviewRepo;
            this.userRepository = userRepository;
            this.jobSeekerRepository = jobSeekerRepository;
            this.addressRepository = addressRepository;
            this.cityRepository = cityRepository;
            this.provinceRepository = provinceRepository;
            this.attachmentRepository = attachmentRepository;
            this.ethnicityRepository = ethnicityRepository;
            this.titleRepository = titleRepository;
            this.genderRepository = genderRepository;
            this.recruiterRepository = recruiterRepository;
            this.skillLevelRepository = skillLevelRepository;
            this.skillRepository = skillRepository;
            this.languageLevelRepository = languageLevelRepository;
            this.jobSeekerLanguagesRepository = jobSeekerLanguagesRepository;
            this.languageRepository = languageRepository;
            this.educationRepository = educationRepository;
            this.qualificationRepository = qualificationRepository;
            this.jobCategoryRepository = jobCategoryRepository;
            this.workExperienceRepository = workExperienceRepository;
            this.jobSeekerSkillsRepository = jobSeekerSkillsRepository;
            this.jobSeekerJobApplications = jobSeekerJobApplications;
            this.companyService = companyService;
            this.jobAdvertService = jobAdvertService;
            this.logger = logger;
        }
        public JobSeeker Get(string id)
        {
            return jobSeekerRepository.Get(id);
        }

        //public JobSeekerPersonalDetailsViewModel GetPersonalDetails(string id);
        public IList<JobSeekerSkillsViewModel> GetJobSeekerSkills(string id)
        {
            var seekerSkills = new List<JobSeekerSkills>();
            var jobSeekerSkillsForViewModel = new List<JobSeekerSkillsViewModel>();
            //Iterate through the skills
            seekerSkills = jobSeekerSkillsRepository.FindByCondition(x => x.JobSeekerId.Equals(id) && x.IsActive == true).ToList();
            //var test = jobSeekerSkillsRepository.FindByCondition(c => c.JobSeekerId == id);
                if(seekerSkills.Count() > 0)
                {
                    foreach (var item in seekerSkills)
                    {
                        var skillToAssign = skillRepository.Get(item.SkillId);
                        var skillLevelToAssign = skillLevelRepository.Get(item.SkillLevelId);

                        jobSeekerSkillsForViewModel.Add(new JobSeekerSkillsViewModel
                        {

                            SkillDescription = skillToAssign.Description,
                            SkillId = skillToAssign.Id,
                            SkillLevelDescription = skillLevelToAssign.Level,
                            SkillLevelId = skillLevelToAssign.Id

                        });
                    }

                    return jobSeekerSkillsForViewModel;
                }
                else
                {
                    return null;
                }
            
            
        }
        public IList<JobSeekerLanguagesViewModel> GetJobSeekerLanguages(string id)
        {
            var seekerLanguages = new List<JobSeekerLanguages>();
            var jobSeekerLanguagesForViewModel = new List<JobSeekerLanguagesViewModel>();

            try
            {
                //Iterate through the skills
                seekerLanguages = jobSeekerLanguagesRepository.FindByCondition(x => x.JobSeekerId.Equals(id)).ToList();

                if (seekerLanguages.Count() > 0)
                {
                    foreach (var item in seekerLanguages)
                    {
                        var LanguageToAssign = languageRepository.Get(item.LanguageId);
                        var LanguageLevelToAssign = languageLevelRepository.Get(item.LanguageLevelId);

                        jobSeekerLanguagesForViewModel.Add(new JobSeekerLanguagesViewModel
                        {

                            LanguageLevelId = LanguageLevelToAssign.Id,
                            LanguageName = LanguageToAssign.Name,
                            LanguageDifficultyLevel = LanguageLevelToAssign.DifficultyLevel,
                            LanguageId = LanguageToAssign.Id

                        });
                    }

                    return jobSeekerLanguagesForViewModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public bool IsJobSeekerLanguageExisting(string languageId,string jobSeekerId)
        {
            var seekerLanguage = jobSeekerLanguagesRepository.FindByCondition(x => x.JobSeekerId.Equals(jobSeekerId) && x.LanguageId.Equals(languageId)).FirstOrDefault();

            return seekerLanguage != null ? true : false;
        }        
        
        public bool IsJobSeekerSkillExisting(string skillId,string jobSeekerId)
        {
            var seekerSkill = jobSeekerSkillsRepository.FindByCondition(x => x.JobSeekerId.Equals(jobSeekerId) && x.SkillId.Equals(skillId)).FirstOrDefault();

            return seekerSkill != null ? true : false;
        }
        //public JobSeekerPersonalDetailsViewModel GetPersonalDetails(string id)
        public JobSeekerPersonalDetailsViewModel GetPersonalDetails(string id)
        {

            try
            {
                var seeker = jobSeekerRepository.Get(id);

                if (seeker != null)
                {
                    var personalDetails = new JobSeekerPersonalDetailsViewModel();

                    personalDetails.FirstName = seeker.FirstName;
                    personalDetails.LastName = seeker.LastName;
                    personalDetails.IdNumber = seeker.IdNumber;
                    personalDetails.Phone = seeker.Phone;
                    personalDetails.TitleId = seeker.TitleId;
                    personalDetails.GenderId = seeker.GenderId;
                    personalDetails.ProvinceId = seeker.ProvinceId;
                    personalDetails.CityId = seeker.CityId;
                    personalDetails.AddressId = seeker.AddressId;
                    personalDetails.StreetName = addressRepository.Get(seeker.AddressId).StreetName;
                    personalDetails.StreetNumber = addressRepository.Get(seeker.AddressId).StreetNumber;
                    personalDetails.BuildingNumber = addressRepository.Get(seeker.AddressId).BuildingNumber;
                    personalDetails.PersonalProfile = seeker.PersonalProfile;
                    personalDetails.Email = userRepository.
                        FindByCondition(c => c.UserId.Equals(id) && c.IsActive == true).
                        FirstOrDefault().Email;

                    var academicRecordAttachment = attachmentRepository.
                                            FindByCondition(a => a.JobSeekerId.Equals(seeker.Id) &&
                                            a.AttachmentType.Equals("AcademicRecord")).
                                            FirstOrDefault();

                    personalDetails.AcademicRecordUrl = academicRecordAttachment != null ? academicRecordAttachment.AttachmentName : null;
                    
                    var cvAttachement = attachmentRepository.FindByCondition(a => a.JobSeekerId.
                                            Equals(seeker.Id) && a.AttachmentType.Equals("CV")).
                                            FirstOrDefault();

                    personalDetails.CVUrl = cvAttachement != null ? cvAttachement.AttachmentName : null;

                    var idAttachment = attachmentRepository.FindByCondition(a => a.JobSeekerId.
                                            Equals(seeker.Id) && a.AttachmentType.Equals("ID")).
                                            FirstOrDefault();

                    personalDetails.IdUrl = idAttachment != null ? idAttachment.AttachmentName : null;
                    personalDetails.JobSeekerId = id;

                    return personalDetails;
                }
                return null;
            }
            catch(Exception exception)
            {
                logger.LogError("Couldn't find user data " + id);
                return null;
            }
        }
        //Lets update personal profile
        public void UpdatePersonalDetails(JobSeekerPersonalDetailsViewModel jobSeekerPersonalDetailsViewModel)
        {
            //Add ID Number later
            var jobSeeker = jobSeekerRepository.Get(jobSeekerPersonalDetailsViewModel.JobSeekerId);
            var newAddressId = Guid.NewGuid().ToString();

            if (jobSeeker == null)
            {
                addressRepository.Insert(new Address {
                    StreetName = jobSeekerPersonalDetailsViewModel.StreetName,
                    StreetNumber = jobSeekerPersonalDetailsViewModel.StreetNumber,
                    BuildingNumber = jobSeekerPersonalDetailsViewModel.BuildingNumber,
                    Id = newAddressId,
                    IsActive = true
                });

                jobSeekerRepository.Insert(new JobSeeker
                {
                    ProvinceId = jobSeekerPersonalDetailsViewModel.ProvinceId,
                    CityId = jobSeekerPersonalDetailsViewModel.CityId,
                    GenderId = jobSeekerPersonalDetailsViewModel.GenderId,
                    EthnicityId = jobSeekerPersonalDetailsViewModel.EthnicityId,
                    TitleId = jobSeekerPersonalDetailsViewModel.TitleId,
                    LastName = jobSeekerPersonalDetailsViewModel.LastName,
                    FirstName = jobSeekerPersonalDetailsViewModel.FirstName,
                    Phone = jobSeekerPersonalDetailsViewModel.Phone,
                    PersonalProfile = jobSeekerPersonalDetailsViewModel.PersonalProfile,
                    IdNumber = jobSeekerPersonalDetailsViewModel.IdNumber,
                    Id = jobSeekerPersonalDetailsViewModel.JobSeekerId,
                    AddressId = newAddressId,
                    IsActive = true
                });
            }
            else
            {
                //Update Address
                var address = addressRepository.Get(jobSeeker.AddressId);
                address.StreetName = jobSeekerPersonalDetailsViewModel.StreetName;
                address.StreetNumber = jobSeekerPersonalDetailsViewModel.StreetNumber;
                address.BuildingNumber = jobSeekerPersonalDetailsViewModel.BuildingNumber;

                addressRepository.Update(address);
                //Update Jobsekker

                jobSeeker.ProvinceId = jobSeekerPersonalDetailsViewModel.ProvinceId;
                jobSeeker.CityId = jobSeekerPersonalDetailsViewModel.CityId;
                jobSeeker.GenderId = jobSeekerPersonalDetailsViewModel.GenderId;
                jobSeeker.EthnicityId = jobSeekerPersonalDetailsViewModel.EthnicityId;
                jobSeeker.TitleId = jobSeekerPersonalDetailsViewModel.TitleId;
                jobSeeker.LastName = jobSeekerPersonalDetailsViewModel.LastName;
                jobSeeker.FirstName = jobSeekerPersonalDetailsViewModel.FirstName;
                jobSeeker.Phone = jobSeekerPersonalDetailsViewModel.Phone;
                jobSeeker.PersonalProfile = jobSeekerPersonalDetailsViewModel.PersonalProfile;
                jobSeeker.IdNumber = jobSeekerPersonalDetailsViewModel.IdNumber;
                jobSeekerRepository.Update(jobSeeker);
            }

        }

        public IList<JobSeekerEducationViewModel> GetJobSeekerEducation(string id)
        {
            var seekerEducation = new List<Education>();
            var jobSeekerEducationForViewModel = new List<JobSeekerEducationViewModel>();

            try
            {
                 //Iterate through the skills
                seekerEducation = educationRepository.FindByCondition(x => x.JobSeekerId.Equals(id)).ToList();
                logger.LogDebug(seekerEducation.Count().ToString());

                if (seekerEducation.Count() > 0)
                {
                    foreach (var item in seekerEducation)
                    {
                        var qualificationToAssign = qualificationRepository.Get(item.QualificationeId);

                        jobSeekerEducationForViewModel.Add(new JobSeekerEducationViewModel
                        {

                            GraduationYear = item.GraduationYear,
                            Insitution = item.Institution,
                            FieldOfStudy = item.FieldOfStudy,
                            Qualification = qualificationToAssign.Name,
                            QualificationId = item.QualificationeId,
                            EducationId = item.Id

                        });
                    }

                    return jobSeekerEducationForViewModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }
        
        public IList<JobSeekerExperienceViewModel> GetJobSeekerWorkExperience(string id)
        {
            var seekerExperience = new List<WorkExperience>();
            var jobSeekerExperienceForViewModel = new List<JobSeekerExperienceViewModel>();

            try
            {
                //Iterate through the skills
                seekerExperience = workExperienceRepository.FindByCondition(x => x.JobSeekerId.Equals(id)).ToList();

                if (seekerExperience.Count() > 0)
                {
                    foreach (var item in seekerExperience)
                    {
                        //Call category from inside the experience list
                        jobSeekerExperienceForViewModel.Add(new JobSeekerExperienceViewModel
                        {

                            JobTitle = item.JobTitle,
                            CompanyName = item.CompanyName,
                            JobCategory = jobCategoryRepository.Get(item.JobCategoryId).Name,
                            IsCurrentCompany = item.IsCurrentCompany,
                            JobCategoryId = item.JobCategoryId,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            WorkExperienceId = item.Id

                        });
                    }

                    return jobSeekerExperienceForViewModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public IList<JobSeekerApplicationHistory> GetJobSeekerApplicationHistory(string id)
        {
            var applicationHistory = jobSeekerJobApplications.FindByCondition(s => s.JobSeekerId.Equals(id)).ToList();
            var jobSeekerApplications = new List<JobSeekerApplicationHistory>();

            if (applicationHistory != null)
            {
                foreach (var item in applicationHistory)
                {
                    jobSeekerApplications.Add(new JobSeekerApplicationHistory
                    {
                        CreatedDate = (DateTime)item.CreatedDate,
                        Status = item.Status,
                        Company = companyService.Get(item.CompanyId).CompanyName,
                        JobTtitle = jobAdvertService.Get(item.JobAdvertId).Caption
                    });
                }

                return jobSeekerApplications;
            }
            return null;
        }

        public ScheduleInterviewViewModel GetJobSeekerApplicationByAdvertId(string advertId, string seekerId)
        {
            var jobApplication = jobSeekerJobApplications.FindByCondition
                (s => s.JobSeekerId.Equals(seekerId) && s.JobAdvertId.Equals(advertId)).FirstOrDefault();

            var company = companyService.Get(jobApplication.CompanyId);
            var address = addressRepository.Get(company.AddressId);
            var recruiter = recruiterRepository.Get(jobApplication.RecruiterId);
            var jobSeeker = jobSeekerRepository.Get(seekerId);
            var jobSeekerEmail = userRepository.FindByCondition(x => x.UserId.Equals(seekerId)).FirstOrDefault().Email;
            var recruiterEmail = userRepository.FindByCondition(x => x.UserId.Equals(jobApplication.RecruiterId)).FirstOrDefault().Email;
            var jobAdvert = jobAdvertService.Get(jobApplication.JobAdvertId);

            return new ScheduleInterviewViewModel
            {
                JobAdvert = jobAdvert,
                Company = company,
                Address = address,
                Recruiter = recruiter,
                JobSeeker = jobSeeker,
                JobSeekerEmail = jobSeekerEmail,
                RecruiterEmail = recruiterEmail
            };
        }

        public void ScheduleInterview(ScheduleInterviewViewModel scheduleInterviewViewModel, Interview interview)
        {
            // Fix bug for when adding physical interviews
            // It breaks because interview link is empty
            var link = "";

            link = interview.InterviewType.Equals("Virtual") ? interview.InterviewLink.Substring(interview.InterviewLink.IndexOf("https://")) : "";

            var interviewCreate = new Interview 
            { 
                Id = Guid.NewGuid().ToString(),
                Company = scheduleInterviewViewModel.Company.CompanyName,
                Job = scheduleInterviewViewModel.JobAdvert.Caption,
                Interviewer = interview.Interviewer,
                InterviewDate = interview.InterviewDate,
                InterviewLink = link,
                InterviewType = interview.InterviewType,
                CompanyId = scheduleInterviewViewModel.Company.Id,
                JobAdvertId = interview.JobAdvertId,
                JobSeekerId = interview.JobSeekerId,
                RecruiterId = scheduleInterviewViewModel.Recruiter.Id,
                DateCreated = DateTime.Now,
                IsActive = true
            };

            _interviewRepository.Insert(interviewCreate);
        }
        public IList<JobSeekerRecruiterSearch> GetJobSeekerBySkillAndCity(string skillId = null, string cityId = null)
        {
            var jobSeekers = jobSeekerRepository.Get();

            var seekers = new List<JobSeeker>();

            if (!String.IsNullOrEmpty(skillId))
            {
                if (skillId.Equals("All") && cityId.Equals("All"))
                {
                    return ProcessRecruiterSearchJobSeeker(jobSeekers);
                }
                else if (!skillId.Equals("All") && cityId.Equals("All"))
                {

                    foreach (var item in jobSeekers)
                    {

                        var seekerSkill = jobSeekerSkillsRepository.FindByCondition(s => s.SkillId.Equals(skillId) && s.JobSeekerId.Equals(item.Id)).FirstOrDefault();

                        if (seekerSkill != null)
                        {
                            if (seekerSkill.SkillId.Equals(skillId))
                            {
                                seekers.Add(item);
                            }
                        }

                    }

                    return ProcessRecruiterSearchJobSeeker(seekers);
                }
                else if (skillId.Equals("All") && !cityId.Equals("All"))
                {
                    jobSeekers = jobSeekers.Where(c => c.CityId.Equals(cityId)).ToList();

                    return ProcessRecruiterSearchJobSeeker(jobSeekers);
                }

                else if (!skillId.Equals("All") && !cityId.Equals("All"))
                {
                    var seekerSkills = jobSeekerSkillsRepository.FindByCondition(s => s.SkillId.Equals(skillId));

                    foreach (var item in jobSeekers)
                    {
                        foreach (var skill in seekerSkills)
                        {
                            if (item.Id.Equals(skill.JobSeekerId))
                            {
                                seekers.Add(item);
                            }
                        }
                    }

                    return ProcessRecruiterSearchJobSeeker(seekers);
                }
            }
            return ProcessRecruiterSearchJobSeeker(jobSeekers);
        }
       

        private IList<JobSeekerRecruiterSearch> ProcessRecruiterSearchJobSeeker(IList<JobSeeker> jobSeekerList)
        {
            if (jobSeekerList == null) { return null; }

            var results = new List<JobSeekerRecruiterSearch>();
            
            foreach (var item in jobSeekerList)
            {
                var education = educationRepository.FindByCondition(e => e.JobSeekerId.Equals(item.Id)).FirstOrDefault();
                //Do not return if profile is still empty, the app will crash

                if (education == null || item.FirstName == null)
                {
                }
                else 
                { 
                    var address = addressRepository.Get(item.AddressId);

                    results.Add(new JobSeekerRecruiterSearch
                    {
                        JobSeekerFullName = item.FirstName + " " + item.LastName,
                        Email = userRepository.FindByCondition(u => u.UserId.Equals(item.Id)).FirstOrDefault().Email,
                        Insitution = education.Institution,
                        FieldOfStudy = education.FieldOfStudy,
                        GraduationYear = education.GraduationYear,
                        Qualification = qualificationRepository.Get(education.QualificationeId).Name,
                        Address = address.StreetNumber + "," + address.StreetName + "," + cityRepository.Get(item.CityId).Name,
                        JobSeekerId = item.Id
                    });
                }
            }

            return results;
        }
        //Breaking Single Responsibility Principle, Deadline too close

        public AdminDashboardStats AdminDashboardStats()
        {
            var adminDashboardStats = new AdminDashboardStats
            {
                Adverts = jobAdvertService.GetAll().Count(),
                Companies = companyService.GetAll().Count(),
                JobSeekers = jobSeekerRepository.Get().Count(),
                Recruiters = recruiterRepository.Get().Count()
            };

            return adminDashboardStats;
        }
        public RecruiterDashboardStats RecruiterDashboardStats(string recruiterId)
        {
            var recruiterDashboardStats = new RecruiterDashboardStats
            {
                Adverts = jobAdvertService.GetByRecruiterId(recruiterId).Count(),
                JobSeekers = jobSeekerJobApplications.FindByCondition(r => r.RecruiterId.Equals(recruiterId)).Count()
            };

            return recruiterDashboardStats;
        }
        public IList<Gender> GetGender()
        {
            return genderRepository.Get();
        }        
        
        public IList<Ethnicity> GetEthnicity()
        {
            return ethnicityRepository.Get();
        }       
        
        public IList<Language> GetLanguages()
        {
            return languageRepository.Get();
        }        
        
        public IList<LanguageLevel> GetLanguageLevel()
        {
            return languageLevelRepository.Get();
        }        
        
        public IList<SkillLevel> GetSkillLevel()
        {
            return skillLevelRepository.Get();
        }        
        
        public IList<Title> GetTitle()
        {
            return titleRepository.Get();
        }  
        
        public IList<Qualification> GetQualification()
        {
            return qualificationRepository.Get();
        }

        public IList<ViewInterviewViewModel> GetInterviews(string recruiterId)
        {
            var interview = _interviewRepository.FindByCondition(x => x.RecruiterId.Equals(recruiterId)).ToList();

            var interviewList = new List<ViewInterviewViewModel>();

            foreach (var item in interview)
            {
                var applicant = jobSeekerRepository.Get(item.JobSeekerId);
                var email = userRepository.FindByCondition(x => x.UserId.Equals(item.JobSeekerId)).FirstOrDefault().Email;

                interviewList.Add(new ViewInterviewViewModel { 
                    InterviewDate = item.InterviewDate,
                    Time = item.InterviewDate.Value.ToString("HH:mm"),
                    Interviewer = item.Interviewer,
                    InterviewLink = item.InterviewLink,
                    InterviewType = item.InterviewType,
                    Job = jobAdvertService.Get(item.JobAdvertId).Caption,
                    JobAdvertId = item.JobAdvertId,
                    JobSeekerId = item.JobSeekerId,
                    Phone = applicant.Phone,
                    Email = email,
                    Applicant = applicant.FirstName + " " +applicant.LastName
                });
            }

            return interviewList;
        }

        public IList<ViewInterviewViewModel> GetSeekersInterviews(string seekerId)
        {
            //Company Job Date Time Address Phone Email Interviewer
            var interview = _interviewRepository.FindByCondition(x => x.JobSeekerId.Equals(seekerId)).ToList();

            var interviewList = new List<ViewInterviewViewModel>();

            foreach (var item in interview)
            {
                var recruiter = recruiterRepository.Get(item.RecruiterId);
                var email = userRepository.FindByCondition(x => x.UserId.Equals(item.RecruiterId)).FirstOrDefault().Email;
                var address = addressRepository.Get(companyService.Get(item.CompanyId).AddressId);

                interviewList.Add(new ViewInterviewViewModel
                {
                    InterviewDate = item.InterviewDate,
                    Time = item.InterviewDate.Value.ToString("HH:mm"),
                    Interviewer = item.Interviewer,
                    InterviewType = item.InterviewType,
                    Company = item.Company,
                    Address = address.StreetNumber + ", " + address.StreetName,
                    Job = item.Job,
                    JobAdvertId = item.JobAdvertId,
                    Phone = recruiter.Phone,
                    Email = email,
                    Applicant = recruiter.FirstName + " " + recruiter.LastName
                });
            }

            return interviewList;
        }
    }
}
