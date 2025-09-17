using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Domain;
using UserManagementSystem.Domain.Repositorys;
using UserManagementSystem.Infrastructure;
using UserManagementSystem.Infrastructure.Repositories;


namespace UserManagementSystem.Api
{
    public static class WebModule
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped<ISendEmailRepository, SendEmailRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

