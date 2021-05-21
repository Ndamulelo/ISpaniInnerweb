using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.EducationViewModel
{
    public class JobSeekerEducationViewModelContainer
    {
        public JobSeekerEducationViewModel JobSeekerEducationViewModel { set; get; }
        public IList<Qualification> Qualifications { set; get; }
    }
}
