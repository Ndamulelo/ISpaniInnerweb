using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
    //[Table("user")]
    public class User : HustlersEntity
    {
        [Required]
        [StringLength(30)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
                        ErrorMessage = "Invalid email format")]
        public string Email { set; get; }
        public string RoleName { set; get; } //Admin, JobSeeker, Recruiter
        public string UserId { set; get; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string Password { set; get; }
        [NotMapped]
        [Required]
        [StringLength(10)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { set; get; }
    }
}
