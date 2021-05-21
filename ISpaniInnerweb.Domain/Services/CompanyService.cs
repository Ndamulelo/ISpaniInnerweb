using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.CompanyViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class CompanyService : ICompanyService
    {
        IRepository<Company> _companyRepository;
        ICompanyRepository _companyRepositoryExtended;
        IAddressService _addressService;
        ICityService _cityService;
        IProvinceService _provinceService;

        ILogger<CompanyService> logger;

        public CompanyService(ILogger<CompanyService> logger, IRepository<Company> companyRepository,
           IAddressService addressService, IProvinceService provinceService, ICityService cityService,
           ICompanyRepository companyRepositoryExtended)
        {
            _companyRepository = companyRepository;
            _companyRepositoryExtended = companyRepositoryExtended;
            _addressService = addressService;
            _provinceService = provinceService;
            _cityService = cityService;
            this.logger = logger;
        }
        public void Create(CreateCompanyViewModel createCompanyViewModel)
        {
            //Should have called their service, not repo directly

            //Create Address
            var address = new Address
            {
                Id = Guid.NewGuid().ToString(),
                StreetName = createCompanyViewModel.StreetName,
                StreetNumber = createCompanyViewModel.StreetNumber,
                BuildingNumber = createCompanyViewModel.BuildingNumber
            };
            _addressService.Create(address);

            //Create company
            //var compan = new Company { AddressId = "" };
            var company = new Company
            {
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                CompanyName = createCompanyViewModel.CompanyName,
                Telephone = createCompanyViewModel.Telephone,
                ProvinceId = createCompanyViewModel.ProvinceId,
                AddressId = address.Id,
                CityId = createCompanyViewModel.CityId
            };

            _companyRepository.Insert(company);
        }
        //
        /**
         * Deleting a company causes problems. Recruiter belongs to a company and a recruiter cannot exist
         * without a company. JobAdverts are linked to a company and a recruiter, job advert cannot exist withou
         *a company. Job Seeker applies for a job that is of a certain company, which is assigned to a certain recruiter
         *Job Seeker has a history of applied jobs. If we delete a company, we must delete job application history for
         *a job seeker as well, all the adverts associated with a company and all the recruiters associated with it.
         *otherwise a system will break trying to find adverts, history of a company that does not exist.
         * */
        public void Delete(string id)
        {
            var company = _companyRepository.Get(id);

            throw new NotImplementedException();
        }

        public Company Get(string id)
        {

            var company = new Company();

            try
            {
                company = _companyRepository.Get(id);
                logger.LogInformation("Company " + company.Id + "retrieved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return company;
        }

        public IList<Company> GetAll()
        {
            var companies = _companyRepository.Get();

            return companies;
        }

        public IList<Company> GetAllWithRelations()
        {
            return _companyRepositoryExtended.GetWithRelatedEntities();
        }

        public void Update(EditCompanyViewModel editCompanyViewModel)
        {
            var companyToUpdate = _companyRepository.Get(editCompanyViewModel.CompanyId);
            var address = _addressService.Get(companyToUpdate.AddressId);
            address.BuildingNumber = editCompanyViewModel.BuildingNumber;
            address.StreetName = editCompanyViewModel.StreetName;
            address.StreetNumber = editCompanyViewModel.StreetNumber;

            _addressService.Update(address);
            //Update an address 
            //companyToUpdate.AddressId = editCompanyViewModel.a
            companyToUpdate.CityId = editCompanyViewModel.CityId;
            companyToUpdate.ProvinceId = editCompanyViewModel.ProvinceId;
            companyToUpdate.CompanyName = editCompanyViewModel.CompanyName;
            companyToUpdate.Telephone = editCompanyViewModel.Telephone;
            _companyRepository.Update(companyToUpdate);

        }

        public void UpdateById(string id)
        {
            throw new NotImplementedException();
        }
    }
}