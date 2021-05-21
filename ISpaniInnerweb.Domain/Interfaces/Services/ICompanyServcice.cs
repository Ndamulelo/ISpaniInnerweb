using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models.CompanyViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface ICompanyService
    {
        void Create(CreateCompanyViewModel createCompanyViewModel);
        void Update(EditCompanyViewModel editCompanyViewModel);
        //void UpdateById(string id);
        Company Get(string id);
        IList<Company> GetAll();
        IList<Company> GetAllWithRelations();
        void Delete(string id);
    }
}
