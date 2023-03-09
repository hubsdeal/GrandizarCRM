using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace SoftGrid.Startup
{
    [DependsOn(typeof(SoftGridCoreModule))]
    public class SoftGridGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}