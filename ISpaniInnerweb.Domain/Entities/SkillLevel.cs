using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("skilllevel")]
    public class SkillLevel : HustlersEntity
    {
        public string Level { set; get; }
    }
}
