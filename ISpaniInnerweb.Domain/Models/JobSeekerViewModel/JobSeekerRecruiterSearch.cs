using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerRecruiterSearch
    {
        public string JobSeekerId { set; get; }
        public string Address { set; get; } //City, StreetName, StreetNumber
        public string Insitution { set; get; }
        public string JobSeekerFullName { set; get; } //FirstName & LastName
        public string Email { set; get; }
        public string FieldOfStudy { set; get; }
        public int GraduationYear { set; get; }
        public string Qualification { set; get; } //Bsc etc   
    }
}
