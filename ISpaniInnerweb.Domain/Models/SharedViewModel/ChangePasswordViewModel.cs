using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.SharedViewModel
{
    public class ChangePasswordViewModel
    {
        public string OldPassword { set; get; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword")]
        public string NewPassword { set; get; }
       
        [Required]
        [StringLength(10)]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { set; get; }
    }
}
