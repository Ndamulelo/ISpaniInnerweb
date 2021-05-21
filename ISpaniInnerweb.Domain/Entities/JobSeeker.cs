﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("jobseeker")]
    public class JobSeeker : HustlersEntity
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string IdNumber { set; get; }
        public string Phone { set; get; }
        public DateTime? DateOfBirth { set; get; } //Extract it from an IdNumber
        public string GenderId { set; get; }
        public string TitleId { set; get; }
        public string EthnicityId { set; get; }
        //public string IsProfileVisible { set; get; }
        public string PersonalProfile { set; get; }
        public string AddressId { set; get; }
        public string CityId { set; get; }
        public string ProvinceId { set; get; }
        public List<Education> Education { set; get; }
        public List<Attachment> Attachment { set; get; }
        public List<WorkExperience> WorkExperience { set; get; }
        public List<JobSeekerJobApplications> JobSeekerJobApplications { set; get; } //Create Association Table for skill and skill level and 
        public List<JobSeekerSkills> JobSeekerSkills { set; get; } //Create Association Table for skill and skill level and 
        public List<JobSeekerLanguages> JobSeekerLanguages { set; get; } //Create Association Table
        //public List<Qualification> Qualification { set; get; }


    }
}
