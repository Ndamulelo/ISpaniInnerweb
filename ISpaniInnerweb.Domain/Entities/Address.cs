using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("address")]
    public class Address : HustlersEntity
    {
        public string StreetName { set; get; }
        public string StreetNumber { set; get; }
        public string BuildingNumber { set; get; }
    }
}
