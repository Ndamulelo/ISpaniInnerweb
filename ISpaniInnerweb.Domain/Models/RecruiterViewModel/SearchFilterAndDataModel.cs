using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Models.RecruiterViewModel
{
    public class SearchFilterAndDataModel
    {
        public List<ViewRecruiterViewModel> ViewRecruiterViewModel { set; get; }
        public List<Company> Companies { set; get; }
    }
}
