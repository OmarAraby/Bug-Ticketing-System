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

            services.AddScoped<JWTService>();




            services.AddValidatorsFromAssembly(typeof(BusinessExtention).Assembly);

        }


    }
}
