using ISpaniInnerweb.Domain.Interfaces.Helpers;
using ISpaniInnerweb.Domain.Models.JobSeekerViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ISpaniInnerweb.Infrastructure.Helpers
{
    public class PDFGenerator : IPDFGenerator
    {
        public string BuildJobHistoryReport(IList<JobSeekerApplicationHistory> jobSeekerApplicationHistory)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>#</th>
                                        <th>Date</th>
                                        <th>Job Title</th>
                                        <th>Company Name</th><!--email+phone-->
                                        <th>Status</th>
                                    </tr>");
            foreach (var jobApplication in jobSeekerApplicationHistory)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>",
                                  jobApplication.CreatedDate,
                                  jobApplication.JobTtitle,
                                  jobApplication.Company,
                                  jobApplication.Status);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }

        public string TestStringBuilder()
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>#</th>
                                        <th>Date</th>
                                        <th>Job Title</th>
                                        <th>Company Name</th><!--email+phone-->
                                        <th>Status</th>
                                    </tr>");
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
