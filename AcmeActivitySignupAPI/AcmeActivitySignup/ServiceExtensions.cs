using System.Collections.Generic;
using AcmeActivitySignup.Data;
using AcmeActivitySignup.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace AcmeActivitySignup
{
    /// <summary>
    /// Service Extension methods to help clean up startup.cs and make it more readable
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configure cors to allow anyorigin/method/header
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
        }
        /// <summary>
        /// Configure Swagger UI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("acme", new OpenApiInfo { Title = "Acme Activity Signup", Version = "v1" });
                c.IncludeXmlComments(string.Format(@"AcmeActivitySignup.xml"));
            });
        }


        /// <summary>
        /// Configure the datastore in the form of a DbContext using MySQL
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Scoped
            );
        }

        
        /// <summary>
        /// Configure dependency injection for all interfaces/implementations
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IAttendeeService, AttendeeService>();
        }
    }
    
    
}