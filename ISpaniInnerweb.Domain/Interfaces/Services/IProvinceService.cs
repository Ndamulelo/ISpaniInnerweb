using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IProvinceService
    {
        void Create(Province province);
        void Update(Province province);
        Province Get(string id);
        IList<Province> GetAll();
        void Delete(string id);
    }
}
