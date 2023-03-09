using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SoftGrid.ApiClient;
using SoftGrid.Mobile.MAUI.Core.ApiClient;

namespace SoftGrid
{
    [DependsOn(typeof(SoftGridClientModule), typeof(AbpAutoMapperModule))]

    public class SoftGridMobileMAUIModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MAUIApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridMobileMAUIModule).GetAssembly());
        }
    }
}