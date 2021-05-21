using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
   // [Table("experiencelevel")]
   //NB: Its for when adding an advert only. Exclude it on the seeker experience
    public class ExperienceLevel : HustlersEntity
    {
        public string Description { set; get; }
    }
}
