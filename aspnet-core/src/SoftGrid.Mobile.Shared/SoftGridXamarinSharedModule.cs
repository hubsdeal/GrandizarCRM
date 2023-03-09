using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace SoftGrid
{
    [DependsOn(typeof(SoftGridClientModule), typeof(AbpAutoMapperModule))]
    public class SoftGridXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridXamarinSharedModule).GetAssembly());
        }
    }
}