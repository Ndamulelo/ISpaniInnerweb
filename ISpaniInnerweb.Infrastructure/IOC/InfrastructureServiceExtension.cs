﻿using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Helpers;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Settings;
using ISpaniInnerweb.infrastructure.DbContexts;
using ISpaniInnerweb.Infrastructure.Helpers;
using ISpaniInnerweb.Infrastructure.Repositories;
using ISpaniInnerweb.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ISpaniInnerweb.Infrastructure.IOC
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddRepositories();
            return service;
        }

        static IServiceCollection AddRepositories(this IServiceCollection service)
        {
 

        service.AddDbContext<HustlersContext>();
            service.AddTransient<IRepository<User>, Repository<User>>();
            service.AddTransient<IRepository<JobSeeker>, Repository<JobSeeker>>();
            service.AddTransient<IRepository<Ethnicity>, Repository<Ethnicity>>();
            service.AddTransient<IRepository<WorkExperience>, Repository<WorkExperience>>();
            service.AddTransient<IRepository<Title>, Repository<Title>>();
            service.AddTransient<IRepository<SkillLevel>, Repository<SkillLevel>>();
            service.AddTransient<IRepository<Recruiter>, Repository<Recruiter>>();
            service.AddTransient<IRepository<Qualification>, Repository<Qualification>>();
            service.AddTransient<IRepository<Province>, Repository<Province>>();
            service.AddTransient<IRepository<Language>, Repository<Language>>();
            service.AddTransient<IRepository<LanguageLevel>, Repository<LanguageLevel>>();
            service.AddTransient<IRepository<JobCategory>, Repository<JobCategory>>();
            service.AddTransient<IRepository<Gender>, Repository<Gender>>();
            service.AddTransient<IRepository<Education>, Repository<Education>>();
            service.AddTransient<IRepository<Company>, Repository<Company>>();
            service.AddTransient<IRepository<City>, Repository<City>>();
            service.AddTransient<IRepository<Address>, Repository<Address>>();

            service.AddTransient<IRepository<Attachment>, Repository<Attachment>>();
            service.AddTransient<IRepository<ExperienceLevel>, Repository<ExperienceLevel>>();
            service.AddTransient<IRepository<JobAdvert>, Repository<JobAdvert>>();
            service.AddTransient<IRepository<JobSeekerJobApplications>, Repository<JobSeekerJobApplications>>();
            service.AddTransient<IRepository<JobSeekerSkills>, Repository<JobSeekerSkills>>();
            service.AddTransient<IRepository<JobSeekerLanguages>, Repository<JobSeekerLanguages>>();
            service.AddTransient<IRepository<JobType>, Repository<JobType>>();
            service.AddTransient<IRepository<Skill>, Repository<Skill>>();
            service.AddTransient<IRepository<WorkExperience>, Repository<WorkExperience>>();
            service.AddTransient<IPasswordEncryption, PasswordEncryption>();
            service.AddTransient<ICompanyRepository, CompanyRepository>();
            service.AddTransient<IStringManipulator, StringManipulator>();
            service.AddTransient<IEmailSettings, EmailSettings>();
            service.AddTransient<IPDFGenerator, PDFGenerator>();
            return service;
        }
    }
}
