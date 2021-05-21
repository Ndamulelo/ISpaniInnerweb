using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobAdvertViewModel
{
    public class CreateJobAdvertViewModel : BaseCreateEditAdvertViewModel
    {
        public IList<Company> Companies;
        public IList<Recruiter> Recruiters;
    }
}
