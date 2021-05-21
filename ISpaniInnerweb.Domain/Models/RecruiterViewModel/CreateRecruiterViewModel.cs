using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.RecruiterViewModel
{
    public class CreateRecruiterViewModel : BaseRecruiterViewModel
    {

        [Required]
        [StringLength(10)]
        [DataType(DataType.Password)]
        public string Password { set; get; }
        [Required]
        [StringLength(10)]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword")]
        public string ConfirmPassword { set; get; }
    }
}
