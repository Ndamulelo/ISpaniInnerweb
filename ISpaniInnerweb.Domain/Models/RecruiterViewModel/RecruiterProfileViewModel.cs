using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.RecruiterViewModel
{
    public class RecruiterProfileViewModel
    {
        public string CompanyName { set; get; }
        public string CompanyId { set; get; }
        [Required]
        [StringLength(30)]
        public string FisrtName { set; get; }
        [Required]
        [StringLength(30)]
        public string LastName { set; get; }
        [Required]
        [StringLength(10, ErrorMessage = "Telephone cannot exceed 10 digits")]
        [MinLength(10, ErrorMessage = "Telephone must 10 digits")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { set; get; }
    }
}
