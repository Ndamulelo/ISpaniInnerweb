using ISpaniInnerweb.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISpaniInnerweb.Domain.Models.RecruiterViewModel
{
    public class EditRecruiterViewModel : BaseRecruiterViewModel
    {
        public string RecruiterId { set; get; }
    }
}
