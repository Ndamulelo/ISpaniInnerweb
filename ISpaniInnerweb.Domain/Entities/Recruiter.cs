using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISpaniInnerweb.Domain.Entities
{
   // [Table("recruiter")]
    public class Recruiter : HustlersEntity
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Phone { set; get; }
        public string CompanyId { get; set; }
    }
}
