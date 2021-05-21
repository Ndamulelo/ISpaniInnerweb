using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
   // [Table("ethnicity")]
    public class Ethnicity : HustlersEntity
    {
        public string EthnicityType { set; get; }
    }
}
