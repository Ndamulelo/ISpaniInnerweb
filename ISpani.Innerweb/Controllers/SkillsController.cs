using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using ISpaniInnerweb.Domain.Models.SkillsViewModel;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ISpaniInnerweb.Controllers
{
    public class SkillsController : Controller
    {
        IJobSeekerService jobSeekerService;
        ISkillsService skillsService;

        public SkillsController(IJobSeekerService jobSeekerService,
        ISkillsService skillsService)
        {
            this.jobSeekerService = jobSeekerService;
            this.skillsService = skillsService;
        }
        public IActionResult Index()
        {
            var addSkillViewModel = new AddSkillViewModel
            {
                SkillLevels = jobSeekerService.GetSkillLevel(),
                Skills = skillsService.GetAll(),
                JobSeekerSkills = jobSeekerService.GetJobSeekerSkills(HttpContext.Session.Get<string>("JobSeekerId"))
            };
            return View(addSkillViewModel);
        }

        //Validate against user adding same skill multiple times
        [HttpPost]
        public JsonResult Create(JobSeekerSkillsViewModel jobSeekerSkillsViewModel)
        {
            var response = new HustlersResponse<JobSeekerSkillsViewModel>();
            jobSeekerSkillsViewModel.JobSeekerId = HttpContext.Session.Get<string>("JobSeekerId");
            //Create JobSeekerSkills
            if (jobSeekerService.IsJobSeekerSkillExisting(jobSeekerSkillsViewModel.SkillId, jobSeekerSkillsViewModel.JobSeekerId))
            {
                return Json(new Dictionary<string, string> { { "message", "Skill already exist" } });
            }
            //Check if skill already exists
            skillsService.Create(jobSeekerSkillsViewModel);
            return Json(new Dictionary<string, string> { { "message", "Skill Successfully Created" } });
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            skillsService.Delete(id, HttpContext.Session.Get<string>("JobSeekerId"));

            return View("SkillDeleteSuccess");
        }
    }
}
