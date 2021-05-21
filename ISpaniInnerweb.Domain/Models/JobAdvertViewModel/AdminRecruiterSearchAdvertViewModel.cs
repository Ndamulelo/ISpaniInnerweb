using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobAdvertViewModel
{
    public class AdminRecruiterSearchAdvertViewModel
    {
        public List<ViewJobAdvertViewModel> ViewJobAdvertViewModel { set; get; }
        public List<Company> Companies { set; get; }
        public List<JobType> JobTypes { set; get; }
        public List<JobCategory> JobCategories { set; get; }
    }
}
