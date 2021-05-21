using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("jobseekerlanguages")]
    public class JobSeekerLanguages : HustlersEntity
    {
        public string JobSeekerId { set; get; }
        public string LanguageLevelId { set; get; }
        public string LanguageId { set; get; }
    }
}
