using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.EducationViewModel;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ISpaniInnerweb.Controllers
{
    public class EducationController : Controller
    {
        private readonly ILogger<EducationController> logger;
        private readonly IEducationService educationService;
        private readonly IJobSeekerService jobSeekerService;
        public EducationController(ILogger<EducationController> logger, IEducationService educationService, IJobSeekerService jobSeekerService)
        {
            this.educationService = educationService;
            this.jobSeekerService = jobSeekerService;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            var jobSeekerEducationViewModelContainer = new JobSeekerEducationViewModelContainer
            {
                Qualifications = jobSeekerService.GetQualification()
            };
            return View(jobSeekerEducationViewModelContainer);
        }

        [HttpPost]
        public IActionResult Create(JobSeekerEducationViewModelContainer jobSeekerEducationViewModelContainer)
        {
            var seekerId = HttpContext.Session.Get<string>("JobSeekerId");

            //call create method.
            if (ModelState.IsValid)
            {
                TempData["EducationCreate"] = "Education Create Successfully";
                jobSeekerEducationViewModelContainer.JobSeekerEducationViewModel.JobSeekerId = seekerId;
                educationService.Create(jobSeekerEducationViewModelContainer.JobSeekerEducationViewModel);
                //Redirect to success page
                logger.LogInformation("Created Education successfully");
                return View("Index", new JobSeekerEducationViewModelContainer { Qualifications = jobSeekerService.GetQualification() });
            }

            return View("Index", new JobSeekerEducationViewModelContainer { Qualifications = jobSeekerService.GetQualification() });
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            educationService.Delete(id, HttpContext.Session.Get<string>("JobSeekerId"));
            return View("EducationDeleteSuccess");
        }
    }
}
