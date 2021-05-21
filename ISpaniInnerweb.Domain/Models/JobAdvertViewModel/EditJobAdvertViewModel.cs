using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.JobAdvertViewModel
{
    public class EditJobAdvertViewModel : BaseCreateEditAdvertViewModel
    {
      
        public string CompanyName { set; get; }              
        public string JobAdvertId { set; get; }

    }
}
