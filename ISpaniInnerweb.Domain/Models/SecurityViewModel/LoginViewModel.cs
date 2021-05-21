using System.ComponentModel.DataAnnotations;

namespace ISpaniInnerweb.Domain.Models.SecurityViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Uname { set; get; }
        [Required]
        public string Pword { set; get; }
    }
}
