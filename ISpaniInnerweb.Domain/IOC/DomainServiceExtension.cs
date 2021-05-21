using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Interfaces.Services.Communication;
using ISpaniInnerweb.Domain.Interfaces.Settings;
using ISpaniInnerweb.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ISpaniInnerweb.Domain.IOC
{
    public static class DomainServiceExtension
    {
        public static IServiceCollection AddDomain(this IServiceCollection service)
        {
            service.AddServices();
            service.AddFactories();
            return service;
        }

        static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddTransient<IJobSeekerService, JobSeekerService>();
            service.AddTransient<IAttachmentService, AttachmentService>();
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IRecruiterService, RecruiterService>();
            service.AddTransient<IAddressService, AddressService>();
            service.AddTransient<ICompanyService, CompanyService>();
            service.AddTransient<IJobAdvertService, JobAdvertService>();
            service.AddTransient<IProvinceService, ProvinceService>();
            service.AddTransient<ICityService, CityService>();
            service.AddTransient<IExperienceLevelService, ExperienceLevelService>();
            service.AddTransient<IJobCategoryService, JobCategoryService>();
            service.AddTransient<IJobTypeService, JobTypeService>();
            service.AddTransient<IWorkExperienceService, WorkExperienceService>();
            service.AddTransient<IEducationService, EducationService>();
            service.AddTransient<ILanguageService, LanguageService>();
            service.AddTransient<ISkillsService, SkillsService>();
            service.AddTransient<IEmailService, EmailService>();
            
            return service;
        }

        static IServiceCollection AddFactories(this IServiceCollection service)
        {

            return service;
        }

        public static IServiceCollection AddLazyResolution(this IServiceCollection services)
        {
            return services.AddTransient(
                typeof(Lazy<>),
                typeof(LazilyResolved<>));
        }

        private class LazilyResolved<T> : Lazy<T>
        {
            public LazilyResolved(IServiceProvider serviceProvider)
                : base(serviceProvider.GetRequiredService<T>)
            {
            }
        }
    }
}
