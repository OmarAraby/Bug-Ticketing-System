using BugTicketingSystem.BL.Managers.Bug;
using BugTicketingSystem.BL.Managers.Project;
using BugTicketingSystem.BL.Managers.User;
using BugTicketingSystem.BL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BugTicketingSystem.BL
{
    public static class BusinessExtention
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IProjectManager, ProjectManager>();
            services.AddScoped<IBugManager, BugManager>();

            services.AddScoped<JWTService>();




            services.AddValidatorsFromAssembly(typeof(BusinessExtention).Assembly);

        }


    }
}
