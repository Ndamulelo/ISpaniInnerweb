using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Infrastructure.Repositories
{
    //Extended Functionaliy for Company incase the Generic is Limited to perform some functions
    public class CompanyRepository : Repository<Company>,ICompanyRepository
    {
        public CompanyRepository(HustlersContext context):base(context)
        {
            
        }

        public IList<Company> GetWithRelatedEntities()
        {
            var courses = context.Company.Include(c => c.CityId).ToList();
            return courses;
        }
    }
}
