using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SubQuip.Business.Interfaces;
using SubQuip.Business.Logic;
using SubQuip.Data.Interfaces;
using SubQuip.Data.Logic;

namespace SubQuip.WebApi
{
    public static class BuildUnityContainer
    {
        public static IServiceCollection RegisterAddTransient(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(
                provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            #region Repository
            services.AddTransient<IEquipmentRepository, EquipmentRepository>();
            services.AddTransient<IMaterialRepository, MaterialRepository>();
            services.AddTransient<IRequestRepository, RequestRepository>();
            services.AddTransient<IBillOfMaterialRepository, BillOfMaterialRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
			services.AddTransient<IGraphRepository, GraphRepository>();
            services.AddTransient<IPartnerRepository, PartnerRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion

            #region Services
            services.AddTransient<IEquipmentManagerService, EquipmentManagerService>();
            services.AddTransient<IMaterialManagerService, MaterialManagerService>();
            services.AddTransient<IRequestManagerService, RequestManagerService>();
            services.AddTransient<IBillOfMaterialsManagerService, BillOfMaterialsManagerService>();
            services.AddTransient<IFileManagerService, FileManagerService>();
            services.AddTransient<IPartnerManagerService, PartnerManagerService>();
            services.AddTransient<ILocationManagerService, LocationManagerService>();
            services.AddTransient<IUserManagerService, UserManagerService>();
            #endregion

            return services;
        }
    }
}
