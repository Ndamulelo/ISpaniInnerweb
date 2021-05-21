using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("province")]
    public class Province : HustlersEntity
    {
        public string Name { set; get; }
    }
}
