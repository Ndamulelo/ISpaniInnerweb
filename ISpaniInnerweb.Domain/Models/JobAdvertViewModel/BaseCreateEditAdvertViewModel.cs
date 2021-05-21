using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobAdvertViewModel
{
    public class BaseCreateEditAdvertViewModel
    {
        public IList<ExperienceLevel> ExperienceLevels;
        public IList<JobType> JobTypes;
        public IList<JobCategory> JobCategories;
        public IList<City> Cities;
        [Required]
        [StringLength(350)]
        public string Introduction { set; get; }
        [Required]
        [StringLength(1000)]
        public string Experience { set; get; }
        [Required]
        [StringLength(1000)]
        public string Duties { set; get; }
        [Required]
        [StringLength(50)]
        public string Caption { set; get; }
        public string ExperienceLevelId { set; get; }
        public string JobCategoryId { set; get; }
        public string JobTypeId { set; get; }
        public string CompanyId { set; get; }
        public string CityId { set; get; }
        public string RecruiterId { set; get; }
        //[RegularExpression(@"^(0(?!\.00)|[1-9]\d{0,6})\.\d{2}$", ErrorMessage = "Invalid salary 3500.50")] [Range(0, 999.99)]
        [Required(ErrorMessage = "Salary From is Required")]
        //[Range(0, 1000000)]
        public long SalaryFrom { set; get; }
        [Required(ErrorMessage ="Salary To is Required")]
        //[Range(1, 1000000)]
        public long SalaryTo { set; get; }

        [Required]
        [StringLength(1000)]
        public string Qualifications { set; get; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { set; get; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { set; get; }
    }
}
