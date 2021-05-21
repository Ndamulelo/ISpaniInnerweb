using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IJobTypeService
    {
        void Create(JobType jobType);
        void Update(JobType jobType);
        JobType Get(string id);
        IList<JobType> GetAll();
    }
}
