using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using ISpaniInnerweb.Domain.Services;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ISpaniInnerweb.Controllers
{
    public class WorkExperienceController : Controller
    {
        private readonly IWorkExperienceService workExperienceService;
        private readonly IJobCategoryService jobCategoryService;
        private readonly ILogger<WorkExperienceController> logger;

        public WorkExperienceController(IWorkExperienceService workExperienceService, IJobCategoryService jobCategoryService,
            ILogger<WorkExperienceController> logger)
        {
            this.workExperienceService = workExperienceService;
            this.jobCategoryService = jobCategoryService;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            var jobSeekerAddExperienceViewModel = new JobSeekerAddExperienceViewModel
            {
                JobCategories = jobCategoryService.GetAll()
            };

            return View(jobSeekerAddExperienceViewModel);
        }

        [HttpPost]
        public IActionResult Create(JobSeekerAddExperienceViewModel jobSeekerAddExperienceViewModel)
        {
            var seekerId = HttpContext.Session.Get<string>("JobSeekerId");
            var fuck = workExperienceService.IsJobSeekerEmployed(seekerId);
            //If user has already existing job, throw an error
            if (workExperienceService.IsJobSeekerEmployed(seekerId) == false && jobSeekerAddExperienceViewModel.JobSeekerExperienceViewModel.IsCurrentCompany)
            {
                ModelState.AddModelError("CompanyName","You can't have more than two current companies");
            }
            
            //https://www.findanaccountant.co.za/content_company-name-criteria

            //call create method.
            if (ModelState.IsValid)
            {
                TempData["WorkExperienceCreate"] = "Work Experience Created Successfully";
                jobSeekerAddExperienceViewModel.JobSeekerExperienceViewModel.JobSeekerId = seekerId;
                workExperienceService.Create(jobSeekerAddExperienceViewModel.JobSeekerExperienceViewModel);
                //Redirect to success page
                logger.LogInformation("Created Experience successfully");
                return View("Index", new JobSeekerAddExperienceViewModel { JobCategories = jobCategoryService.GetAll() });
            }

            return View("Index", new JobSeekerAddExperienceViewModel { JobCategories = jobCategoryService.GetAll()});
        }

        //ExperienceDeleteSuccess
        [HttpGet]
        public IActionResult Delete(string id)
        {
            workExperienceService.Delete(id, HttpContext.Session.Get<string>("JobSeekerId"));
            return View("ExperienceDeleteSuccess");
        }
    }
}
