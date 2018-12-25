using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RS.Data.Interfaces;
using RS.Data.Logic;
using RS.Service;
using RS.Service.Interfaces;
using RS.Service.Logic;


namespace RS.Web
{
    public static class BuildUnityContainer
    {
        public static IServiceCollection RegisterAddTransient(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(
                provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            #region Repository
            services.AddTransient<IApprovalRepository, ApprovalRepository>();
            services.AddTransient<ICandidateRepository, CandidateRepository>();
            services.AddTransient<IOpeningRepository, OpeningRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISkillRepository, SkillRepository>();
            services.AddTransient<IQualificationRepository, QualificationRepository>();
            #endregion

            #region Services
            services.AddTransient<IApprovalManagerService, ApprovalManagerService>();
            services.AddTransient<ICandidateManagerService, CandidateManagerService>();
            services.AddTransient<IOpeningManagerService, OpeningManagerService>();
            services.AddTransient<IRoleManagerService, RoleManager>();
            services.AddTransient<IUserManagerService, UserManagerService>();
            services.AddTransient<ISkillManagerService, SkillManagerService>();
            services.AddTransient<IQualificationManagerService, QualificationManagerService>();
            #endregion

            return services;
        }
    }
}
