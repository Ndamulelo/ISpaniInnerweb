using System;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Interfaces.Services.Communication;
using ISpaniInnerweb.Domain.Models.SecurityViewModel;
using ISpaniInnerweb.Domain.Models.SharedViewModel;
using ISpaniInnerweb.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ISpaniInnerweb.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRecruiterService _recruiterService;
        private readonly IJobSeekerService _JobSeekerService;
        private readonly IEmailService  _emailService;
        private ILogger<UserController> _logger;
        public UserController(IUserService userService, IRecruiterService recruiterService
            ,IJobSeekerService jobSeekerService, ILogger<UserController> logger, IEmailService emailService)
        {
            _userService = userService;
            //_session = session;
            _recruiterService = recruiterService;
            _JobSeekerService = jobSeekerService;
            _logger = logger;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session;

            if (session.IsAvailable)
            {
                var role = HttpContext.Session.Get<string>("Role");

                if (role == null)
                {
                    return View();
                }

                if (role.Equals("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if(role.Equals("Recruiter"))
                { 
                    return RedirectToAction("Index", "Recruiter");
                }
                else if(role.Equals("JobSeeker"))
                {
                    return RedirectToAction ("Index", "JobSeeker");
                }
            }

            return View();
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Auth(User userModel,LoginViewModel loginModel)
        {
            //Return a relevant admin page based on the type of user logging in
            //Do not forget to encrypt a password
            var controller = "";
            HttpContext.Session.Clear();
           
            var authUser = _userService.AuthUser(userModel);
            
            //HttpContext.Session.Clear();
            if (authUser != null)
            {
                //Set a session
                //_sessionManager.Set
                if(authUser.RoleName.Equals("Admin") && userModel.RoleName.Equals("Admin"))
                {
                    HttpContext.Session.Set<string>("AdminId",authUser.Id);
                    HttpContext.Session.Set<string>("Role", authUser.RoleName);
                    HttpContext.Session.Set<string>("Username", authUser.Email);
                    controller = "Admin";
                }

                if(authUser.RoleName.Equals("Recruiter") && userModel.RoleName.Equals("Recruiter"))
                {
                    //var recruiter =_recruiterService.Get(authUser.UserId);
                    HttpContext.Session.Set<string>("RecruiterId", authUser.UserId);
                    HttpContext.Session.Set<string>("Role", authUser.RoleName);
                    HttpContext.Session.Set<string>("Username", authUser.Email);
                    controller = "Recruiter";
                }

                if (authUser.RoleName.Equals("JobSeeker") && userModel.RoleName.Equals("JobSeeker"))
                {
                    //var jobSeeker = _JobSeekerService.Get(authUser.UserId);
                    HttpContext.Session.Set<string>("JobSeekerId", authUser.UserId);
                    HttpContext.Session.Set<string>("Role", authUser.RoleName);
                    HttpContext.Session.Set<string>("Username", authUser.Email);
                    controller = "JobSeeker";
                }
            }
            TempData["Credentials"] = "Wrong Credentials";
            _logger.LogInformation(HttpContext.Session.Get<string>("Role"));
            return RedirectToAction("Index", controller);

        }

        [HttpPost]
        public ActionResult SignUp(User userModel)
        {
            
            if (ModelState.IsValid)
            {
                userModel.RoleName = "JobSeeker";
                userModel.UserId = Guid.NewGuid().ToString();
                bool isJobSeekerRegistered = _userService.IsUserRegistered(userModel);

                if (isJobSeekerRegistered)
                {
                    _emailService.SendMail("RecruitmentJob Registration Success",
                        "Good day\nWelcome to RecruitmentJob,\nYour username is "+userModel.Email+" \nYour password is " + userModel.Password, userModel.Email);
                    TempData["SignUpSucess"] = "Successfully registered, Proceed to login";
                    return RedirectToAction(nameof(Index));
                }
                else
                 {
                    TempData["SignUpSucess"] = "Email already Taken";
                }
            }
            
            return View(nameof(Index));
        }

        public ActionResult SignOut()
        {
            HttpContext.Session.Clear();
            TempData.Clear();
            return RedirectToAction(nameof(Index));
        }
        //Clear all the sessions
        public ActionResult JobSeekerRegistered()
        {
            ViewBag.Registered = "User successfully registered";
            return View();
        }        
        
        [HttpGet]
        public ActionResult ChangePassword()
        {
            
            return View("ChangePassword");
        }
        
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var role = HttpContext.Session.Get<string>("Role");
            //Call change password service
            var userId = "";

            if (role.Equals("JobSeeker"))
            {
                userId = HttpContext.Session.Get<string>("JobSeekerId");
            }
            else if (role.Equals("Admin"))
            {
                userId = HttpContext.Session.Get<string>("AdminId");
            }
            else
            {
                userId = HttpContext.Session.Get<string>("RecruiterId");
            }

            if (!_userService.PasswordMatches(changePasswordViewModel,userId))
            {
                ModelState.AddModelError("OldPassword","Current password is Wrong");
            }
            
            if(ModelState.IsValid)
            {
                _userService.ChangePassword(changePasswordViewModel, userId);
                TempData["PasswordChanged"] = "Password Successfully Changed";
            }

            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if(_userService.IsEmailRegistered(email) == false)
            {
                ModelState.AddModelError("email", "Email does not exist");
            }

            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByEmail(email);

                _emailService.SendMail("RecruitmentJob Password Request", "Your password is : " + user.Password, email);
                TempData["ForgotPassword"] = "Password successfully sent to " + email;
            }

            return View();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

    }
}
