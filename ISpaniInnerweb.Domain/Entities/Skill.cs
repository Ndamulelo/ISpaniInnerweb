using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("skill")]
    public class Skill : HustlersEntity
    {
        public string Description { set; get; }
    }
}
