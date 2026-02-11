using FITR_DC_FORM.Filters;
using FITR_DC_FORM.Repositories.Interfaces;
using FITR_DC_FORM.Repositories.Repository;
using FITR_DC_FORM.Services.Interfaces;
using FITR_DC_FORM.Services.Repo_Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FITR_DC_FORM.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            // 🔹 Database Connection (Dapper)
            services.AddScoped<IDbConnection>(sp =>
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
            // REPO layer 
            services.AddScoped<IFitrRepository, FitrRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFitrVisualMasterRepository, FitrVisualMasterRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IPrintCompanyRepository, PrintCompanyRepository>();
            services.AddScoped<IUserPageRightRepository, UserPageRightRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            
            //services layer

            services.AddScoped<IFitrService, FitrService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFitrVisualMasterService, FitrVisualMasterService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPrintCompanyService, PrintCompanyService>();
            services.AddScoped<IUserPageRightService, UserPageRightService>();
            services.AddScoped<IPermissionService, PermissionService>();
           



            return services;
        }


    }
}
