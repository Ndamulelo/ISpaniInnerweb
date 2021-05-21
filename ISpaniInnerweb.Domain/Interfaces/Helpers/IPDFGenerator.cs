using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Helpers
{
    public interface IPDFGenerator
    {
        string TestStringBuilder();
        string BuildJobHistoryReport (IList<JobSeekerApplicationHistory> GetJobSeekerApplicationHistory); //Build jobApplicant job history report
    }
}
