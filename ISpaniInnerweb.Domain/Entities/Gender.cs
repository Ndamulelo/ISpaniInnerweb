using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("gender")]
    public class Gender : HustlersEntity
    {
        public string Type { set; get; }
    }
}
