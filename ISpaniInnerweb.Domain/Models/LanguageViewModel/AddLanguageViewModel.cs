using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.LanguageViewModel
{
    public class AddLanguageViewModel
    {
        public IList<Language> Languages { set; get; }
        public IList<JobSeekerLanguagesViewModel> JobSeekerLanguagesViewModel { set; get; }
        public IList<LanguageLevel> LanguageLevels { set; get; }
    }
}
