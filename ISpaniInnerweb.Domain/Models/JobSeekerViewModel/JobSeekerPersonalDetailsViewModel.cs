using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobSeekerViewModel
{
    public class JobSeekerPersonalDetailsViewModel :BaseJobSeekerViewModel
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string IdNumber { set; get; }
        [StringLength(5)]
        public string Phone { set; get; }
        public string GenderId { set; get; }
        public string TitleId { set; get; }
        public string EthnicityId { set; get; }
        public string ProvinceId { set; get; }
        public string AddressId { set; get; }
        public string CityId { set; get; }
        public string PersonalProfile { set; get; }
        public string StreetName { set; get; }
        public string StreetNumber { set; get; }
        public string BuildingNumber { set; get; }
        public string Email { set; get; }
        public string CVUrl { set; get; }
        public string AcademicRecordUrl { set; get; }
        public string IdUrl { set; get; }
    }
}