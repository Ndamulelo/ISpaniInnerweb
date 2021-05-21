using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.RecruiterViewModel
{
    public class ViewRecruiterViewModel
    { 
        public string CompanyName { set; get; }
        public string FullName { set; get; }
        public string Username { set; get; }
        public string Email { set; get; }
        public string RecruiterId { set; get; }
        public string CompanyId { set; get; }

    }
}
