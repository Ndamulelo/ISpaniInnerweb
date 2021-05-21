using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IExperienceLevelService
    {
        void Create(ExperienceLevel experienceLevel);
        void Update(ExperienceLevel experienceLevel);
        ExperienceLevel Get(string id);
        IList<ExperienceLevel> GetAll();
    }
}
