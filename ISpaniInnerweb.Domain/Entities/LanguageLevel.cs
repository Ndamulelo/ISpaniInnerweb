using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("languagelevel")]
    public class LanguageLevel : HustlersEntity
    {
        public string DifficultyLevel { set; get; }
    }
}
