using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobAdvertViewModel
{
    public class JobSeekerSearchAdvertViewModel
    {
        public List<ViewJobAdvertViewModel> ViewJobAdvertViewModel { set; get; }
        public List<JobType> JobTypes { set; get; }
        public List<JobCategory> JobCategories { set; get; }
        public List<City> JobCities { set; get; }
    }
}
