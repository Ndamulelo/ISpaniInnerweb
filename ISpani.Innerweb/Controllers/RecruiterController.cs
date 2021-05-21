using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Interfaces.Services.Communication;
using ISpaniInnerweb.Domain.Models.RecruiterViewModel;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace ISpaniInnerweb.Controllers
{
    public class RecruiterController : Controller
    {
        private readonly IRecruiterService _recruiterService;
        private readonly ICompanyService _companyService;
        private ILogger<RecruiterController> _logger;
        private readonly IJobSeekerService _jobSeekerService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment webHostEnvironment;
        public RecruiterController(IRecruiterService recruiterService, ILogger<RecruiterController> logger, IJobSeekerService jobSeekerService,
            ICompanyService companyService, IEmailService emailService, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _recruiterService = recruiterService;
            this.webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
            _companyService = companyService;
            _jobSeekerService = jobSeekerService;
            _userService = userService;
            _logger = logger;
        }
        // GET: RecruiterController
        public ActionResult Index()
        {
            //Get User using id saved on a session when they login

            return View("Dashboard",_jobSeekerService.RecruiterDashboardStats(HttpContext.Session.Get<string>("RecruiterId")));
        }

        [HttpGet]
        public ActionResult AdminViewRecruiter(string CompanyId)
        {
            //Get User using id saved on a session when they login
            
            //Load Companies

            var companies = _companyService.GetAll();
            //companies.Insert(0,new Company {CompanyName = "All",Id = "0" });
            var recruiters = _recruiterService.GetAll(CompanyId);

            SearchFilterAndDataModel searchFilterAndDataModel = new SearchFilterAndDataModel
            {
                ViewRecruiterViewModel = (List<ViewRecruiterViewModel>)recruiters,
                Companies = (List<Company>)companies

            };
            return View("AdminViewRecruiter", searchFilterAndDataModel);
        }

        private string BuildRecruiterListReport()
        {
            var sb = new StringBuilder();
            var recruiters = _recruiterService.GetAll(null);

            sb.Append(@"<div class='card'>
            <div class='card-header header-elements-inline'>
                <h5 class='card-title'>List of Job Seekers Report</h5>
            </div>

            <div class='table-responsive'>
                <table class='table'>
                    <thead>
                        <tr>

                            <th>Full Name</th>
                            <th>Email</th>
                            <th>Company</th>
                        </tr>
                    </thead>
                    <tbody>
                    ");

            foreach (var item in recruiters)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                  </tr>",
                   item.FullName,
                   item.Email,
                   item.CompanyName);

            }

            sb.AppendFormat(@"<tr>
                                <td>Total Recruiters:</td>
                                <td>{0}</td>
                            </tr>", recruiters.Count);

            sb.Append(@"</ tbody>
                </ table >
            </ div >
        </ div >");

            return sb.ToString();
        }

        /*public IActionResult ExportRecruiterReport()
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
            var file = Renderer.RenderHtmlAsPdf(BuildRecruiterListReport());
            var contentLength = file.BinaryData.Length;

            return File(file.BinaryData, "application/pdf;");
        }*/

        // GET: RecruiterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RecruiterController/Create
        public ActionResult Create()
        {
            CreateRecruiterViewModel createRecruiterViewModel = new CreateRecruiterViewModel
            {
                Companies = _companyService.GetAll()
            };

            return View("AdminCreate", createRecruiterViewModel);
        }

        // POST: RecruiterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRecruiterViewModel createRecruiterViewModel)
        {
            
            CreateRecruiterViewModel createRecruiterModel = new CreateRecruiterViewModel
            {
                Companies = _companyService.GetAll(),
            };

            if(_userService.IsEmailRegistered(createRecruiterViewModel.Email))
            {
                ModelState.AddModelError("Email","Email already exist, use a different one");
            }

            if (ModelState.IsValid)
            {     
                _recruiterService.Create(createRecruiterViewModel);
                _emailService.SendMail("Recruiter Registration Success \n",
                    "Your account has been created successfully \n" +
                    "Find your login credentials\nEmail: " + createRecruiterViewModel.Email + 
                    "\nPassword: "+ createRecruiterViewModel.Password, createRecruiterViewModel.Email);
                TempData["CreateRecruiter"] = "Recruiter Successfully Registered";
                return View("AdminCreate", createRecruiterModel);
            }

            return View("AdminCreate", createRecruiterModel);
        }

        // GET: RecruiterController/Edit/5
        public ActionResult Edit(string id)
        {

            return View("AdminEdit", PrepareRecruiterEdit(id));
        }

        private EditRecruiterViewModel PrepareRecruiterEdit(string id)
        {
            var recruiter = _recruiterService.Get(id);

            var editRecruiterViewModel = new EditRecruiterViewModel
            {
                FirstName = recruiter.FirstName,
                LastName = recruiter.LastName,
                Companies = _companyService.GetAll(),
                CompanyId = recruiter.CompanyId,
                Phone = recruiter.Phone,
                RecruiterId = id
            };

            return editRecruiterViewModel;
        }
        // POST: RecruiterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditRecruiterViewModel editRecruiterViewModel, string id)
        {
            if (ModelState.IsValid)
            {
                editRecruiterViewModel.RecruiterId = id;
                _recruiterService.Update(editRecruiterViewModel);
                TempData["RecruiterUpdateMessage"] = "Updated Successfully";
                return View("AdminEdit", PrepareRecruiterEdit(id));
            }
            else
            {
                return View("AdminEdit", PrepareRecruiterEdit(id));
            }
        }

        // GET: RecruiterController/Delete/5
        [HttpGet]
        public ActionResult DeleteRec(string id)
        {
            TempData["RecruiterDeleted"] = "Recruiter Is Deleted";
            _recruiterService.Delete(id);
            return RedirectToAction("AdminViewRecruiter");
        }

        // POST: RecruiterController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            _recruiterService.Delete(id);

            return RedirectToAction("AdminViewRecruiter");
        }

        [HttpGet]
        public ActionResult Profile()
        {
            return View("Profile",_recruiterService.GetProfileData(HttpContext.Session.Get<string>("RecruiterId")));
        }        
        
        [HttpPost]
        public ActionResult Profile(RecruiterProfileViewModel recruiterProfileViewModel)
        {
            var recruiterId = HttpContext.Session.Get<string>("RecruiterId");

            if (ModelState.IsValid)
            {
                var editRecruiterModel = new EditRecruiterViewModel
                {
                    CompanyId = recruiterProfileViewModel.CompanyId,
                    FirstName = recruiterProfileViewModel.FisrtName,
                    LastName = recruiterProfileViewModel.LastName,
                    Phone = recruiterProfileViewModel.Phone,
                    RecruiterId = recruiterId
                };
                _recruiterService.Update(editRecruiterModel);
                TempData["RecruiterProfile"] = "Profile Successfully Updated";
            }
            return View("Profile",_recruiterService.GetProfileData(recruiterId));
        }
    }
}
