using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SoftGrid.Authorization;

namespace SoftGrid
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(SoftGridApplicationSharedModule),
        typeof(SoftGridCoreModule)
        )]
    public class SoftGridApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridApplicationModule).GetAssembly());
        }
    }
}