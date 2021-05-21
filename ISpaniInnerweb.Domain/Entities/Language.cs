using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("language")]
    public class Language : HustlersEntity
    {
        public string Name { set; get; }
    }
}
