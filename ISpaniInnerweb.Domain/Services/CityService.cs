using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class CityService : ICityService
    {
        private readonly ILogger<CityService> _logger;
        private readonly IRepository<City> _cityRepository;

        public CityService(ILogger<CityService> logger, IRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
            _logger = logger;
        }
        public void Create(City city)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public City Get(string id)
        {
            var city = new City();

          
                city = _cityRepository.Get(id);
                _logger.LogInformation("City " + city.Id + "retrieved");
            
      

            return city;
        }

        public IList<City> GetAll()
        {
            return _cityRepository.Get();
        }

        public City GetByCompany(string id)
        {
               
            var city = _cityRepository.FindByCondition(x => x.Id.Equals(id) && x.IsActive == true);
            
            return (City)city;
        }

        public void Update(City city)
        {
            throw new NotImplementedException();
        }
    }
}
