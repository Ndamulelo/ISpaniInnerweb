using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Helpers;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Interfaces.Services.Communication;
using ISpaniInnerweb.Domain.Models.JobAdvertViewModel;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

            return View("InterviewInviteSuccess"); 
        }

        [HttpGet]
        public IActionResult Interviews()
        {
            return View(_jobSeekerService.GetInterviews(HttpContext.Session.Get<string>("RecruiterId")));
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
        
        private string BuildRecruiterViewAppliedJobsReport()
        {
            var appliedJobs = _recruiterService.GetAppliedJobs(HttpContext.Session.Get<string>("RecruiterId"));
            var sb = new StringBuilder();
            int pending = 0, interviewed = 0, declined = 0;

            sb.Append(@"<div class='card'>
            <div class='card-header header-elements-inline'>
                <h5 class='card-title'>Recruiter Job Applications History</h5>
            </div>

            <div class='table-responsive'>
                <table class='table'>
                    <thead>
                        <tr>

                            <th>Date</th>
                            <th>ApplicantName</th>
                            <th>Contacts</th>
                            <th>Job</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                    ");  
            
            foreach (var item in appliedJobs)
            {
                
                if (item.ApplicationStatus.Equals("Pending"))
                {
                    pending = pending + 1;
                }
                else if (item.ApplicationStatus.Equals("Declined"))
                {
                    declined = declined + 1;
                }
                else
                {
                    interviewed = interviewed + 1;
                }
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2} {3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                  </tr>",
                    item.DateCreated.ToString("yyyy-MM-dd"),
                    item.ApplicantName,
                    item.ApplicantPhone,
                    item.Email,
                    item.JobCaption,
                    item.ApplicationStatus);
       
                }
            sb.AppendFormat(@"<tr>
                                <td>Total Jobs Applied:</td>
                                <td>{0}</td>
                            </tr>", appliedJobs.Count);

            sb.AppendFormat(@"<tr>
                                <td>Pending Job Application:</td>
                                <td>{0}</td>
                            </tr>", pending);

            sb.AppendFormat(@"<tr>
                                <td>Interviewed Job Application:</td>
                                <td>{0}</td>
                            </tr>", interviewed);

            sb.AppendFormat(@"<tr>
                                <td>Declined Job Application:</td>
                                <td>{0}</td>
                            </tr>", declined);

            sb.Append(@"</ tbody>
                </ table >
            </ div >
        </ div >");

            return sb.ToString();
        }

        /*public IActionResult ExportRecruiterAppliedJobsReport()
        {
            var Renderer = new IronPdf.HtmlToPdf();
            //IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            // add a header to very page easily
            Renderer.PrintOptions.FirstPageNumber = 1;
            Renderer.PrintOptions.Header.DrawDividerLine = true;
            Renderer.PrintOptions.Header.CenterText = "{url}";
            Renderer.PrintOptions.Header.FontFamily = "Helvetica,Arial";
            Renderer.PrintOptions.Header.FontSize = 12;
            Renderer.PrintOptions.CustomCssUrl = webHostEnvironment.WebRootPath + "\\css\\bootstrap.css";
            // add a footer too
            Renderer.PrintOptions.Footer.DrawDividerLine = true;
            Renderer.PrintOptions.Footer.FontFamily = "Arial";
            Renderer.PrintOptions.Footer.FontSize = 10;
            Renderer.PrintOptions.Footer.LeftText = "{date} {time}";
            Renderer.PrintOptions.Footer.RightText = "{page} of {total-pages}";
            var file = Renderer.RenderHtmlAsPdf(BuildRecruiterViewAppliedJobsReport());
            var contentLength = file.BinaryData.Length;

            return File(file.BinaryData, "application/pdf;");
        }*/

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
