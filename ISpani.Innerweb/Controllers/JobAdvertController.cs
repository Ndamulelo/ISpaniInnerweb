using ClosedXML.Excel;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Helpers;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Interfaces.Services.Communication;
using ISpaniInnerweb.Domain.Models;
using ISpaniInnerweb.Domain.Models.JobAdvertViewModel;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace ISpaniInnerweb.Controllers
{
    public class JobAdvertController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ICompanyService _companyService;
        private readonly IJobAdvertService _jobAdvertService;
        private readonly IJobSeekerService _jobSeekerService;
        private readonly IExperienceLevelService _experienceLevelService;
        private readonly IProvinceService _provinceService;
        private readonly IJobTypeService _jobTypeService;
        private readonly IRecruiterService _recruiterService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ICityService _cityService;
        private readonly ILogger<JobAdvertController> _logger;
        private readonly IStringManipulator _stringManipulator;
        private readonly IWebHostEnvironment webHostEnvironment;
        private string CV;
        private string ID;
        private string Transcripts;

        public JobAdvertController(ICompanyService companyService, IJobAdvertService jobAdvertService, IExperienceLevelService experienceLevelService,
            IProvinceService provinceService, IRecruiterService recruiterService, IJobCategoryService jobCategoryService,IJobTypeService jobTypeService,
            IStringManipulator stringManipulator, ICityService cityService, IJobSeekerService jobSeekerService, IWebHostEnvironment webHostEnvironment,
            IEmailService emailService)
        {
            _emailService = emailService;
            _companyService = companyService;
            _jobSeekerService = jobSeekerService;
            _provinceService = provinceService;
            _recruiterService = recruiterService;
            _cityService = cityService;
            _experienceLevelService = experienceLevelService;
            _jobAdvertService = jobAdvertService;
            _jobCategoryService = jobCategoryService;
            _jobTypeService = jobTypeService;
            _stringManipulator = stringManipulator;
            this.webHostEnvironment = webHostEnvironment;
            CV = "\\hustlersAttachments\\cv\\";
            Transcripts = "\\hustlersAttachments\\academicRecord\\";
            ID = "\\hustlersAttachments\\id\\";
        }
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(PrepareJobAdvertModel());
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateJobAdvertViewModel createJobAdvertViewModel)
        {
            var sessionObject = HttpContext.Session;

            //Create validations here
            //var isValidDates = createJobAdvertViewModel.StartDate.CompareTo(createJobAdvertViewModel.EndDate);
            if (createJobAdvertViewModel.SalaryFrom > createJobAdvertViewModel.SalaryTo)
            {
                ModelState.AddModelError("SalaryTo", "Salary From field cannot be greater than Salary To field");
            }


            if (createJobAdvertViewModel.EndDate < createJobAdvertViewModel.StartDate)
            {
                ModelState.AddModelError("StartDate", "Start Date field cannot be greater than End Date field");
            }

            if (!string.IsNullOrEmpty(createJobAdvertViewModel.RecruiterId))
            {
                if (createJobAdvertViewModel.RecruiterId.Equals("0") || createJobAdvertViewModel.CompanyId.Equals("0"))
                {
                    ModelState.AddModelError("CompanyId", "Select a company and relevant recruiter");
                }
            }

            if (ModelState.IsValid)
            {
                /**
                 * Here its important to check who is creating a job advert because forms are different 
                 * for admin and recruiter service for registering is being reused.
                 */
                TempData["AdvertCreate"] = "Advert Successfully Created";
                if (sessionObject.Get<string>("Role").Equals("Admin"))
                {
                    _jobAdvertService.Create(createJobAdvertViewModel);
                }
                else
                {
                    //Get a recruiter from the db so that we have a company ID
                    var recruiter = _recruiterService.Get(sessionObject.Get<string>("RecruiterId"));
                    createJobAdvertViewModel.RecruiterId = recruiter.Id;

                    createJobAdvertViewModel.CompanyId = recruiter.CompanyId;

                    _jobAdvertService.Create(createJobAdvertViewModel);
                }
                //Redirect to success page
                return View(PrepareJobAdvertModel());
            }
            else
            {
                return View(PrepareJobAdvertModel());
            }
        }

        private CreateJobAdvertViewModel PrepareJobAdvertModel()
        {
            CreateJobAdvertViewModel createJobAdvertViewModel = new CreateJobAdvertViewModel
            {
                JobCategories = _jobCategoryService.GetAll(),
                JobTypes = _jobTypeService.GetAll(),
                ExperienceLevels = _experienceLevelService.GetAll(),
                Companies = _companyService.GetAll(),
                Recruiters = _recruiterService.Get(),
                Cities = _cityService.GetAll()
            };

            return createJobAdvertViewModel;
        }
        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult ScheduleInterview(string sekkerId, string advertId) 
        {
            return View(new Interview { JobSeekerId = sekkerId, JobAdvertId = advertId });
        }        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ScheduleInterview(Interview interview) 
        {
            var scheduleInterview = _jobSeekerService.GetJobSeekerApplicationByAdvertId(interview.JobAdvertId,interview.JobSeekerId);
                
            _jobSeekerService.ScheduleInterview(scheduleInterview, interview);
            //Update status on JobApplication Table
            _jobAdvertService.InviteToInverView(interview.JobAdvertId, interview.JobSeekerId);
            //Send notification


            return View("InterviewInviteSuccess"); 
        }
        //Exporting Applicant Interviews
        private IActionResult ExportJobSeekerInterviewHistoryToExcel(DataTable applicationHistory)
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Application History");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Company";
                worksheet.Cell(currentRow, 2).Value = "Job";
                worksheet.Cell(currentRow, 3).Value = "Date";
                worksheet.Cell(currentRow, 4).Value = "Time";
                worksheet.Cell(currentRow, 5).Value = "Address";
                worksheet.Cell(currentRow, 6).Value = "Phone";
                worksheet.Cell(currentRow, 7).Value = "Email";
                worksheet.Cell(currentRow, 8).Value = "Recruiter";
                worksheet.Cell(currentRow, 9).Value = "Type";
                worksheet.Columns().AdjustToContents();

                for (int i = 0; i < applicationHistory.Rows.Count; i++)
                {
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = applicationHistory.Rows[i]["Company"];
                        worksheet.Cell(currentRow, 2).Value = applicationHistory.Rows[i]["Job"];
                        worksheet.Cell(currentRow, 3).Value = applicationHistory.Rows[i]["Date"];
                        worksheet.Cell(currentRow, 4).Value = applicationHistory.Rows[i]["Time"];
                        worksheet.Cell(currentRow, 5).Value = applicationHistory.Rows[i]["Address"];
                        worksheet.Cell(currentRow, 6).Value = applicationHistory.Rows[i]["Phone"];
                        worksheet.Cell(currentRow, 7).Value = applicationHistory.Rows[i]["Email"];
                        worksheet.Cell(currentRow, 8).Value = applicationHistory.Rows[i]["Recruiter"];
                        worksheet.Cell(currentRow, 9).Value = applicationHistory.Rows[i]["Type"];

                    }
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(
                    content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "JobInterviews_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            }
        }
        private DataTable JobSeekerInterviewHistoryDataTable(IList<ViewInterviewViewModel> applicationHistoryList)
        {
            //Bad naming used for the sake of time
            DataTable dtApplicationHistory = new DataTable("jobSeekerApplicationHistory");
            dtApplicationHistory.Columns.AddRange(new DataColumn[9] { new DataColumn("Company"),
                                            new DataColumn("Job"),
                                            new DataColumn("Date"),
                                            new DataColumn("Time"),
                                            new DataColumn("Address"),
                                            new DataColumn("Phone"),
                                            new DataColumn("Email"),
                                            new DataColumn("Recruiter"),
                                            new DataColumn("Type")});
            foreach (var application in applicationHistoryList)
            {
                dtApplicationHistory.Rows.Add(application.Company, application.Job, application.InterviewDate.Value.ToString("yyyy-MM-dd"), 
                    application.Time, application.Address, application.Phone, application.Email, application.Applicant, application.InterviewType);
            }

            return dtApplicationHistory;
        }


        [HttpPost]
        public IActionResult ExportJobSeekerInterviewHistoryReport()
        {
            var dtJobSeekerApplicationHistory = JobSeekerInterviewHistoryDataTable(_jobSeekerService.GetSeekersInterviews(HttpContext.Session.Get<string>("JobSeekerId")));
            return ExportJobSeekerInterviewHistoryToExcel(dtJobSeekerApplicationHistory);
        }

        [HttpGet]
        public IActionResult Interviews()
        {
            return View(_jobSeekerService.GetInterviews(HttpContext.Session.Get<string>("RecruiterId")));
        }

        //Export Interviews For Recruiter 
        public IActionResult ExportRecruiterInterviews()
        {
            var interviews = _jobSeekerService.GetInterviews(HttpContext.Session.Get<string>("RecruiterId"));

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Recruiter Interviews");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Job";
                worksheet.Cell(currentRow, 2).Value = "Date";
                worksheet.Cell(currentRow, 3).Value = "Time";
                worksheet.Cell(currentRow, 4).Value = "Interviewer";
                worksheet.Cell(currentRow, 5).Value = "Applicant";
                worksheet.Cell(currentRow, 6).Value = "Email";
                worksheet.Cell(currentRow, 7).Value = "Phone";
                worksheet.Cell(currentRow, 8).Value = "InterviewLink";
                worksheet.Cell(currentRow, 9).Value = "InterviewType";

                worksheet.Columns().AdjustToContents();

                foreach (var interview in interviews)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = interview.Job;
                    worksheet.Cell(currentRow, 2).Value = interview.InterviewDate.Value.ToString("yyyy-MM-dd");
                    worksheet.Cell(currentRow, 3).Value = interview.InterviewDate.Value.ToString("HH:mm");
                    worksheet.Cell(currentRow, 4).Value = interview.Interviewer;
                    worksheet.Cell(currentRow, 5).Value = interview.Applicant;
                    worksheet.Cell(currentRow, 6).Value = interview.Email;
                    worksheet.Cell(currentRow, 7).Value = interview.Phone;
                    worksheet.Cell(currentRow, 8).Value = interview.InterviewLink;
                    worksheet.Cell(currentRow, 9).Value = interview.InterviewType;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "AppliedJobsReport" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }
            }
        }
        // GET: For Admin and Recruiter
        [HttpGet]
        public ActionResult JobAdvertList(string jobTypeId, string companyId, DateTime dateFrom, DateTime dateTo, string jobCategoryId)
        {
            var companies = _companyService.GetAll();
            var jobTypes = _jobTypeService.GetAll();
            var jobCategories = _jobCategoryService.GetAll();
            var jobAdverts = _jobAdvertService.GetAllByAdminRecruiter(jobTypeId,companyId,dateFrom,dateTo,jobCategoryId);

            AdminRecruiterSearchAdvertViewModel adminRecruiterSearchAdvertViewModel = new AdminRecruiterSearchAdvertViewModel
            {
                ViewJobAdvertViewModel = (List<ViewJobAdvertViewModel>)jobAdverts,
                Companies = (List<Company>)companies,
                JobCategories = (List<JobCategory>)jobCategories,
                JobTypes = (List<JobType>)jobTypes

            };
            return View("JobAdvertList", adminRecruiterSearchAdvertViewModel);
        }           
        
        [HttpGet]
        public ActionResult RecruiterAdvertList(string jobTypeId, string jobCategoryId)
        {
            var jobTypes = _jobTypeService.GetAll();
            var jobCategories = _jobCategoryService.GetAll();
            var jobAdverts = _jobAdvertService.GetAllByRecruiter(HttpContext.Session.Get<string>("RecruiterId"),jobTypeId,jobCategoryId);
            jobCategories.Insert(0, new JobCategory { Id = "All", Name = "All" });
            jobTypes.Insert(0,new JobType { Id = "All", Description = "All"});

            AdminRecruiterSearchAdvertViewModel adminRecruiterSearchAdvertViewModel = new AdminRecruiterSearchAdvertViewModel
            {
                ViewJobAdvertViewModel = (List<ViewJobAdvertViewModel>)jobAdverts,
                JobCategories = (List<JobCategory>)jobCategories,
                JobTypes = (List<JobType>)jobTypes

            };
            return View("RecruiterViewAdvert", adminRecruiterSearchAdvertViewModel);
        }     
        
        //Apply for a job
        [HttpPost]
        public IActionResult ApplyForJob(JobSeekerJobDetailsViewModel jobSeekerJobDetailsViewModel)
        {
            _jobAdvertService.ApplyJob(
                jobSeekerJobDetailsViewModel.JobAdvertId,
                jobSeekerJobDetailsViewModel.RecruiterId,
                jobSeekerJobDetailsViewModel.CompanyId,
                HttpContext.Session.Get<string>("JobSeekerId")
                );

            TempData["Apply"] = "Job Applied";

            var jobDetails = _jobAdvertService.GetDetailedJob(jobSeekerJobDetailsViewModel.JobAdvertId);

            if (_jobAdvertService.IsAlreadyApplied(jobDetails.JobAdvertId, HttpContext.Session.Get<string>("JobSeekerId")))
            {
                jobDetails.IsAlreadyAppliedBySeeker = true;
            }



            return View("JobSeekerJobAdvertDetails", jobDetails);
        }

        [HttpPost]
        public IActionResult DeclineApplication(string jobSeekerId, string jobAdvertId)
        {

            //Call service to update job Application ststus
            _jobAdvertService.DeclineApplication(jobAdvertId, jobSeekerId);
            return View("InterviewDeclineSuccess");
        }

        [HttpPost]
        public IActionResult InviteForInterView(string jobSeekerId, string jobAdvertId)
        {
            _jobAdvertService.InviteToInverView(jobAdvertId, jobSeekerId);
            //Call service to update job Application ststus
            return View("InterviewInviteSuccess");
        }

        // Recruiter Get Applied Jobs 
        public ActionResult RecruiterViewAppliedJobs()
        {
            var rec = HttpContext.Session.Get<string>("RecruiterId");
            HttpContext.Session.Set<string>("InviteDecline", "yes");
            var appliedJobs = _recruiterService.GetAppliedJobs(HttpContext.Session.Get<string>("RecruiterId"));

            return View("AdminRecruiterViewAppliedJobs",appliedJobs);
        }


        public IActionResult ExportRecruiterAppliedJobsReport()
        {
            var appliedJobs = _recruiterService.GetAppliedJobs(HttpContext.Session.Get<string>("RecruiterId"));

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Time";
                worksheet.Cell(currentRow, 3).Value = "ApplicantName";
                worksheet.Cell(currentRow, 4).Value = "Email";
                worksheet.Cell(currentRow, 5).Value = "Phone";
                worksheet.Cell(currentRow, 6).Value = "Job";
                worksheet.Cell(currentRow, 7).Value = "ApplicationStatus";

                worksheet.Columns().AdjustToContents();

                foreach (var appliedJob in appliedJobs)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = appliedJob.DateCreated.ToString("yyyy-MM-dd");
                    worksheet.Cell(currentRow, 2).Value = appliedJob.DateCreated.ToString("HH:mm");
                    worksheet.Cell(currentRow, 3).Value = appliedJob.ApplicantName;
                    worksheet.Cell(currentRow, 4).Value = appliedJob.Email;
                    worksheet.Cell(currentRow, 5).Value = appliedJob.ApplicantPhone;
                    worksheet.Cell(currentRow, 6).Value = appliedJob.JobCaption;
                    worksheet.Cell(currentRow, 7).Value = appliedJob.ApplicationStatus;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "RecruiterAppliedJobsReport" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }
            }
        }

        public ActionResult RecruiterAppliedJobsDetails(string id,string jobAdvertId)
        {
            var viewApplicantProfileViewModel = new ViewApplicantProfileViewModel
            {
                JobSeekerSkillsViewModel = _jobSeekerService.GetJobSeekerSkills(id),
                JobSeekerPersonalDetailsViewModel = _jobSeekerService.GetPersonalDetails(id),
                JobSeekerEducationViewModel = _jobSeekerService.GetJobSeekerEducation(id),
                JobSeekerExperienceViewModel = _jobSeekerService.GetJobSeekerWorkExperience(id),
                JobSeekerLanguagesViewModel = _jobSeekerService.GetJobSeekerLanguages(id),
                LanguageLevel = _jobSeekerService.GetLanguageLevel(),
                SkillLevel = _jobSeekerService.GetSkillLevel(),
                Genders = _jobSeekerService.GetGender(),
                Ethnicities = _jobSeekerService.GetEthnicity(),
                JobAdvertId = jobAdvertId

            };

            //Clipboard.SetText("Test");
            viewApplicantProfileViewModel.JobSeekerPersonalDetailsViewModel.CityId = 
            _cityService.Get(viewApplicantProfileViewModel.JobSeekerPersonalDetailsViewModel.CityId).Name;
            return View("AdminRecruiterAppliedJobDetails",viewApplicantProfileViewModel);
        }
        
        [HttpGet]
        public ActionResult JobSeekerJobDetails(string id)
        {
            var jobDetails = _jobAdvertService.GetDetailedJob(id);
            
            if(_jobAdvertService.IsAlreadyApplied(jobDetails.JobAdvertId, HttpContext.Session.Get<string>("JobSeekerId")))
            {
                jobDetails.IsAlreadyAppliedBySeeker = true;
            }

            return View("JobSeekerJobAdvertDetails",jobDetails);
        }

        // GET: HomeController/Edit/5
        [HttpGet]
        public ActionResult Edit(string id)
        {
            
            //Call the method to retrieve ana advert
            return View(PrepareJobAdvertEdit(id));
        }

        private EditJobAdvertViewModel PrepareJobAdvertEdit(string id)
        {
            var advert = _jobAdvertService.Get(id);

            EditJobAdvertViewModel editJobAdvertViewModel = new EditJobAdvertViewModel
            {
                JobCategories = _jobCategoryService.GetAll(),
                JobTypes = _jobTypeService.GetAll(),
                ExperienceLevels = _experienceLevelService.GetAll(),
                Cities = _cityService.GetAll(),
                Caption = advert.Caption,
                Introduction = advert.Introduction,
                Experience = advert.Experience,
                Duties = advert.Duties,
                ExperienceLevelId = advert.ExperienceLevelId,
                CityId = advert.CityId,
                CompanyId = advert.CompanyId,
                SalaryFrom = advert.SalaryFrom,
                SalaryTo = advert.SalaryTo,
                JobTypeId = advert.JobTypeId,
                JobCategoryId = advert.JobCategoryId,
                Qualifications = advert.Qualifications,
                StartDate = (DateTime)advert.StartDate,
                EndDate = (DateTime)advert.EndDate,
                JobAdvertId = id
            };

            return editJobAdvertViewModel;
        }
        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditJobAdvertViewModel editJobAdvertViewModel,string id)
        {
            if (editJobAdvertViewModel.SalaryFrom > editJobAdvertViewModel.SalaryTo)
            {
                ModelState.AddModelError("SalaryTo","Salary From field cannot be greater than salary To");
            }
                        
            
            if (editJobAdvertViewModel.EndDate < editJobAdvertViewModel.StartDate)
            {
                ModelState.AddModelError("StartDate","Start Date field cannot be greater than End date");
            }

            if (ModelState.IsValid)
            {
                //Check if start date is greater than end date
                //Check if salary from is less than salary to

                TempData["AdvertEdit"] = "Successfully Edited";
                editJobAdvertViewModel.JobAdvertId = id;
                _jobAdvertService.Update(editJobAdvertViewModel);

                return RedirectToAction();
            }
            else
            {
                return View("Edit",PrepareJobAdvertEdit(id));
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Restful api
        [HttpGet]
        public IActionResult GetByCompanyId(string companyId)
        {
            var recruiters =  _recruiterService.GetByCompanyId(companyId);
            
            return Ok(recruiters);
        }
    }
}
