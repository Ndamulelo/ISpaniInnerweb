using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IJobCategoryService
    {
        void Create(JobCategory jobCategory);
        void Update(JobCategory jobCategory);
        JobCategory Get(string id);
        IList<JobCategory> GetAll();
    }
}
