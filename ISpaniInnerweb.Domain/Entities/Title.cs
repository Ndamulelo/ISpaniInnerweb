using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("title")]
    public class Title : HustlersEntity
    {
        public string TitleName { set; get; }
    }
}
