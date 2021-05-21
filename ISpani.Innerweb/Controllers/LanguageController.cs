using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using ISpaniInnerweb.Domain.Models.LanguageViewModel;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ISpaniInnerweb.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ILanguageService languageService;
        private readonly IJobSeekerService jobSeekerService;

        public LanguageController(ILanguageService languageService, IJobSeekerService jobSeekerService)
        {
            this.languageService = languageService;
            this.jobSeekerService = jobSeekerService;
        }
        public IActionResult Index()
        {
            var addLanguageViewModel = new AddLanguageViewModel
            {
                LanguageLevels = jobSeekerService.GetLanguageLevel(),
                Languages = languageService.GetAll(),
                JobSeekerLanguagesViewModel = jobSeekerService.GetJobSeekerLanguages(HttpContext.Session.Get<string>("JobSeekerId"))

            };

            return View(addLanguageViewModel);
        }

        //Validate against user adding same language multiple times
        [HttpPost]
        public JsonResult Create(JobSeekerLanguagesViewModel jobSeekerLanguagesViewModel)
        {
            var response = new HustlersResponse<JobSeekerLanguagesViewModel>();
            jobSeekerLanguagesViewModel.JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId");
            //Check if language already exists
            if (jobSeekerService.IsJobSeekerLanguageExisting(jobSeekerLanguagesViewModel.LanguageId, jobSeekerLanguagesViewModel.JobSeekerId))
            {
                return Json(new Dictionary<string, string> { { "message","Language already exist"} });
            }
            //Create JobSeekerSkills
            languageService.Create(jobSeekerLanguagesViewModel);
            return Json(new Dictionary<string, string> { { "message", "Language Successfully Created" } });
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            languageService.Delete(id, HttpContext.Session.Get<string>("JobSeekerId"));
            return View("LanguageDeleteSuccess");
        }
    }
}
