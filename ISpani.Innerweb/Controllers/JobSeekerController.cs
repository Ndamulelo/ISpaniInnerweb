using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using ISpaniInnerweb.Domain.Models.EducationViewModel;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ISpaniInnerweb.Domain.Models.JobAdvertViewModel;
using System.Text.RegularExpressions;
using System.IO;
using ISpaniInnerweb.Domain.Interfaces.Helpers;
using System.Text;
using ClosedXML.Excel;
using System.Xml;
using System.Data;

namespace ISpaniInnerweb.Controllers
{
    public class JobSeekerController : Controller
    {
        private readonly IJobSeekerService _jobSeekerService;
        private readonly IJobTypeService _jobTypeService;
        private readonly ICompanyService _companyService;
        private readonly IJobAdvertService _jobAdvertService;
        private readonly IProvinceService _provinceService;
        private readonly ICityService _cityService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly IWorkExperienceService _workExperienceService;
        private readonly IEducationService educationService;
        private readonly ILanguageService languageService;
        private readonly ISkillsService skillService;
        private readonly IAttachmentService attachmentService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IPDFGenerator pdfGenerator;
        private string CV;
        private string ID;
        private string Transcripts;
        public JobSeekerController(IUserService userService, IJobSeekerService jobSeekerService, IJobAdvertService jobAdvertService,
            IProvinceService provinceService, ICityService cityService, IJobCategoryService jobCategoryService, IWorkExperienceService workExperienceService,
            IEducationService educationService, ISkillsService skillService, ILanguageService languageService, IWebHostEnvironment webHostEnvironment,
            ICompanyService companyService, IJobTypeService jobTypeService, IAttachmentService attachmentService,IPDFGenerator pdfGenerator)
        {
            _jobSeekerService = jobSeekerService;
            _jobAdvertService = jobAdvertService;
            _provinceService = provinceService;
            _cityService = cityService;
            this.attachmentService = attachmentService;
            _jobAdvertService = jobAdvertService;
            _workExperienceService = workExperienceService;
            _jobCategoryService = jobCategoryService;
            this.educationService = educationService;
            this.languageService = languageService;
            this.skillService = skillService;
            this.webHostEnvironment = webHostEnvironment;
            _companyService = companyService;
            _jobTypeService = jobTypeService;
            CV = "/hustlersAttachments/cv/";
            Transcripts = "/hustlersAttachments/academicRecord/";
            ID = "/hustlersAttachments/id/";
        }

        //Update or Add Profile data

        
        public JsonResult UpdatePersonalDetails(JobSeekerPersonalDetailsViewModel jobSeekerPersonalDetailsViewModel)
        {
            jobSeekerPersonalDetailsViewModel.JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId");
            var response = new HustlersResponse<JobSeekerPersonalDetailsViewModel>();
            bool isValidInput = true;

            if(String.IsNullOrEmpty(jobSeekerPersonalDetailsViewModel.FirstName))
            {
                response.Status = "fail";
                response.Messages = "FirstName is required\n";
                isValidInput = false;
                return Json(response);
            }            
            
            if(jobSeekerPersonalDetailsViewModel.FirstName.Length > 50)
            {
                response.Status = "fail";
                response.Messages = "FirstName cannot exceed 50 characters\n";
                isValidInput = false;
                return Json(response);
            }


            if (String.IsNullOrEmpty(jobSeekerPersonalDetailsViewModel.LastName))
            {
                response.Status = "fail";
                response.Messages = "LastName is required\n";
                isValidInput = false;
                return Json(response);
            }

            if (jobSeekerPersonalDetailsViewModel.LastName.Length > 50)
            {
                response.Status = "fail";
                response.Messages = "LastName cannot exceed 50 characters\n";
                isValidInput = false;
                return Json(response);
            }

            if (String.IsNullOrEmpty(jobSeekerPersonalDetailsViewModel.IdNumber))
            {
                response.Status = "fail";
                response.Messages = "Id Number is required\n";
                isValidInput = false;
                return Json(response);
            }
                        
            
            if (jobSeekerPersonalDetailsViewModel.IdNumber.Length > 13)
            {
                response.Status = "fail";
                response.Messages = "Id Number cannot be more than 13 digits\n";
                isValidInput = false;
                return Json(response);
            }

            if (String.IsNullOrEmpty(jobSeekerPersonalDetailsViewModel.Phone))
            {
                response.Status = "fail";
                response.Messages = "Phone is required\n";
                isValidInput = false;
                return Json(response);
            }            
            
            if (jobSeekerPersonalDetailsViewModel.Phone.Length > 10)
            {
                response.Status = "fail";
                response.Messages = "Phone cannot be more than 10 digits \n";
                isValidInput = false;
                return Json(response);
            }

            if (!Regex.Match(jobSeekerPersonalDetailsViewModel.FirstName, "^[A-Z][a-zA-Z]*$").Success)
            {
                response.Status = "fail";
                response.Messages = "Enter a valid FirstName\n";
                isValidInput = false;
            }
            
            if(!Regex.Match(jobSeekerPersonalDetailsViewModel.LastName, "^[A-Z][a-zA-Z]*$").Success)
            {
                response.Messages = response.Messages + " Enter a valid LastName\n";
                isValidInput = false;
            }
            
            var dob = jobSeekerPersonalDetailsViewModel.IdNumber.Substring(0, 6);
           
            if(Int32.Parse(dob.Substring(2,2)) > 12 || Int32.Parse(dob.Substring(4, 2)) > 31)
            {
                isValidInput = false;
                response.Messages = response.Messages + " Enter a valid ID Number\n";
            }

            if(isValidInput)
            {
                //Call update personal details service
                response.Status = "ok";
                response.Messages = "Personal Details Updated Successfully";
                _jobSeekerService.UpdatePersonalDetails(jobSeekerPersonalDetailsViewModel);
            }


            return Json(response);
        }

        [HttpGet]
        public IActionResult JobInterviews()
        {

            return View("JobInterview",_jobSeekerService.GetSeekersInterviews(HttpContext.Session.Get<string>("JobSeekerId")));
        }

        public JsonResult UpdateExperience(JobSeekerExperienceViewModel jobSeekerExperienceViewModel)
        {
            //jobSeekerPersonalDetailsViewModel.JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId");
            var response = new HustlersResponse<JobSeekerPersonalDetailsViewModel>();

                _workExperienceService.Update(jobSeekerExperienceViewModel);
                response.Status = "ok";
                response.Messages = "Successfully Updated";

            return Json(response);
        }

        [HttpPost]
        public JsonResult UpdateEducation(Domain.Models.EducationViewModel.JobSeekerEducationViewModel jobSeekerEducationViewModel)
        {
            var response = new HustlersResponse<Domain.Models.EducationViewModel.JobSeekerEducationViewModel>();
            //Check Validations
            educationService.Edit(jobSeekerEducationViewModel);
            response.Messages = "Education Updated Successfully";
            response.Status = "ok";
            return Json(response);
        }

        [HttpPost]
        public JsonResult UpdateSkill(JobSeekerSkillsViewModel jobSeekerSkillsViewModel)
        {
            var response = new HustlersResponse<JobSeekerSkillsViewModel>();
            jobSeekerSkillsViewModel.JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId");
            //Check Validations
            skillService.Edit(jobSeekerSkillsViewModel);
            response.Messages = "Skill Updated Successfully";
            response.Status = "ok";
            return Json(response);
        }        
        
        [HttpPost]
        public JsonResult UpdateLanguage(JobSeekerLanguagesViewModel jobSeekerLanguagesViewModel)
        {
            var response = new HustlersResponse<JobSeekerLanguagesViewModel>();
            jobSeekerLanguagesViewModel.JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId");
            //Check Validations
            languageService.Edit(jobSeekerLanguagesViewModel);
            response.Messages = "Language Updated Successfully";
            response.Status = "ok";
            return Json(response);
        }

        public ActionResult Profile()
        {
            var jobSeekerId = HttpContext.Session.Get<string>("JobSeekerId");

            //Get personal data
            var personalData = _jobSeekerService.GetPersonalDetails(jobSeekerId);
            //Get Skills
            var skillsData = _jobSeekerService.GetJobSeekerSkills(jobSeekerId);
            //Get Experience
            var experienceData = _jobSeekerService.GetJobSeekerWorkExperience(jobSeekerId);
            //Get Languages
            var LanguagesData = _jobSeekerService.GetJobSeekerLanguages(jobSeekerId);
            //Get Education
            var educationData = _jobSeekerService.GetJobSeekerEducation(jobSeekerId);

            //Get attachments
            //var AttachmentsData = _jobSeekerService.GetAttachments(jobSeekerId);

            var jobSeekerProfileViewModelContainer = new JobSeekerProfileViewModelContainer
            {
                JobSeekerPersonalDetailsViewModel = personalData,
                JobSeekerExperienceViewModel = experienceData,
                JobSeekerEducationViewModel = educationData,
                JobSeekerSkillsViewModel = skillsData,
                JobSeekerLanguagesViewModel = LanguagesData,
                Genders = _jobSeekerService.GetGender(),
                Titles = _jobSeekerService.GetTitle(),
                Ethnicities = _jobSeekerService.GetEthnicity(),
                LanguageLevels = _jobSeekerService.GetLanguageLevel(),
                SkillLevels = _jobSeekerService.GetSkillLevel(),
                Provinces = _provinceService.GetAll(),
                Cities = _cityService.GetAll(),
                JobCategories =_jobCategoryService.GetAll(),
                Qualifications = _jobSeekerService.GetQualification(),
                JobSeekerCreateAcademicRecordViewModel = new JobSeekerCreateAcademicRecordViewModel(),
                JobSeekerCVCreateViewModel = new JobSeekerCVCreateViewModel(),
                JobSeekerCreateIdViewModel = new JobSeekerCreateIdViewModel()
            };

            return View(jobSeekerProfileViewModelContainer);
        }


        public IActionResult UploadCV(IFormFile CV)
        {

            if (CV == null)
            {
                return View("FileIsNull");
            }

            //On Success redirecct to the success page
            var cvName = SaveToStorage(this.CV, CV);
            var cvAttachment = new Attachment
            {
                Id = Guid.NewGuid().ToString(),
                AttachmentType = "CV",
                JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId"),
                AttachmentName = cvName,
                IsActive = true
            };
            //Call a service for uploading a CV
            attachmentService.Create(cvAttachment);
            return View("CvUploadSuccess");
        }        
         
        public IActionResult UploadId(IFormFile ID)
        {
            if (ID == null)
            {
                return View("FileIsNull");
            }

            //On Success redirecct to the success page
            var idName = SaveToStorage(this.ID, ID);
            var idAttachment = new Attachment
            {
                Id = Guid.NewGuid().ToString(),
                AttachmentType = "ID",
                JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId"),
                AttachmentName = idName,
                IsActive = true
            };
            //Call a service for uploading a CV
            attachmentService.Create(idAttachment);
            return View("IDUploadSuccess");
        }

        public IActionResult UploadAcademicRecord(IFormFile AcademicRecord)
        {
            //On Success redirecct to the success page
            //Check if a file is empty

            if(AcademicRecord == null)
            {
                return View("FileIsNull");
            }

            var academicRecordName = SaveToStorage(this.Transcripts, AcademicRecord);

            var academicRecordAttachment = new Attachment
            {
                Id = Guid.NewGuid().ToString(),
                AttachmentType = "AcademicRecord",
                JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId"),
                AttachmentName = academicRecordName,
                IsActive = true
            };
            //Call a service for uploading a Transcript
            attachmentService.Create(academicRecordAttachment);
            return View("AcademicRecordUploadSuccess");
        }

        private string SaveToStorage(string folderPath, IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            folderPath += fileName;

            string serverFolder = webHostEnvironment.WebRootPath + folderPath;

             file.CopyTo(new FileStream(serverFolder, FileMode.Create));

            return fileName;
        }

        // GET: JobSeekerController
        [HttpGet]
        public ActionResult Index(string jobCityId,string jobCategoryId)
        {
            //var temp = _jobSeekerService.GetJobSeekerBySkillAndCity();
            //var companies = _companyService.GetAll();
            var jobTypes = _jobTypeService.GetAll();
            var jobCategories = _jobCategoryService.GetAll();
            var jobCities = _cityService.GetAll();
            jobCities.Insert(0,new City { Id = "All",Name="All"});
            jobCategories.Insert(0,new JobCategory { Id = "All", Name ="All"});
            var jobAdverts = _jobAdvertService.GetAllByJobSeeker(jobCategoryId, jobCityId);

            JobSeekerSearchAdvertViewModel jobSeekerSearchAdvertViewModel = new JobSeekerSearchAdvertViewModel
            {
                ViewJobAdvertViewModel = (List<ViewJobAdvertViewModel>)jobAdverts,
                JobCategories = (List<JobCategory>)jobCategories,
                JobTypes = (List<JobType>)jobTypes,
                JobCities = (List<City>)jobCities
            };

            return View("Dashboard",jobSeekerSearchAdvertViewModel);
        }

        // GET: Recruiter to search JobSeeker
        [HttpGet]
        public ActionResult SearchJobSeeker(string skillId, string cityId)
        {
            var skills = skillService.GetAll();
            var cities = _cityService.GetAll();
            cities.Insert(0, new City { Id = "All", Name = "All" });
            skills.Insert(0, new Skill { Id = "All", Description = "All" });
            //Lets setup a session to control when to display Invite and Decline button
            HttpContext.Session.Set<string>("InviteDecline", "no"); 
            HttpContext.Session.Set<string>("SkillId", skillId); 
            HttpContext.Session.Set<string>("CityId", cityId); 
            
            var jobSeekerRecruiterSearchContainer = new JobSeekerRecruiterSearchContainer
            {
                JobSeekerRecruiterSearch = _jobSeekerService.GetJobSeekerBySkillAndCity(skillId, cityId),
                Cities = cities,
                Skills = skills
            };

            return View("RecruiterSearch", jobSeekerRecruiterSearchContainer);
        }

        //Export Job Seeker Search Results
        public IActionResult ExportRecruiterJobSeekerSearchResults()
        {

            var jobSeekers = _jobSeekerService.GetJobSeekerBySkillAndCity(HttpContext.Session.Get<string>("SkillId"), 
                HttpContext.Session.Get<string>("CityId"));

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Job Seekers");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "Email";
                worksheet.Cell(currentRow, 3).Value = "Qualification";
                worksheet.Cell(currentRow, 4).Value = "Institution";
                worksheet.Cell(currentRow, 5).Value = "Address";

                worksheet.Columns().AdjustToContents();

                foreach (var appliedJob in jobSeekers)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = appliedJob.JobSeekerFullName;
                    worksheet.Cell(currentRow, 2).Value = appliedJob.Email;
                    worksheet.Cell(currentRow, 3).Value = appliedJob.Qualification;
                    worksheet.Cell(currentRow, 4).Value = appliedJob.Insitution;
                    worksheet.Cell(currentRow, 5).Value = appliedJob.Address;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "JobSeekerSearchReport_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }
            }
        }
        public ActionResult JobApplicationHistory()
        {

            return View("JobApplicationHistory",_jobSeekerService.GetJobSeekerApplicationHistory(HttpContext.Session.Get<string>("JobSeekerId")));
        }

        private IActionResult ExportJobSeekerApplicationHistoryToExcel(DataTable applicationHistory)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Application History");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "ApplicationDate";
                worksheet.Cell(currentRow, 2).Value = "JobTtitle";
                worksheet.Cell(currentRow, 3).Value = "Company";
                worksheet.Cell(currentRow, 4).Value = "Status";
                worksheet.Columns().AdjustToContents();

                for (int i = 0; i < applicationHistory.Rows.Count; i++)
                {
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = applicationHistory.Rows[i]["CreatedDate"];
                        worksheet.Cell(currentRow, 2).Value = applicationHistory.Rows[i]["JobTtitle"];
                        worksheet.Cell(currentRow, 3).Value = applicationHistory.Rows[i]["Company"];
                        worksheet.Cell(currentRow, 4).Value = applicationHistory.Rows[i]["Status"];

                    }
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "jobApplicationHistory_"+DateTime.Now.ToString("yyyyMMddHHmmss")+".xlsx");
            }
        }
        private DataTable JobSeekerApplicationHistoryDataTable(IList<JobSeekerApplicationHistory> applicationHistoryList)
        {

            DataTable dtApplicationHistory = new DataTable("jobSeekerApplicationHistory");
            dtApplicationHistory.Columns.AddRange(new DataColumn[4] { new DataColumn("JobTtitle"),
                                            new DataColumn("Company"),
                                            new DataColumn("Status"),
                                            new DataColumn("CreatedDate") });
            foreach (var application in applicationHistoryList)
            {
                dtApplicationHistory.Rows.Add(application.JobTtitle, application.Company, application.Status, application.CreatedDate);
            }

            return dtApplicationHistory;
        }


        [HttpPost]
        public IActionResult ExportJobSeekerHistoryReport()
        {

            var dtJobSeekerApplicationHistory = JobSeekerApplicationHistoryDataTable(_jobSeekerService.GetJobSeekerApplicationHistory(HttpContext.Session.Get<string>("JobSeekerId")));
            return ExportJobSeekerApplicationHistoryToExcel(dtJobSeekerApplicationHistory);
        }

        // GET: JobSeekerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: JobSeekerController/Create
        public ActionResult Create()
        {
            return View();
        }

    }
}
