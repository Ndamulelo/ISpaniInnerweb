using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("qualification")]
    public class Qualification : HustlersEntity
    {
        public string Name { set; get; }
    }
}
